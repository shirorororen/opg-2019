namespace opg_201910_interview.Business.Contract
{
    public class ClientSettings
    {
        public string ClientId { get; set; }
        public string FileDirectoryPath { get; set; }
        public string[] AcceptedFileName { get; set; }
        public string DateFormat { get; set; }
        public string DateDelimiter { get; set; }
    }
}
