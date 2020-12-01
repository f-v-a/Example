using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        NetworkStream writerStream;
        Socket s;
        public Form1()
        {
            InitializeComponent();
            TcpClient eclient = new TcpClient("localhost", 34567);
            writerStream = eclient.GetStream();
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter("D:\\Учеба\\Нижегородова\\2-ая лаба(готова)\\Client\\client\\bin\\Debug\\SMS.txt");
                sw.WriteLine("Сотрудник : " + textBox1.Text);
                sw.WriteLine("Премия : " + textBox2.Text);
                sw.WriteLine("Оклад : " + textBox3.Text);
                sw.WriteLine("Часы: " + textBox4.Text);
                sw.Close();
                BinaryFormatter format = new BinaryFormatter();
                byte[] buf = new byte[1024];
                int count;
                string puti;
                puti = "D:\\Учеба\\Нижегородова\\2-ая лаба(готова)\\Client\\client\\bin\\Debug\\SMS.txt";
                FileStream fs = new FileStream(puti, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                long k = fs.Length;
                format.Serialize(writerStream, k.ToString());
                while ((count = br.Read(buf, 0, 1024)) > 0)
                {
                    format.Serialize(writerStream, buf);
                }
            }
            catch (Exception f)
            {
                Console.WriteLine("Exception: " + f.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Byte[] data = new byte[256];
            Int32 bytes = writerStream.Read(data, 0, data.Length);
            string response = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
            richTextBox1.Text = response;
        }
    }
}
