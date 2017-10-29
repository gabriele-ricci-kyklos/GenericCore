using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GenericCore.Support.Excel
{
    public class OleDbExcelReader
    {
        public string ConnectionString { get; private set; }

        public OleDbExcelReader(string connectionString)
        {
            connectionString.AssertNotNull("connectionString");

            ConnectionString = connectionString;
        }

        public DataSet ReadData(string[] sheetNames)
        {
            return ReadDataImpl(ConnectionString, sheetNames, (name, conn) => new DataTable(name), null);
        }

        public DataSet ReadMixedTypesData(string[] sheetNames)
        {
            string connectionStringWhitoutHDR = ConnectionString.ReplaceInsensitive("HDR=YES", "HDR=NO");

            Action<DataTable> afterFilledAction = dt => AdjustColumnsInDatatable(dt, ConnectionString);

            return ReadDataImpl(connectionStringWhitoutHDR, sheetNames, (name, conn) => new DataTable(name), afterFilledAction);
        }

        private DataSet ReadDataImpl(string connectionString, string[] sheetNames, Func<string, OleDbConnection, DataTable> tableCreator, Action<DataTable> afterFilledAction)
        {
            DataSet dataset = new DataSet("ExcelData");

            if (!sheetNames.IsNullOrEmptyList())
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    foreach (string sheet in sheetNames)
                    {
                        DataTable table = tableCreator(sheet, connection);
                        FillData(table, connection, sheet);
                        if (afterFilledAction.IsNotNull())
                        {
                            afterFilledAction(table);
                        }
                        dataset.Tables.Add(table);
                    }
                }
            }

            return dataset;
        }

        private void AdjustColumnsInDatatable(DataTable table, string connectionString)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                DataTable columnsTable = CreateDynamicTable(connection, table.TableName);
                if (table.Rows.Count > 0)
                {
                    table.Rows[0].Delete();
                    table.AcceptChanges();
                }
                for (int i = 0; i < columnsTable.Columns.Count; ++i)
                {
                    table.Columns[i].ColumnName = columnsTable.Columns[i].ColumnName;
                }
            }
        }

        private DataTable CreateDynamicTable(OleDbConnection connection, string sheet)
        {
            DataTable table = new DataTable(sheet);
            const string selectPattern = "select * from [{0}] where 1 = 0";
            FillWithCommand(selectPattern, connection, sheet, table);
            DataTable newTable = new DataTable(sheet);

            table
                .Columns
                .AsEnumerable()
                .ForEach(dc => newTable.Columns.Add(dc.ColumnName, typeof(string)));

            return newTable;
        }

        private void FillData(DataTable table, OleDbConnection connection, string sheet)
        {
            if (sheet.IsNullOrEmpty())
            {
                return;
            }

            const string selectPattern = "select * from [{0}]";
            FillWithCommand(selectPattern, connection, sheet, table);
        }

        private void FillWithCommand(string selectPattern, OleDbConnection connection, string sheet, DataTable table)
        {
            using (OleDbDataAdapter command = new OleDbDataAdapter(selectPattern, connection))
            {
                command.SelectCommand.CommandText = string.Format(selectPattern, (sheet[sheet.Length - 1] == '$') ? sheet : string.Format("{0}{1}", sheet, "$"));
                command.TableMappings.Add(table.TableName, sheet);
                command.Fill(table);
            }
        }

        public string[] ExtractSheetNames()
        {
            return ExtractSheetNames(ConnectionString);
        }

        private string[] ExtractSheetNames(string connectionString)
        {
            List<string> sheetNames = new List<string>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                var dtSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                foreach (DataRow row in dtSchema.Rows)
                {
                    sheetNames.Add(row.Field<string>("TABLE_NAME"));
                }
            }

            return sheetNames.ToArray();
        }
    }
}
