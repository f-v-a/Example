using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UDP_2
{
    class Chat
    {
        private static IPAddress remoteIPAd;
        private static int remotePort;
        private static int localPort;
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                /*Console.WriteLine("Введите значение локального порта");
                localPort = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine("Введите значение удаленного порта");
                remotePort = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine("Введите значение удаленного IP-адреса");
                remoteIPAd = IPAddress.Parse(Console.ReadLine());*/
                localPort = 5002;
                remotePort = 5001;
                remoteIPAd = IPAddress.Parse("127.0.0.1");
                Thread tRec = new Thread(new ThreadStart(Receiver));
                tRec.Start();
                while(true)
                {
                    Send(Console.ReadLine());
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private static void Send(string datagram)
        {
            UdpClient sender = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(remoteIPAd, remotePort);
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(datagram);
                sender.Send(bytes, bytes.Length, endPoint);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                sender.Close();
            }
        }
        public static void Receiver()
        {
            UdpClient receivUdpClient = new UdpClient(localPort);
            IPEndPoint remoteIPEndPoint = null;
            try
            {
                Console.WriteLine("Готово для чата!");
                while(true)
                {
                    byte[] receiveBytes = receivUdpClient.Receive(ref remoteIPEndPoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes);
                    Console.WriteLine("-" + returnData.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
