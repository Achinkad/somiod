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

namespace MosquittoSubscriber
{
    public partial class Form1 : Form
    {
        MqttClient mClient = new MqttClient("127.0.0.1");
 
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


        private void a(object sender, EventArgs e)
        {
            mClient.Connect(Guid.NewGuid().ToString());
            if (!mClient.IsConnected)
            {
                MessageBox.Show("Error connecting to lightbulb...");
                return;
            }
            byte[] qosLevels = {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE};
            string[] mStrTopicsInfo = {"light_bulb"};

            mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;


            pictureBox1.Show();
            pictureBox2.Hide();


            mClient.Subscribe(mStrTopicsInfo, qosLevels);

        }

        

    }
}
