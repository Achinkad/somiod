using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using SOMIOD.Models;

namespace SOMIOD.Controllers
{
    public class ApplicationController : DatabaseConnection
    {
        private List<Application> applications;

        public ApplicationController()
        {
            this.applications = new List<Application>();   
        }
        
        public List<Application> GetApplications()
        {
            List<Application> applications = new List<Application>();
            
            SetSqlComand("SELECT * FROM applications");
            
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

            return new List<Application>(this.applications);
        }

        public Application GetApplication(int id)
        {
            try
            {
                Connect();
                SetSqlComand("SELECT * FROM applications WHERE Id = @id");
                Select(id);
                Disconnect();

                return applications.Count() > 0 ? applications[0] : null;
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
            }
        }

        public int GetApplicationByName(string name)
        {
            try
            {
                Connect();
                SetSqlComand("SELECT * FROM applications WHERE name = @name");
                SelectByName(name);
                Disconnect();

                return applications.Count() > 0 ? applications[0].Id : -1;
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
            }
        }

        public bool Store(Application value)
        {
            string sql = "INSERT INTO applications VALUES(@name, @creation_dt)";

            try
            {
                Connect();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", value.Name);
                cmd.Parameters.AddWithValue("@creation_dt", DateTime.Now);

                int n = InsertOrUpdate(cmd);
                
                Disconnect();

                return n > 0;
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
            }
        }

        public bool Update(Application value, int id)
        {
            string update_query = "UPDATE applications SET name = @name, creation_dt = @creation_dt WHERE Id = @id";

            try
            {
                Connect();

                SqlCommand cmd = new SqlCommand(update_query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", value.Name);
                cmd.Parameters.AddWithValue("@creation_dt", DateTime.Now);
                
                int n = InsertOrUpdate(cmd);

                Disconnect();

                return n > 0;
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
            }
        }

        public bool DeleteApplication(int id)
        {
            try
            {
                Connect();
                SetSqlComand("DELETE FROM applications WHERE Id=@id");
                int n = Delete(id);
                Disconnect();

                return n == 1;
            }
            catch (Exception exception) { 
                if (conn.State == System.Data.ConnectionState.Open) Disconnect(); 
                throw exception; 
            }
        }

        public override void ReaderIterator(SqlDataReader reader)
        {
            while (reader.Read())
            {
                string date = (string)reader["Creation_dt"];
                Console.WriteLine(date);
                Application application = new Application
                {
                    Id = (int) reader["Id"],
                    Name = (string) reader["Name"],
                    Creation_dt = (DateTime) reader["Creation_dt"],
                };

                applications.Add(application);
            }
        }
    }
}