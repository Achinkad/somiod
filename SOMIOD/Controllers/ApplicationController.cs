﻿using System;
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
        //Select Aplications
        public List<Application> GetApplications()
        {
            List<Application> applications = new List<Application>();
            setSqlComand("SELECT * FROM Application");
            
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
            return new List<Application>(this.applications);
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
                cmd.Parameters.AddWithValue("@creation_dt", value.Creation_dt);

                int numRegistos = InsertOrUpdate(cmd);
                /*
                int numRegistos = cmd.ExecuteNonQuery();
                
                conn.Close();
                */
                disconnect();
                if (numRegistos > 0)
                {
                    return 1;
                }
                return -1;
            
                }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    disconnect();
                return -1;
            }
        }

        //Add a reference to an Application in the Module database table
        public int AddModule(int id_app, int id_module)
        {
            string sql = "INSERT INTO Module VALUES (@parent) WHERE id=@IdApp";
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
            string sql = "DELETE FROM Modules VALUES (@parent) WHERE id=@IdApp";
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
        //Delete a application from database
        public int Delete(int id)
        {
            string sql = "DELETE FROM Application WHERE Id=@id";
            //SqlConnection conn = null;
            try
            {
                /*
                conn = new SqlConnection(connectionString);
                conn.Open();
               */
                connect();
                /*
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idApp", id);
                int numRegistos = cmd.ExecuteNonQuery();
                */
                int numRegistos = Delete(id);
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