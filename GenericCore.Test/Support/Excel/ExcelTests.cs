using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCore.Support.Excel;
using System.Data;

namespace GenericCore.Test.Support.Excel
{
    [TestClass]
    public class ExcelTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string path = @"C:\Temp\data_sheet_20171029135142.xlsx";
            OleDbExcelDataAccessor reader = new OleDbExcelDataAccessor(OleDbExcelDataAccessor.ConnectionStrings.ToExcelV12(path));
            string[] sheets = reader.ExtractSheetNames();
            DataSet ds = reader.ReadDataWithMixedData(sheets);
        }
    }
}
