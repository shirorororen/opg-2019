using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using opg_201910_interview.Business.Contract;
using opg_201910_interview.Business.Interface;

using System;
using System.Collections.Generic;
using System.Linq;

namespace opg_201910_interview.Business.Service
{
    public class FileProcess : IFileProcess
    {
        private readonly IOptions<ClientSettings> _clientSettings;
        private readonly IFileProvider _fileProvider;
        private readonly IDirectoryContents _files;
        private readonly List<UploadFile> _uploadedFiles;

        public FileProcess(IOptions<ClientSettings> clientSettings, IFileProvider fileProvider)
        {
            _clientSettings = clientSettings;
            _fileProvider = fileProvider;
            _files = _fileProvider.GetDirectoryContents(_clientSettings.Value.FileDirectoryPath);
            _uploadedFiles = ExtractFiles();
        }

        public List<UploadFile> GetUploadFiles()
        {
            var acceptedFiles = new List<UploadFile>();

            foreach (var format in _clientSettings.Value.AcceptedFileName)
            {
                var orderedFiles = _uploadedFiles.Where(x => x.ShortName.Equals(format, StringComparison.InvariantCultureIgnoreCase) && x.IsValid)
                                            .OrderBy(o => o.FileDate);
                acceptedFiles.AddRange(orderedFiles);
            }
            return acceptedFiles;
        }

        private List<UploadFile> ExtractFiles()
        {
            var uploadedFiles = new List<UploadFile>();
            _files.ToList().ForEach(x => uploadedFiles.Add(new UploadFile
            {
                ClientId = _clientSettings.Value.ClientId,
                DateUpload = x.LastModified.DateTime,
                FileName = x.Name,
                ShortName = GetDateParts(x.Name).Item1,
                FileDate = GetDateParts(x.Name).Item2
            }));

            return uploadedFiles;
        }

        public (string, DateTime?) GetDateParts(string fileName)
        {
            string extension = System.IO.Path.GetExtension(fileName);
            var split = fileName.Substring(0, fileName.Length - extension.Length).Split(_clientSettings.Value.DateDelimiter, 2);
            var date = split.Length > 1 ? split[1] : null;
            DateTime processedDate;

            if (date != null && DateTime.TryParseExact(date, _clientSettings.Value.DateFormat, null,
                System.Globalization.DateTimeStyles.None, out processedDate))
                return (split[0], processedDate);
            else
                return (split[0], null);
        }
    }
}
