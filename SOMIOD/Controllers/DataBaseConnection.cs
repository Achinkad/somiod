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

        protected void Connect()
        {
            conn = null;
            conn = new SqlConnection(connectionString);
            conn.Open();
        }

        protected void Disconnect()
        {
            conn.Close();
        }

        protected void SetSqlComand(string sql)
        {
            this.sql = sql;
        }

        protected void Select() 
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            ReaderIterator(reader);
            reader.Close();
        }

        protected void Select(int id)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = cmd.ExecuteReader();
            ReaderIterator(reader);
            reader.Close();
        }

        protected void SelectByName(string name)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", name);

            SqlDataReader reader = cmd.ExecuteReader();
            ReaderIterator(reader);
            reader.Close();
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

        public abstract void ReaderIterator(SqlDataReader reader);
    }
}