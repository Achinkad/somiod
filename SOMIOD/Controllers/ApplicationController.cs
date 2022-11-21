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
        //Select Aplications
        public IEnumerable<Application> SelectApplications()
        {
            List<Application> applications = new List<Application>();
            string sql = "SELECT * FROM Application";
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Application application = new Application
                    {
                        id = (int)reader["Id"],
                        name = (string)reader["Name"],
                        creation_dt = (DateTime)reader["Creation_dt"],
                    };
                }
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close()
            }
            return applications;
        }

        //Store a new Apllication and their values in the database

        public int Store(Application value)
        {
            string sql = "INSERT INTO Application VALUES(@name,@creation_dt)";
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", value.Name);
                cmd.Parameters.AddWithValue("@category", value.Creation_dt);
                int numRegistos = cmd.ExecuteNonQuery();
                conn.Close();
                if (numRegistos > 0)
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

        //Add a reference to an Application in the Module database table
        public int AddModule(int id_app, int id_module)
        {
            string sql = "INSERT INTO Module VALUES (@parent) WHERE id=@IdApp";
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idApp", id_app);
                cmd.Parameters.AddWithValue("@parent", id_module);
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
        //Remove a reference to an Application in the Module database table

        public int RemoveModule(int id_app, int id_module)
        {
            string sql = "DELETE FROM Modules VALUES (@parent) WHERE id=@IdApp";
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idApp", id_app);
                cmd.Parameters.AddWithValue("@parent", id_module);
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
        //Delete a application from database
        public int Delete(int id)
        {
            string sql = "DELETE FROM Application WHERE Id=@IdApp";
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
    }
}