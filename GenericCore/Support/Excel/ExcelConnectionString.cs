using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Support.Excel
{
    public class ExcelConnectionString
    {
        public static string ToExcelV8(string excelFileName)
        {
            excelFileName.AssertNotNull("excelFileName");
            return $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={excelFileName};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
        }

        public static string ToExcelV12(string excelFileName, bool useHeaders = true)
        {
            excelFileName.AssertNotNull("excelFileName");
            return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={excelFileName};Extended Properties=\"Excel 12.0;HDR={(useHeaders ? "Yes" : "No")};IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text;READONLY=TRUE;\"";
        }

        public static string ToExcelV14(string excelFileName)
        {
            excelFileName.AssertNotNull("excelFileName");
            return $"Provider=Microsoft.ACE.OLEDB.14.0;Data Source={excelFileName};Extended Properties=\"Excel 12.0;HDR=YES\"";
        }
    }
}
