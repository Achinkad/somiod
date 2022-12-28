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
            
            setSqlComand("SELECT * FROM applications");
            
            try
            {
                connect();
                Select();
                disconnect();
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                throw exception;
            }

            return new List<Application>(this.applications);
        }

        public Application GetApplication(int id)
        {
            try
            {
                connect();
                setSqlComand("SELECT * FROM applications WHERE Id = @id");
                Select(id);
                disconnect();

                if (this.applications[0] == null)
                {
                    return null;
                }
                return this.applications[0];

            }
            catch (Exception)
            {
                //fechar ligação à DB
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    disconnect();
                }
                return null;
                //return BadRequest();
            }
        }

        public int GetApplicationByName(string name)
        {
            try
            {
                connect();
                setSqlComand("SELECT * FROM applications WHERE name = @name");
                SelectByName(name);
                disconnect();

                return applications.Count() > 0 ? applications[0].Id : -1;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                return -1;
            }
        }


        //Store a new Apllication and their values in the database
        public bool Store(Application value)
        {
            string sql = "INSERT INTO applications VALUES(@Name, @Creation_dt)";

            try
            {

                connect();
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Name", value.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", DateTime.Now);

                int numRegistos = InsertOrUpdate(cmd);
                
                if (numRegistos > 0)
                {
                    disconnect();
                    return true;
                }
                disconnect();
                return false;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    disconnect();
                return false;
            }
        }

        public bool Update(Application value, int id)
        {
            string update_query = "UPDATE applications SET name = @name, creation_dt = @creation_dt WHERE Id = @id";

            try
            {
                connect();

                SqlCommand cmd = new SqlCommand(update_query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", value.Name);
                cmd.Parameters.AddWithValue("@creation_dt", DateTime.Now);
                int rows = InsertOrUpdate(cmd);

                disconnect();

                return rows > 0;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                return false;
            }
        }

        //Add a reference to an Application in the Module database table
        public int AddModule(int id_app, int id_module)
        {
            string sql = "INSERT INTO modules VALUES (@parent) WHERE id=@IdApp";
            //SqlConnection conn = null;
            try
            {
                /*
                conn = new SqlConnection(connectionString);
                conn.Open();
                */
                connect();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idApp", id_app);
                cmd.Parameters.AddWithValue("@parent", id_module);
                int numRegistos = InsertOrUpdate(cmd);
                disconnect();
              //  int numRegistos = cmd.ExecuteNonQuery();
               // conn.Close();
                if (numRegistos == 1)
                {
                    return 1;
                }
                return -1;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    //conn.Close();
                    disconnect();
                return -1;
            }
        }
        //Remove a reference to an Application in the Module database table

        public int RemoveModule(int id_app, int id_module)
        {
            string sql = "DELETE FROM modules VALUES (@parent) WHERE id=@IdApp";
            //SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idApp", id_app);
                cmd.Parameters.AddWithValue("@parent", id_module);
                int numRegistos = InsertOrUpdate(cmd);
                //int numRegistos = cmd.ExecuteNonQuery();
                //conn.Close();
                disconnect();
                if (numRegistos == 1)
                {
                    return 1;
                }
                return -1;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    //conn.Close();
                    disconnect();
                return -1;
            }


        }

        //Delete an application from database
        public bool DeleteApplication(int id)
        {
            try
            {
                connect();
                setSqlComand("DELETE FROM applications WHERE Id=@id");

                int numRegistos = Delete(id);
                
                disconnect();

                return numRegistos == 1;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                return false;
            }
        }

        public override void readerIterator(SqlDataReader reader)
        {
            while (reader.Read())
            {
                Application application = new Application
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Creation_dt = (DateTime)reader["Creation_dt"],
                };
                this.applications.Add(application);
            }
        }
    }
}