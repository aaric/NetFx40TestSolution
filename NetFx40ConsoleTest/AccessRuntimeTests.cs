using System;
using System.Data;
using System.Data.OleDb;

namespace NetFx40ConsoleTest
{
    /// <summary>
    /// Microsoft Access 2013 Runtime
    /// https://www.microsoft.com/zh-cn/download/details.aspx?id=39358
    ///
    /// <code>FreeSql.Provider.MsAccess --version 3.2.801</code>
    /// </summary>
    public class AccessRuntimeTests
    {
        public static void Main1(string[] args)
        {
            // connect
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\\test_file\\test.mdb";
            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();

            // query
            if (ConnectionState.Open == connection.State)
            {
                // select
                string sql = @"SELECT xh, symc FROM syk";
                DataSet dataSet = new DataSet();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                dataAdapter.Fill(dataSet);

                Console.WriteLine("-- Data --");
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    Console.WriteLine("{0} - {1}", row["xh"], row["symc"]);
                }

                // close
                connection.Close();
            }
        }
    }
}