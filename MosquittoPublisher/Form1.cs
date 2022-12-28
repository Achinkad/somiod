using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
        public Form1()
        {
            InitializeComponent();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            mClient.Connect(Guid.NewGuid().ToString());
            if (!mClient.IsConnected)
            {
                MessageBox.Show("Error connecting to lightbulb...");
                return;
            }
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
