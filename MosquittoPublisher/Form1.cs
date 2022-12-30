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
        List<string> app_names = new List<string>();
        List<string> app_ids = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        async private void button1_Click(object sender, EventArgs e)
        { 
            if(comboBox1.SelectedItem != null)
            {
                if (comboBox2.SelectedItem != null)
                {
                    // -> POST DO DATA ON
                    var values_data = new Dictionary<string, string> {
                        { "res_type", "data" },
                        { "content", "on" }                 
                    };

                    var content_data = new StringContent(
                        JsonConvert.SerializeObject(values_data),
                        System.Text.Encoding.UTF8,
                        "application/json"
                    );

                    var response_data = await client.PostAsync("http://localhost:53818/api/somiod/"+ comboBox1.SelectedItem.ToString().ToLower()+"/"+ comboBox2.SelectedItem.ToString().ToLower(), content_data);
                    MessageBox.Show("The information was sended successfully.", "Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please, insert a module.", "Input Validations", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please, insert an application.", "Input Validations", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       async private void comboBox1_Click(object sender, EventArgs e)
       {
            // -> GET PARA O DROPDOWN DOS APPS
            var response_GET = await client.GetAsync("http://localhost:53818/api/somiod/applications");
            var responseString_GET = await response_GET.Content.ReadAsStringAsync();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseString_GET);

            XmlNodeList nodes_name = xmlDoc.SelectNodes("//*[local-name() = 'Name']");
            XmlNodeList nodes_id = xmlDoc.SelectNodes("//*[local-name() = 'Id']");

            app_names.Clear();
            foreach (XmlNode node in nodes_name)
            {
                app_names.Add(node.InnerText);
            }

            app_ids.Clear();
            foreach (XmlNode node in nodes_id)
            {
                app_ids.Add(node.InnerText);
            }

            comboBox1.Items.Clear();
            foreach (String name in app_names)
            {
                comboBox1.Items.Add(name);
            }
       }

       async private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
       {
            //GET PARA O DROPDOWN DOS MODULES
            var response_GET = await client.GetAsync("http://localhost:53818/api/somiod/applications/"+ app_ids[comboBox1.SelectedIndex] + "/modules");
            var responseString_GET = await response_GET.Content.ReadAsStringAsync();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseString_GET);

            XmlNodeList nodes_name = xmlDoc.SelectNodes("//*[local-name() = 'Name']");

            List<string> module_names = new List<string>();

            module_names.Clear();

            foreach (XmlNode node in nodes_name)
            {
                module_names.Add(node.InnerText);
            }

            comboBox2.Items.Clear();
            foreach (String name in module_names)
            {
                comboBox2.Items.Add(name);
            }
       }

       async private void button2_Click(object sender, EventArgs e)
       {
            if (comboBox1.SelectedItem != null)
            {
                if (comboBox2.SelectedItem != null)
                {
                    // -> POST DO DATA OFF
                    var values_data = new Dictionary<string, string> {
                        { "res_type", "data" },
                        { "content", "off" }
                    };

                    var content_data = new StringContent(
                        JsonConvert.SerializeObject(values_data),
                        System.Text.Encoding.UTF8,
                        "application/json"
                    );

                    await client.PostAsync("http://localhost:53818/api/somiod/" + comboBox1.SelectedItem.ToString().ToLower() + "/" + comboBox2.SelectedItem.ToString().ToLower(), content_data);
                    MessageBox.Show("The information was sended successfully.", "Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
