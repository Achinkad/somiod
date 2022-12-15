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
            setSqlComand("SELECT * FROM Data");

            try
            {
                connect();
                Select();
                disconnect();  

            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    disconnect();
                }
            }
            disconnect();
            return new List<Data>(this.data_list);
        }

        //Store a new Data and their values in the database

        public bool Store(Data value)
        {
            try
            {
                connect();
                // id, string mname, DateTime creation_dt, int parent

                string sql = "INSERT INTO DATA VALUES(@content,@parent,@creation_dt)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@content", value.Content);
                cmd.Parameters.AddWithValue("@parent", value.Parent);
                cmd.Parameters.AddWithValue("@creation_dt", value.Creation_dt);
                setSqlComand(sql);

                int numRow = InsertOrUpdate(cmd);
                disconnect();
                if (numRow == 1)
                {
                    return true;
                }
                return false;

            } catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                return false;
            }
        }

        //Delete a application from database
        public Boolean DeleteData(int id)
        {
            try
            {
                connect();

                setSqlComand("DELETE FROM Data WHERE Id=@IdApp");
                int numRow = Delete(id);
                disconnect();
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
                    disconnect();
                }
                return false;
            }
        }
        public override void readerIterator(SqlDataReader reader)
        {
            this.data_list = new List<Data>();
            while (reader.Read())
            {
                Data data = new Data
                {
                    Id = (int)reader["Id"],
                    Content = (string)reader["Content"],
                    Parent = (int)reader["Parent"],
                    Creation_dt =(DateTime)reader["Creation_dt"],
                };
                this.data_list.Add(data);
            }
        }
    }
}