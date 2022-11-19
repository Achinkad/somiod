using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SOMIOD.Controllers
{
    public abstract class DataBaseConnection
    {
        protected SqlConnection conn;
        protected String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\marco\\Desktop\\IS coisas\\SOMIOD\\SOMIOD\\App_Data\\Database1.mdf\";Integrated Security=True";
        protected string sql = "";

        protected int connect()
        {
            this.conn = null;
            try
            {
                this.conn = new SqlConnection(connectionString);
                this.conn.Open();

            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;
        }

        protected void disconnect()
        {
            conn.Close();
        }

        protected void setSqlComand(string sql)
        {
            this.sql = sql;
        }

        protected void SelectAbstract() 
        {
            SqlCommand cmd = new SqlCommand(this.sql, this.conn);

            SqlDataReader reader = cmd.ExecuteReader();
            readerIterator(reader);

            reader.Close();
        }

        public abstract void readerIterator(SqlDataReader reader);



        protected void InsertAbstract()
        {

        }

        protected void UpdateAbstract()
        {

        }

        protected void DeleteAbstract()
        {

        }
    }
}