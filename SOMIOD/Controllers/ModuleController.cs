using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI.MobileControls;
using System.Windows.Documents;

namespace SOMIOD.Controllers
{
    public class ModuleController : DatabaseConnection
    {

        private List<Module> modules;

        public ModuleController() { 
            this.modules = new List<Module>();
        }

        public List<Module> Select() {

            List<Module> modules = new List<Module>();
            setSqlComand("SELECT * FROM Module ORDER BY Id");
            connect();
            try
            {
                SelectAbstract();
                disconnect();

            }
            catch (Exception e) 
            {
                return null;
            }
            return new List<Module>(this.modules);
        }

        public override void readerIterator(SqlDataReader reader)
        {
            this.modules = new List<Module>();
            while (reader.Read())
            {
                Module module = new Module((int)reader["id"], (string)reader["name"], (DateTime)reader["cration_dt"], (int)reader["parent"]);

                this.modules.Add(module);
            }
        }
    }
}