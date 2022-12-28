using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SOMIOD.Controllers
{
    public class DataController : DatabaseConnection
    {
        private List<Data> data_list;

        public DataController()
        {
            data_list = new List<Data>();
        }

        public List<Data> GetData()
        {
            List<Data> data_list = new List<Data>();
            SetSqlComand("SELECT * FROM data");

            try
            {
                Connect();
                Select();
                Disconnect();  
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
            }

            return new List<Data>(data_list);
        }

        public Data GetData(int id)
        {
            try
            {
                Connect();
                SetSqlComand("SELECT * FROM data WHERE Id = @id");
                Select(id);
                Disconnect();

                return data_list.Count() > 0 ? data_list[0] : null;
            }
            catch (Exception exception)
            { 
                if (conn.State == System.Data.ConnectionState.Open) Disconnect(); 
                throw exception; 
            }
        }

        public bool Store(Data value, int parent_id)
        {
            try
            {
                Connect();

                string sql = "INSERT INTO data VALUES(@content, @creation_dt, @parent)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@content", value.Content);
                cmd.Parameters.AddWithValue("@creation_dt", DateTime.Now);
                cmd.Parameters.AddWithValue("@parent", parent_id);

                int n = InsertOrUpdate(cmd);
                
                Disconnect();

                return n == 1;
            } 
            catch (Exception exception) 
            { 
                if (conn.State == System.Data.ConnectionState.Open) Disconnect(); 
                throw exception; 
            }
        }

        public bool DeleteData(int id)
        {
            try
            {
                Connect();
                SetSqlComand("DELETE FROM Data WHERE Id = @id");
                int n = Delete(id);
                Disconnect();

                return n == 1;
            }
            catch (Exception exception) 
            { 
                if (conn.State == System.Data.ConnectionState.Open) Disconnect(); 
                throw exception; 
            }
        }

        public override void ReaderIterator(SqlDataReader reader)
        {
            data_list = new List<Data>();
            while (reader.Read())
            {
                Data data = new Data
                {
                    Id = (int) reader["Id"],
                    Content = (string) reader["Content"],
                    Parent = (int) reader["Parent"],
                    Creation_dt = new DateTime(),
                };

                data_list.Add(data);
            }
        }
    }
}