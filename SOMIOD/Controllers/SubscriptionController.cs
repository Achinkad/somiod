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

            setSqlComand("SELECT * FROM Subscription ORDER BY Id");
            
            try
            {
                connect();
                Select();
                disconnect();
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
                connect();
                setSqlComand("SELECT * FROM Subscription WHERE Id=@idProd");
                Select(id);
                disconnect();
                return subscriptions?[0] ?? null;

            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                return null;
            }
        }

        public bool Store(Subscription subscription, int parent_id)
        {
            // Check if Event String is "Creation" or "Deletion"
            if (valid_events.Any(s => s.Contains(subscription.Event))) return false;

            try
            {
                connect();
                
                string sql = "INSERT INTO subscriptions VALUES (@Name, @Creation_dt, @Parent, @Event, @Endpoint)";
               
                SqlCommand cmd = new SqlCommand(sql, conn);
                
                cmd.Parameters.AddWithValue("@Name", subscription.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", DateTime.Now);
                cmd.Parameters.AddWithValue("@Parent", parent_id);
                cmd.Parameters.AddWithValue("@Event", subscription.Event);
                cmd.Parameters.AddWithValue("@Endpoint", subscription.Endpoint);
                
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

        public bool UpdateSubcription(Subscription subscription)
        {
            // Check if Module Parent exists
            ModuleController module = new ModuleController();
            if (module.GetModule(subscription.Parent) == null) return false;

            // Check if Event String is "Creation" or "Deletion"
            if (valid_events.Any(s => s.Contains(subscription.Event))) return false;

            try
            {
                connect();
                
                string sql = "UPDATE subscriptions SET Name = @Name, Creation_date = @Creation_date, Parent = @Parent, Event = @Event, Endpoint = @Endpoint  WHERE id = @id";
                
                SqlCommand cmd = new SqlCommand(sql, conn);
                
                cmd.Parameters.AddWithValue("@id", subscription.Id);
                cmd.Parameters.AddWithValue("@Name", subscription.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", subscription.Creation_dt);
                cmd.Parameters.AddWithValue("@Parent", subscription.Parent);
                cmd.Parameters.AddWithValue("@Event", subscription.Event);
                cmd.Parameters.AddWithValue("@Endpoint", subscription.Endpoint);
                
                setSqlComand(sql);

                int numRow = InsertOrUpdate(cmd);

                disconnect();

                return numRow == 1 ? true : false;

            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                return false;
            }
        }

        public bool DeleteSubcription(int id)
        {
            try
            {
                connect();
                setSqlComand("DELETE FROM Subscription WHERE id = @id");
                int numRow = Delete(id);
                disconnect();
                return numRow == 1 ? true : false;
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) disconnect();
                return false;
            }
        }

        public override void readerIterator(SqlDataReader reader)
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