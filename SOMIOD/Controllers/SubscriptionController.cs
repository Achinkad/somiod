using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SOMIOD.Controllers
{
    public class SubscriptionController : DatabaseConnection
    {
        private List<Subscription> subscriptions;

        public SubscriptionController()
        {
            this.subscriptions = new List<Subscription>();
        }
        public List<Subscription> GetModules()
        {

            List<Module> modules = new List<Module>();
            setSqlComand("SELECT * FROM Subscription ORDER BY Id");
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
            return new List<Subscription>(this.subscriptions);
        }

        public Subscription GetSubcription(int id)
        {
            try
            {
                connect();
                setSqlComand("SELECT * FROM Subscription  WHERE Id=@idProd");
                Select(id);
                disconnect();

                if (this.subscriptions[0] == null)
                {
                    return null;
                }
                return this.subscriptions[0];

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

        public bool Store(Subscription subscription)
        {
            try
            {
                connect();
                // id, string mname, DateTime creation_dt, int parent

                string sql = "INSERT INTO Prods VALUES (@Id,@Name,@Creation_date,@Parent,@Event,@Endpoint)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", subscription.Id);
                cmd.Parameters.AddWithValue("@Name", subscription.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", subscription.Creation_dt);
                cmd.Parameters.AddWithValue("@Parent", subscription.Parent);
                cmd.Parameters.AddWithValue("@Event", subscription.Event);
                cmd.Parameters.AddWithValue("@Endpoint", subscription.Endpoint);
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

        public int UpdateSubcriptio(Subscription subscription)
        {
            try
            {
                connect();
                string sql = "UPDATE Prods SET Id = @Id, Name = @Name, Creation_date = @Creation_date, Parent = @Parent, Event = @Event, Endpoint = @Endpoint  WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", subscription.Id);
                cmd.Parameters.AddWithValue("@Name", subscription.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", subscription.Creation_dt);
                cmd.Parameters.AddWithValue("@Parent", subscription.Parent);
                cmd.Parameters.AddWithValue("@Event", subscription.Event);
                cmd.Parameters.AddWithValue("@Endpoint", subscription.Endpoint);
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

        public int DeleteSubcriptio(int id)
        {
            try
            {
                connect();

                setSqlComand("DELETE FROM Subscription WHERE id = @id");
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

        public override void readerIterator(SqlDataReader reader)
        {
            this.subscriptions = new List<Subscription>();
            while (reader.Read())
            {
                Subscription subscription = null;

                this.subscriptions.Add(subscription);
            }
        }
    }
}