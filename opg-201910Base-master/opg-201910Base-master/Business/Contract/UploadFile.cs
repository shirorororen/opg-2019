using System;
using System.Text.Json.Serialization;

namespace opg_201910_interview.Business.Contract
{
    public class UploadFile
    {
        public string FileName { get; set; }
        [JsonIgnore]
        public string ShortName { get; set; }
        [JsonIgnore]
        public DateTime DateUpload { get; set; }
        [JsonIgnore]
        public string ClientId { get; set; }
        [JsonIgnore]
        public DateTime? FileDate { get; set; }
        [JsonIgnore]
        public bool IsValid => FileDate.HasValue && !string.IsNullOrEmpty(ShortName);
    }
}
