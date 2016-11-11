using System;
using System.Data;
using System.Data.OleDb;

namespace WebReader.Service
{
    public class DBService
    {
        private OleDbConnection conn = null;

        public DBService()
        {
            conn = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;data source="+ AppDomain.CurrentDomain.BaseDirectory + "WebReader.mdb");
            conn.Open();
        }

        //打开连接
        public void Open()
        {
            conn.Open();
        }

        //关闭连接
        public void Close()
        {
            conn.Close();
        }

        //查询
        public DataTable Query(string sql)
        {
            DataTable dataTable = new DataTable();
            new OleDbDataAdapter(sql, conn).Fill(dataTable);
            return dataTable;
        }

        //更新
        public bool UpdateTable(string sql)
        {
            return new OleDbCommand(sql, conn).ExecuteNonQuery() > 0 ? true : false;
        }
    }
}