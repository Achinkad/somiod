using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;



namespace MosquittoPublisher
{


    public partial class Form1 : Form
    {
        MqttClient mClient = new MqttClient("127.0.0.1");
        private static readonly HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            
        }
       async private void Form1_Load(object sender, EventArgs e)
        {
            mClient.Connect(Guid.NewGuid().ToString());
            if (!mClient.IsConnected)
            {
                MessageBox.Show("Error connecting to lightbulb...");
                return;
            }

            //GET PARA O DROPDOWN

            var response_GET = await client.GetAsync("http://localhost:53818/api/somiod/applications");
            var responseString_GET = await response_GET.Content.ReadAsStringAsync();
            MessageBox.Show(responseString_GET);

            ///////////////////////////////////////////
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mClient.IsConnected)
            {
              
                MessageBox.Show("aaa");
                mClient.Publish("light_bulb", Encoding.UTF8.GetBytes("on"));
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mClient.IsConnected)
            {

                MessageBox.Show("bbb");
                mClient.Publish("light_bulb", Encoding.UTF8.GetBytes("off"));
            }
        }
    }
}
