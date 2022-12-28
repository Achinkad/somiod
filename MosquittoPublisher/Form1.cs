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
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
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

            List<string> app_names = new List<string>();
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseString_GET);

            XmlNodeList nodes = xmlDoc.SelectNodes("//*[local-name() = 'Name']");
            
            
            foreach (XmlNode node in nodes)
            {
                app_names.Add(node.InnerText);
                MessageBox.Show(node.InnerText);
               
            }
            
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
