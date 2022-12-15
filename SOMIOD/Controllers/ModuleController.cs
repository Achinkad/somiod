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

        public bool Store(Module module) {
            try
            {
                connect();
                // id, string mname, DateTime creation_dt, int parent

                string sql = "INSERT INTO modules VALUES (@Id,@Name,@Creation_date,@Parent)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", module.Id);
                cmd.Parameters.AddWithValue("@Name", module.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Parent", module.Parent);
                setSqlComand(sql);

                int numRow = InsertOrUpdate(cmd);
                disconnect();
                if (numRow == 1)
                {
                    return true;
                }
                return false;

            }
            catch (Exception)
            {
                //fechar ligação à DB
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    disconnect();
                }
                return false;
            }
        }

        public int UpdateModule(Module module)
        {
            try
            {
                connect();
                string sql = "UPDATE modules SET Id = @Id, Name = @Name, Creation_date = @Creation_date, Parent = @Parent WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", module.Id);
                cmd.Parameters.AddWithValue("@Name", module.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", module.Creation_dt);
                cmd.Parameters.AddWithValue("@Parent", module.Parent);
                setSqlComand(sql);

                int numRow = InsertOrUpdate(cmd);

                disconnect();
                if (numRow == 1)
                {
                    return numRow;
                }
                return -1;

            }
            catch (Exception)
            {
                //fechar ligação à DB
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    disconnect();
                }

                return -1;
            }
        }

        public int DeleteModule(int id) {
            try
            {
                connect();

                setSqlComand("DELETE FROM modules WHERE id = @id");
                int numRow = Delete(id);
                disconnect();
                if (numRow == 1)
                {
                    return 1;
                }
                return -1;

            }
            catch (Exception)
            {
                //fechar ligação à DB
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    disconnect();
                }
                return -1;
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