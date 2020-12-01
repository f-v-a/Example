using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        byte[] bytes = new byte[1024];
        Socket s;
        int k, i = 0;
        string data;
        public Form1()
        {
            InitializeComponent();
            try
            {
                IPHostEntry ipHost = Dns.Resolve("127.0.0.1");
                IPAddress ipAdr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAdr, 11000);
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(ipEndPoint);
            }
            catch(Exception e)
            {
                MessageBox.Show("Исключение", e.ToString());
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

                byte[] msg2 = Encoding.ASCII.GetBytes(textBox2.Text);
                s.Send(msg2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] R = new byte[1024];
            s.Receive(R);
            textBox5.Text = Encoding.ASCII.GetString(R);
        }

        private void button1_Click(object sender, EventArgs e)
        {

                byte[] msg1 = Encoding.ASCII.GetBytes(textBox1.Text);
                s.Send(msg1);
        }
    }
}
