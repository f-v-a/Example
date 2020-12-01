using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    
    class Program
    {
        
        static void Main(string[] args)
        {
            double sum = 0;
            IPHostEntry ipHost = Dns.Resolve("127.0.0.1");
            IPAddress ipAdr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAdr, 11000);
            Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                while(true)
                {
                    Console.WriteLine("Ожидание соеднения с портом {0}", ipEndPoint);
                    Socket handler = sListener.Accept();
                    string data, data2, data3, data4 = null;
                    while(true)
                    {
                        byte[] name = new byte[1024];
                        int nameRec = handler.Receive(name);
                        data = Encoding.ASCII.GetString(name, 0, nameRec);
                        Console.WriteLine("Сотрудник : {0}", data);
                        byte[] premiya = new byte[1024];
                        int premiyaRec = handler.Receive(premiya);
                        data2 = Encoding.ASCII.GetString(premiya, 0, premiyaRec);
                        Console.WriteLine("Премия : {0}", data2);
                        byte[] oklad = new byte[1024];
                        int okladRec = handler.Receive(oklad);
                        data3 = Encoding.ASCII.GetString(oklad, 0, okladRec);
                        Console.WriteLine("Оклад : {0}", data3);
                        byte[] clock = new byte[1024];
                        int clockRec = handler.Receive(clock);
                        data4 = Encoding.ASCII.GetString(clock, 0, clockRec);
                        Console.WriteLine("Часы : {0}", data4);
                        if(data != null & data2 != null & data3 != null & data4 != null)
                        {
                            Console.WriteLine("Производим расчет заработной платы, учитывая отработанные часы");
                            double spend = Convert.ToDouble(data4);
                            double oklad2 = Convert.ToDouble(data3);
                            double premiya2 = Convert.ToDouble(data2);
                            if (spend >= 100)
                            {
                                sum = oklad2 + premiya2;
                                Console.WriteLine("Заработная плата сотрудника : {0} = {1}", data, sum);
                                byte[] price = Encoding.ASCII.GetBytes(sum.ToString());
                                handler.Send(price);
                            }
                            else
                            {
                                Console.WriteLine("Заработная плата сотрудника : {0} = {1}", data, oklad2);
                                byte[] price = Encoding.ASCII.GetBytes(sum.ToString());
                                handler.Send(price);
                            }
                        }
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch(Exception e)
            {
                Console.Write(e.ToString());
            }
           // String zarp = Convert.ToString(sum);
          //  byte[] msg = Encoding.ASCII.GetBytes(zarp);
          //  sListener.Send(msg);
        }
    }
}
