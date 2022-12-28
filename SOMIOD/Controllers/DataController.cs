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
            this.data_list = new List<Data>();
        }
        //Select Aplications
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
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    Disconnect();
                }
            }
            Disconnect();
            return new List<Data>(this.data_list);
        }

        public Data GetData(int id)
        {
            try {

                Connect();
                SetSqlComand("SELECT * FROM data WHERE Id=@id");
                Select(id);
                Disconnect();

                if (this.data_list[0] == null)
                {
                    return null;
                }
                return this.data_list[0];

            }
            catch (Exception)
            {
                //fechar ligação à DB
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    Disconnect();
                }
                return null;
                //return BadRequest();
            }
        }

        //Store a new Data and their values in the database

        public bool Store(Data value, int parent_id)
        {
            try
            {
                Connect();

                string sql = "INSERT INTO data VALUES(@Content, @Creation_dt, @Parent)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Creation_dt", DateTime.Now);
                cmd.Parameters.AddWithValue("@Content", value.Content);
                cmd.Parameters.AddWithValue("@Parent", parent_id);

                int numRow = InsertOrUpdate(cmd);
                
                Disconnect();

                return numRow == 1;
            } catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                return false;
            }
        }

        //Delete a application from database
        public Boolean DeleteData(int id)
        {
            try
            {
                Connect();

                SetSqlComand("DELETE FROM Data WHERE Id=@IdApp");
                int numRow = Delete(id);
                Disconnect();
                if (numRow == 1)
                {
                    return true;
                }
                return false;

            }
            catch (Exception)
            {
                //fechar ligação à DB
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    Disconnect();
                }
                return false;
            }
        }
        public override void ReaderIterator(SqlDataReader reader)
        {
            this.data_list = new List<Data>();
            while (reader.Read())
            {
                Data data = new Data
                {
                    Id = (int)reader["Id"],
                    Content = (string)reader["Content"],
                    Parent = (int)reader["Parent"],
                    Creation_dt = new DateTime(),
                };
                this.data_list.Add(data);
            }
        }
    }
}