using opg_201910_interview.Business.Contract;
using System;
using System.Collections.Generic;

namespace opg_201910_interview.Business.Interface
{
    public interface IFileProcess
    {
        public List<UploadFile> GetUploadFiles();

        public (string, DateTime?) GetDateParts(string fileName);

    }
}
