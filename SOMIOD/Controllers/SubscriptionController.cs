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
        private List<string> valid_events = new List<string>() { "creation", "deletion" };

        public SubscriptionController()
        {
            subscriptions = new List<Subscription>();
        }

        public List<Subscription> GetModules()
        {
            List<Module> modules = new List<Module>();

            SetSqlComand("SELECT * FROM Subscription ORDER BY Id");
            
            try
            {
                Connect();
                Select();
                Disconnect();
            }
            catch (Exception)
            {
                return null;
            }

            return new List<Subscription>(subscriptions);
        }

        public Subscription GetSubcription(int id)
        {
            try
            {
                Connect();
                SetSqlComand("SELECT * FROM Subscription WHERE Id=@idProd");
                Select(id);
                Disconnect();
                return subscriptions?[0] ?? null;

            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                return null;
            }
        }

        public bool Store(Subscription subscription, int parent_id)
        {
            // Check if Event String is "Creation" or "Deletion"
            if (!valid_events.Any(s => s.Contains(subscription.Event))) return false;

            try
            {
                Connect();

                string sql = "INSERT INTO subscriptions VALUES (@Name, @Creation_dt, @Event, @Endpoint, @Parent)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                
                cmd.Parameters.AddWithValue("@Name", subscription.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", DateTime.Now);
                cmd.Parameters.AddWithValue("@Parent", parent_id);
                cmd.Parameters.AddWithValue("@Event", subscription.Event);
                cmd.Parameters.AddWithValue("@Endpoint", subscription.Endpoint);
                
                SetSqlComand(sql);

                int numRow = InsertOrUpdate(cmd);
                
                Disconnect();

                return numRow == 1;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                return false;
            }
        }

        public bool UpdateSubcription(Subscription subscription)
        {
            // Check if Event String is "Creation" or "Deletion"
            if (!valid_events.Any(s => s.Contains(subscription.Event))) return false;

            try
            {
                Connect();
                
                string sql = "UPDATE subscriptions SET Name = @Name, Creation_date = @Creation_date, Parent = @Parent, Event = @Event, Endpoint = @Endpoint  WHERE id = @id";
                
                SqlCommand cmd = new SqlCommand(sql, conn);
                
                cmd.Parameters.AddWithValue("@id", subscription.Id);
                cmd.Parameters.AddWithValue("@Name", subscription.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", subscription.Creation_dt);
                cmd.Parameters.AddWithValue("@Parent", subscription.Parent);
                cmd.Parameters.AddWithValue("@Event", subscription.Event);
                cmd.Parameters.AddWithValue("@Endpoint", subscription.Endpoint);
                
                SetSqlComand(sql);

                int numRow = InsertOrUpdate(cmd);

                Disconnect();

                return numRow == 1 ? true : false;

            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                return false;
            }
        }

        public bool DeleteSubcription(int id)
        {
            try
            {
                Connect();
                SetSqlComand("DELETE FROM Subscription WHERE id = @id");
                int numRow = Delete(id);
                Disconnect();
                return numRow == 1 ? true : false;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                return false;
            }
        }

        public override void ReaderIterator(SqlDataReader reader)
        {
            subscriptions = new List<Subscription>();
            while (reader.Read())
            {
                Subscription subscription = null;
                subscriptions.Add(subscription);
            }
        }
    }
}