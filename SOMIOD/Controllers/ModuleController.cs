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

        public ModuleController() { 
            this.modules = new List<Module>();
        }

        public List<Module> GetModules() {

            List<Module> modules = new List<Module>();
            setSqlComand("SELECT * FROM modules ORDER BY Id");
            try
            {
                connect();
                Select();
                disconnect();

            }
            catch (Exception e) 
            {
                return null;
            }

            //TODO: É necessário converter os objetos para JSON
            return new List<Module>(this.modules);
            //return JsonSerializer.Serialize(new List<Module>(this.modules));
        }

        public Module GetModule(int id)
        {
            try
            {
                connect();
                setSqlComand("SELECT * FROM modules WHERE Id=@id");
                Select(id);
                disconnect();

                if (this.modules[0] == null)
                {
                    return null;
                }
                return this.modules[0];

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

        public int GetModuleByName(string name)
        {
            try
            {
                connect();
                setSqlComand("SELECT * FROM modules WHERE name = @name");
                SelectByName(name);
                disconnect();

                return modules.Count() > 0 ? modules[0].Id : -1;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                return -1;
            }
        }

        public bool Store(Module module, int parent_id) {
            try
            {
                connect();

                string sql = "INSERT INTO modules VALUES (@Name, @Creation_dt, @Parent)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Name", module.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", DateTime.Now);
                cmd.Parameters.AddWithValue("@Parent", parent_id);

                setSqlComand(sql);

                int numRow = InsertOrUpdate(cmd);

                disconnect();

                return numRow == 1;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                return false;
            }
        }

        public bool UpdateModule(Module module, int id)
        {
            try
            {
                connect();
                string sql = "UPDATE modules SET name = @Name, creation_dt = @Creation_dt WHERE id = @id";
                
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Name", module.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", DateTime.Now);

                setSqlComand(sql);

                int numRow = InsertOrUpdate(cmd);

                disconnect();

                return numRow == 1;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                return false;
            }
        }

        public bool DeleteModule(int id) {
            try
            {
                connect();
                setSqlComand("DELETE FROM modules WHERE id = @id");
                int numRow = Delete(id);
                disconnect();

                return numRow == 1;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                return false;
            }
        }

        public override void readerIterator(SqlDataReader reader) {
            this.modules = new List<Module>();
            while (reader.Read())
            {
                Module module = new Module
                {
                    Id=(int)reader["id"],
                    Name=(string)reader["name"],
                    Creation_dt = new DateTime(),//TODO: verificar data
                    Parent = (int)reader["parent"],
                };

                this.modules.Add(module);
            }
        }
    }
}