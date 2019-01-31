﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Support.Excel
{
    public interface IExcelDataAccessor
    {
        DataSet ReadData(string[] sheetNames);
        DataSet ReadDataWithMixedData(string[] sheetNames);
        string[] ExtractSheetNames();
    }
}
