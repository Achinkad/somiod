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
            string sql = "INSERT INTO DATA VALUES(@content,@parent,@creation_dt)";
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@content", value.Content);
                cmd.Parameters.AddWithValue("@parent", value.Parent);
                cmd.Parameters.AddWithValue("@creation_dt", value.Creation_dt);
                int numRegistos = cmd.ExecuteNonQuery();
                conn.Close();
                if (numRegistos > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                return false;
            }
        }

        //Delete a application from database
        public int Delete(int id)
        {
            string sql = "DELETE FROM Data WHERE Id=@IdApp";
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idApp", id);
                int numRegistos = cmd.ExecuteNonQuery();
                conn.Close();
                if (numRegistos == 1)
                {
                    return 1;
                }
                return -1;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                return -1;
            }
        }
        public override void readerIterator(SqlDataReader reader)
        {
            this.data_list = new List<Data>();
            while (reader.Read())
            {
                Data data = new Data
                (
                    (int)reader["Id"],
                    (string)reader["Content"],
                    (int)reader["Parent"],
                    (DateTime)reader["Creation_dt"],
                );
                this.data_list.Add(data);
            }
        }
    }
}