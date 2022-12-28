using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Windows;

namespace SOMIOD.Controllers
{
    public abstract class DatabaseConnection
    {
        protected SqlConnection conn;

        protected String connectionString = Properties.Settings.Default.ConnStr;
        protected string sql = "";

        protected void connect()
        {
          
            this.conn = null;
            this.conn = new SqlConnection(connectionString);
            this.conn.Open();
            Console.WriteLine("Connection DONE");

        }

        protected void disconnect()
        {
            conn.Close();
            Console.WriteLine("DISCONECT");
        }

        protected void setSqlComand(string sql)
        {
            this.sql = sql;
            Console.WriteLine("SQL was DEFINED");
        }

        protected void Select() 
        {
            SqlCommand cmd = new SqlCommand(this.sql, this.conn);

            SqlDataReader reader = cmd.ExecuteReader();
            readerIterator(reader);

            reader.Close();
        }

        public abstract void readerIterator(SqlDataReader reader);

        protected void Select(int id)
        {
            SqlCommand cmd = new SqlCommand(this.sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = cmd.ExecuteReader();
            readerIterator(reader);
            reader.Close();
            Console.WriteLine("Select DONE");
        }

        protected void SelectByName(string name)
        {
            SqlCommand cmd = new SqlCommand(this.sql, conn);
            cmd.Parameters.AddWithValue("@name", name);

            SqlDataReader reader = cmd.ExecuteReader();
            readerIterator(reader);
            reader.Close();
            Console.WriteLine("Select DONE");
        }

        protected int InsertOrUpdate(SqlCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        protected int Delete(int id)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery();
        }
    }
}