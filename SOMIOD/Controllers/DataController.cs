using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SOMIOD.Controllers
{
    public class DataController : DatabaseConnection
    {
        private List<Data> data_list;
        MqttClient mClient = new MqttClient("127.0.0.1");

        public DataController()
        {
            data_list = new List<Data>();
        }

        public void Publish(string content, int parent_id)
        {
            mClient.Connect(Guid.NewGuid().ToString());

            if (!mClient.IsConnected) throw new Exception("Error connecting to message broker...");

            ModuleController module = new ModuleController();
            Module parent_module = module.GetModule(parent_id);

            if (parent_module == null) throw new Exception("There is no module associated with this data.");

            try
            {
                mClient.Publish(parent_module.Name, Encoding.UTF8.GetBytes(content));
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<Data> GetData()
        {
            List<Data> data_list = new List<Data>();
            SetSqlComand("SELECT * FROM data");

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

            return new List<Data>(data_list);
        }

        public Data GetData(int id)
        {
            try
            {
                Connect();
                SetSqlComand("SELECT * FROM data WHERE Id = @id");
                Select(id);
                Disconnect();

                return data_list.Count() > 0 ? data_list[0] : null;
            }
            catch (Exception exception)
            { 
                if (conn.State == System.Data.ConnectionState.Open) Disconnect(); 
                throw exception; 
            }
        }

        public List<Data> GetDataByModule(int id)
        {
            SetSqlComand("SELECT * FROM data WHERE parent = @id ORDER BY Id");

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

            return new List<Data>(this.data_list);
        }

        public bool Store(Data value, int parent_id)
        {
            try
            {
                Connect();

                string sql = "INSERT INTO data VALUES(@content, @creation_dt, @parent)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@content", value.Content);
                cmd.Parameters.AddWithValue("@creation_dt", DateTime.Now);
                cmd.Parameters.AddWithValue("@parent", parent_id);

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

        public bool DeleteData(int id)
        {
            try
            {
                Connect();
                SetSqlComand("DELETE FROM Data WHERE Id = @id");
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
            data_list = new List<Data>();
            while (reader.Read())
            {
                Data data = new Data
                {
                    Id = (int) reader["Id"],
                    Content = (string) reader["Content"],
                    Parent = (int) reader["Parent"],
                    Creation_dt = reader["Creation_dt"].ToString(),
                };

                data_list.Add(data);
            }
        }
    }
}