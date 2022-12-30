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
                MessageBox.Show("Error connecting to lightbulb...", "Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
    
            mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            pictureBox1.Show();
            pictureBox2.Hide();
        }

       async private void button1_Click(object sender, EventArgs e)
       {
            if (textBoxApp.Text!= string.Empty)
            {
                if (textBoxModule.Text!= string.Empty)
                {
                    if (textBoxSub.Text!= string.Empty)
                    {
                        // -> POST DO APPLICATION                        
                        var values_app = new Dictionary<string, string>
                        {
                            { "res_type", "application" },
                            { "name", textBoxApp.Text }
                        };

                        var content_app = new StringContent(
                            JsonConvert.SerializeObject(values_app),
                            System.Text.Encoding.UTF8,
                            "application/json"
                        );

                        var response_app = await client.PostAsync("http://localhost:53818/api/somiod/", content_app);
                        

                        // -> SUBSCRIBE CHANNEL
                        byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
                        string[] mStrTopicsInfo = { textBoxModule.Text };

                        mClient.Subscribe(mStrTopicsInfo, qosLevels);


                        // -> POST DO MODULE
                        var values_module = new Dictionary<string, string>
                        {
                            { "res_type", "module" },
                            { "name", textBoxModule.Text }
                        };

                        var content_module = new StringContent(
                            JsonConvert.SerializeObject(values_module),
                            System.Text.Encoding.UTF8,
                            "application/json"
                        );

                        var response_module = await client.PostAsync("http://localhost:53818/api/somiod/" + textBoxApp.Text.ToLower(), content_module);

                       
                        // -> POST DO SUBSCRIPTION
                        var values_subscription = new Dictionary<string, string>
                        {
                            { "res_type", "subscription" },
                            { "name", textBoxSub.Text },
                            { "event", "creation" },
                            { "endpoint", "127.0.0.1" }
                        };

                        var content_subscription = new StringContent(
                            JsonConvert.SerializeObject(values_subscription),
                            System.Text.Encoding.UTF8,
                            "application/json"
                        );

                        var response_subscription = await client.PostAsync("http://localhost:53818/api/somiod/" + textBoxApp.Text.ToLower() + "/" + textBoxModule.Text.ToLower(), content_subscription);

                        MessageBox.Show("The connection was made successfully.", "Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Please insert a subscription.", "Input Validations", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please insert a module.", "Input Validations", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please insert an application.", "Input Validations", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
