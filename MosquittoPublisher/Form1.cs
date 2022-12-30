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
using System.Reflection;
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
        
        private static readonly HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            
        }

       async private void Form1_Load(object sender, EventArgs e)
        {


            //POST DO MODULE LIGHT_COMMAND

            var values_module = new Dictionary<string, string>
              {
                  { "res_type", "module" },
                  { "name", "light_command" }
              };

            var content_module = new StringContent(
                                                  JsonConvert.SerializeObject(values_module),
                                                   System.Text.Encoding.UTF8,
                                                   "application/json"
                                                   );

            var response_module = await client.PostAsync("http://localhost:53818/api/somiod/lightning", content_module);

            var responseString_module = await response_module.Content.ReadAsStringAsync();

            MessageBox.Show(responseString_module);


            ///////////////////////////////////////////
        }

       async private void button1_Click(object sender, EventArgs e)
        {
            /*
            if(comboBox1.SelectedItem!=null)
            {
                

            }
            else
            {
                MessageBox.Show("First Choose a Application!");
            }
            */

            //POST DO DATA ON
            var values_data = new Dictionary<string, string>
              {
                  { "res_type", "data" },
                  { "content", "on" }
              };

            var content_data = new StringContent(
                                                  JsonConvert.SerializeObject(values_data),
                                                   System.Text.Encoding.UTF8,
                                                   "application/json"
                                                   );

            var response_data = await client.PostAsync("http://localhost:53818/api/somiod/lightning/light_bulb", content_data);

            var responseString_data = await response_data.Content.ReadAsStringAsync();

            //MessageBox.Show(responseString_module);

        }

       async private void button2_Click(object sender, EventArgs e)
        {
            /*
            if (comboBox1.SelectedItem != null)
            {
                if (textBox1.Text!=null)
                {

                }
                else
                {
                    MessageBox.Show("Write the name of the Module!");
                }
                
                
            }
            else
            {
                MessageBox.Show("First Choose a Application!");
            }
            */

            //POST DO DATA OFF
            var values_data = new Dictionary<string, string>
              {
                  { "res_type", "data" },
                  { "content", "off" }
              };

            var content_data = new StringContent(
                                                  JsonConvert.SerializeObject(values_data),
                                                   System.Text.Encoding.UTF8,
                                                   "application/json"
                                                   );

            var response_data = await client.PostAsync("http://localhost:53818/api/somiod/lightning/light_bulb", content_data);

            var responseString_data = await response_data.Content.ReadAsStringAsync();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

       async private void comboBox1_Click(object sender, EventArgs e)
        {
            //GET PARA O DROPDOWN DOS APPS

            var response_GET = await client.GetAsync("http://localhost:53818/api/somiod/applications");
            var responseString_GET = await response_GET.Content.ReadAsStringAsync();

            List<string> app_names = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseString_GET);

            XmlNodeList nodes = xmlDoc.SelectNodes("//*[local-name() = 'Name']");


            foreach (XmlNode node in nodes)
            {
                app_names.Add(node.InnerText);


            }
            comboBox1.Items.Clear();
            foreach (String name in app_names)
            {
                comboBox1.Items.Add(name);
            }
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
