using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace GenericCore.Support.Excel
{
    public class ExternalExcelReader
    {
        public string FilePath { get; set; }
        public ExternalExcelReader(string filePath)
        {
            filePath.AssertNotNull("filePath");
            FilePath = filePath;
        }

        private DataSet ReadData()
        {
            DataSet resultSet = null;

            using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    resultSet = reader.AsDataSet();
                }
            }

            return resultSet;
        }
    }
}
