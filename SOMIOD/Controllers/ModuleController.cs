using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;

namespace SOMIOD.Controllers
{
    public class ModuleController : DatabaseConnection
    {

        private List<Module> modules;

        public ModuleController() 
        { 
            this.modules = new List<Module>();
        }

        public List<Module> GetModules() 
        {
            List<Module> modules = new List<Module>();
            SetSqlComand("SELECT * FROM modules ORDER BY Id");

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

            return new List<Module>(this.modules);
        }

        public List<Module> GetModulesByApplication(int id)
        {
            List<Module> modules = new List<Module>();
            SetSqlComand("SELECT * FROM modules WHERE parent = @id ORDER BY Id");

            try
            {
                Connect();
                Select(id);
                Disconnect();
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
            }

            return new List<Module>(this.modules);
        }

        public Module GetModule(int id)
        {
            try
            {
                Connect();
                SetSqlComand("SELECT * FROM modules WHERE Id = @id");
                Select(id);
                Disconnect();
                return modules.Count() > 0 ? modules[0] : null;
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
            }
        }

        public int GetModuleByName(string name)
        {
            try
            {
                Connect();
                SetSqlComand("SELECT * FROM modules WHERE name = @name");
                SelectByName(name);
                Disconnect();

                return modules.Count() > 0 ? modules[0].Id : -1;
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
            }
        }

        public bool Store(Module module, int parent_id) 
        {
            try
            {
                Connect();

                string sql = "INSERT INTO modules VALUES (@name, @creation_dt, @parent)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", module.Name);
                cmd.Parameters.AddWithValue("@creation_dt", DateTime.Now);
                cmd.Parameters.AddWithValue("@parent", parent_id);

                SetSqlComand(sql);

                int n = InsertOrUpdate(cmd);
                
                Disconnect();

                return n == 1;
            }
            catch (Exception exception) { 
                if (conn.State == System.Data.ConnectionState.Open) Disconnect(); 
                throw exception; 
            }
        }

        public bool UpdateModule(Module module, int id)
        {
            try
            {
                Connect();
               
                string sql = "UPDATE modules SET name = @name, creation_dt = @creation_dt WHERE id = @id"; 
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", module.Name);
                cmd.Parameters.AddWithValue("@creation_dt", DateTime.Now);
                cmd.Parameters.AddWithValue("@id", id);

                SetSqlComand(sql);
                int n = InsertOrUpdate(cmd);
                Disconnect();

                return n == 1;
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
            }
        }

        public bool DeleteModule(int id)
        {
            try
            {
                Connect();
                DataController dc = new DataController();
                SubscriptionController sc = new SubscriptionController();

                List<Data> dl = new List<Data>();
                List<Subscription> sl = new List<Subscription>();
                dl = dc.GetDataByModule(id);
                sl = sc.GetSubscriptionByModule(id);
                if (dl != null)
                {
                    foreach (Data data in dl)
                    {
                        dc.DeleteData(data.Id);
                    }
                }
                if (sl != null)
                {
                    foreach (Subscription s in sl)
                    {
                        sc.DeleteSubcription(s.Id);
                    }
                }

                SetSqlComand("DELETE FROM modules WHERE id = @id");
                int n = Delete(id);
                Disconnect();
                return n == 1;
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
            }
        }

        public override void ReaderIterator(SqlDataReader reader)
        {
            modules = new List<Module>();
            while (reader.Read())
            {
                Module module = new Module
                {
                    Id = (int) reader["id"],
                    Name = (string) reader["name"],
                    Creation_dt = reader["Creation_dt"].ToString(),
                    Parent = (int) reader["parent"],
                };

                modules.Add(module);
            }
        }
    }
}