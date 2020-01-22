using Xunit;
using Moq;
using opg_201910_interview.Business.Interface;
using opg_201910_interview.Business.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using opg_201910_interview.Business.Service;
using System.IO;
using System.Linq;

namespace FilesUnitTest
{
    public class FileTests
    {
        // Not really exposed to unit testing :)

        private IFileProcess _fileProcess;

        Mock<IOptions<ClientSettings>> _clientSetting;
        IFileProvider fileProvider;

        public FileTests()
        {
            var builder = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();

            IConfiguration config = builder.Build();

            var client = config.GetSection(nameof(ClientSettings)).Get<ClientSettings>();

            //trying moq
            _clientSetting = new Mock<IOptions<ClientSettings>>();
            _clientSetting.Setup(o => o.Value).Returns(client);

            var currentDirectory = Directory.GetCurrentDirectory();
            // bad code 
            var filePath = Path.GetDirectoryName($"{currentDirectory.Substring(0, currentDirectory.IndexOf("opg-201910Base-master") + 22)}opg-201910Base-master\\");

            fileProvider = new PhysicalFileProvider(filePath);
            _fileProcess = new FileProcess(_clientSetting.Object, fileProvider);
        }

        [Theory]
        [InlineData("blaze-2018-05-01.xml", "01/05/2018 12:00:00 AM")]
        public void Test_Get_Date_Part(string fileName, string dateExpected)
        {
            var result = _fileProcess.GetDateParts(fileName);
            Assert.Equal(dateExpected, result.Item2.ToString());
        }

        [Theory]
        [InlineData("blaze-2018-05-01.xml", "blaze")]
        public void Test_Get_Name_Part(string fileName, string dateExpected)
        {
            var result = _fileProcess.GetDateParts(fileName);
            Assert.Equal(dateExpected, result.Item1.ToString());
        }

        [Theory]
        [InlineData(new object[] { new string[] { "shovel-2000-01-01.xml", "waghor-2012-06-20.xml", 
            "blaze-2018-05-01.xml", "blaze-2019-01-23.xml", "discus-2015-12-16.xml" } })]
        public void Test_Get_Client_Files(string[] files)
        {
            var result = _fileProcess.GetUploadFiles();
            var clientFiles = result.Select(x => x.FileName);

            Assert.Equal(files, clientFiles);
        }
    }
}
