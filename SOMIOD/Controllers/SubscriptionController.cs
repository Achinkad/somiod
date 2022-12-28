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
        private readonly List<string> valid_events = new List<string>() { "creation", "deletion" };

        public SubscriptionController()
        {
            subscriptions = new List<Subscription>();
        }

        public bool Store(Subscription subscription, int parent_id)
        {
            // Check if Event String is "Creation" or "Deletion"
            if (!valid_events.Any(s => s.Contains(subscription.Event))) return false;

            try
            {
                Connect();
                string sql = "INSERT INTO subscriptions VALUES (@name, @creation_dt, @event, @endpoint, @parent)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                
                cmd.Parameters.AddWithValue("@name", subscription.Name);
                cmd.Parameters.AddWithValue("@creation_dt", DateTime.Now);
                cmd.Parameters.AddWithValue("@event", subscription.Event);
                cmd.Parameters.AddWithValue("@endpoint", subscription.Endpoint);
                cmd.Parameters.AddWithValue("@parent", parent_id);

                SetSqlComand(sql);
                int numRow = InsertOrUpdate(cmd);
                Disconnect();

                return numRow == 1;
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
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
                return numRow == 1;
            }
            catch (Exception exception)
            {
                if (conn.State == System.Data.ConnectionState.Open) Disconnect();
                throw exception;
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