﻿using System;
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
            string path = @"C:\Temp\lol.xlsx";
            OleDbExcelReader reader = new OleDbExcelReader(ExcelOleDbConnectionString.ToExcelV12(path));
            string[] sheets = reader.ExtractSheetNames();
            DataSet ds = reader.ReadData(sheets);
        }
    }
}