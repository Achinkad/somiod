using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net.Http;
using Newtonsoft.Json;

namespace MosquittoSubscriber
{
    public partial class Form1 : Form
    {
        MqttClient mClient = new MqttClient("127.0.0.1");
        private static readonly HttpClient client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }
        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            String status = "";
            MessageBox.Show("Received = " + Encoding.UTF8.GetString(e.Message));
            status = Encoding.UTF8.GetString(e.Message);

            

            
            if (status == "on")
            {
                pictureBox1.BeginInvoke((MethodInvoker)delegate { pictureBox1.Hide(); });
                pictureBox2.BeginInvoke((MethodInvoker)delegate { pictureBox2.Show(); });
            }
            if (status == "off")
            {
                pictureBox1.BeginInvoke((MethodInvoker)delegate { pictureBox1.Show(); });
                pictureBox2.BeginInvoke((MethodInvoker)delegate { pictureBox2.Hide(); });
            }
            
        }


        async private void a(object sender, EventArgs e)
        {
            

            mClient.Connect(Guid.NewGuid().ToString());
            if (!mClient.IsConnected)
            {
                MessageBox.Show("Error connecting to lightbulb...");
                return;
            }

            //HTTP POSTs AQUI

            //POST DO APPLICATION
            var values_application = new Dictionary<string, string>
              {
                  { "res_type", "application" },
                  { "name", "Lightning" }
              };


            var content_application = new StringContent(
                                                     JsonConvert.SerializeObject(values_application),
                                                      System.Text.Encoding.UTF8,
                                                      "application/json"
                                                      );

            var response_application = await client.PostAsync("http://localhost:53818/api/somiod", content_application);

            var responseString_application = await response_application.Content.ReadAsStringAsync();

            //MessageBox.Show(responseString_application);

            //POST DO MODULE

            var values_module = new Dictionary<string, string>
              {
                  { "res_type", "module" },
                  { "name", "light_bulb" }
              };

            var content_module = new StringContent(
                                                  JsonConvert.SerializeObject(values_module),
                                                   System.Text.Encoding.UTF8,
                                                   "application/json"
                                                   );

            var response_module = await client.PostAsync("http://localhost:53818/api/somiod/"+ values_application["name"].ToLower(), content_module);

            var responseString_module = await response_module.Content.ReadAsStringAsync();

            //MessageBox.Show(responseString_module);

            //POST DO SUBSCRIPTION

            var values_subscription = new Dictionary<string, string>
              {
                  { "res_type", "subscription" },
                  { "name", "sub1" },
                  { "event", "creation" },
                  { "endpoint", "127.0.0.1" }
              };

            var content_subscription = new StringContent(
                                                  JsonConvert.SerializeObject(values_subscription),
                                                   System.Text.Encoding.UTF8,
                                                   "application/json"
                                                   );

            var response_subscription = await client.PostAsync("http://localhost:53818/api/somiod/" + values_application["name"].ToLower()+"/"+ values_module["name"].ToLower(), content_subscription);

            var responseString_subscription = await response_subscription.Content.ReadAsStringAsync();

            //MessageBox.Show(responseString_subscription);

            /////////////////////////////////////////////////////////////////////////7

            byte[] qosLevels = {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE};
            string[] mStrTopicsInfo = {"light_bulb"};

            mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;


            pictureBox1.Show();
            pictureBox2.Hide();


            mClient.Subscribe(mStrTopicsInfo, qosLevels);

        }

        

    }
}
