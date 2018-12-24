using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCore.Support;

namespace GenericCore.Test.Support.ExtensionMethods
{
    [TestClass]
    public class DataExtensionMethodsTests
    {
        [TestMethod]
        public void ToEntityListMethodsTest()
        {
            DataTable table = new DataTable("Orders");
            table.Columns.Add("MyStrValue", typeof(string));
            table.Columns.Add("MyIntValue", typeof(int));

            DataRow row1 = table.NewRow();
            row1[0] = "value1";
            row1[1] = 1;
            table.Rows.Add(row1);

            DataRow row2 = table.NewRow();
            row2[0] = "value2";
            row2[1] = 2;
            table.Rows.Add(row2);

            var list = table.ToEntityList<MyType>();
            Assert.IsTrue(list.Any(x => x.MyIntValue == 1));
            Assert.IsTrue(list.Any(x => x.MyStrValue == "value2"));
        }
    }

    class MyType
    {
        public string MyStrValue { get; set; }
        public int MyIntValue { get; set; }
    }
}
