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
            setSqlComand("SELECT * FROM Module ORDER BY Id");
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
            return new List<Module>(this.modules);
        }

        public override void readerIterator(SqlDataReader reader) {
            this.modules = new List<Module>();
            while (reader.Read())
            {
                Module module = new Module((int)reader["id"], (string)reader["name"], (DateTime)reader["cration_dt"], (int)reader["parent"]);

                this.modules.Add(module);
            }
        }
    }
}