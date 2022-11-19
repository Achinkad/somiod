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
        public List<Subscription> Select()
        {

            List<Module> modules = new List<Module>();
            setSqlComand("SELECT * FROM Subscription ORDER BY Id");
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
            return new List<Subscription>(this.subscriptions);
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