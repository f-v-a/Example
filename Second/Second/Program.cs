using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Second
{
    public class FileSender
    {
        private static FileDetails fileDet = new FileDetails();
        private static IPAddress remoteIPAdress;
        private const int remotePort = 5002;
        private static UdpClient sender = new UdpClient();
        private static IPEndPoint endPoint;
        private static FileStream fs;

        private static byte[] receiveByte = new byte[0];
        [Serializable]
        public class FileDetails
        {
            public string FILETYPE = "";
            public long FILESIZE = 0;
        }
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter Remote IP address");
                remoteIPAdress = IPAddress.Parse(Console.ReadLine().ToString());
                endPoint = new IPEndPoint(remoteIPAdress, remotePort);
                Console.WriteLine("Введите путь и имя файла для отправки");
                fs = new FileStream(@Console.ReadLine().ToString(), FileMode.Open, FileAccess.Read);
                if (fs.Length > 15000)
                {
                    Console.Write("Эта версия передает файлы размером < 15мб");
                    Console.ReadKey();
                    sender.Close();
                    fs.Close();
                    return;
                }
                SendFileInfo();
                Thread.Sleep(1000);
                SendFile();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static void SendFileInfo()
        {
            fileDet.FILETYPE = fs.Name.Substring((int)fs.Name.Length - 3, 3);
            fileDet.FILESIZE = fs.Length;
            XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));
            MemoryStream stream = new MemoryStream();
            fileSerializer.Serialize(stream, fileDet);
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
            Console.WriteLine("Отправка деталей файла...");
            sender.Send(bytes, bytes.Length, endPoint);
            stream.Close();
        }
        private static void SendFile()
        {
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            Console.WriteLine("Отправка файла размером = " + fs.Length + "байт");
            try
            {
                sender.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            { 
                fs.Close();
                sender.Close();
            }
            try
            {
                Thread.Sleep(2000);
                Process.Start(@"D:\Study\Разработка РР\Примеры\3-яя лаба\Second\888.html");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.Read();
            Console.WriteLine("Файл успешно отправлен");
        }
    }
}
