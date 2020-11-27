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



namespace Second_2
{
    public class Program
    {
        private static FileDetails fileDet;
        private static int localPort = 5002;
        private static UdpClient receivingUdpClient = new UdpClient(localPort);
        private static IPEndPoint RemoteIpEndPoint = null;
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
            GetFileDetails();
            ReceiveFile();
        }
        private static void GetFileDetails()
        {
            try
            {
                Console.WriteLine("---***Ожидание получениия деталей файла!! ****---");
                receiveByte = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                Console.WriteLine("--Детали файла получены!!");
                XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));
                MemoryStream stream1 = new MemoryStream();
                stream1.Write(receiveByte, 0, receiveByte.Length);
                stream1.Position = 0;
                fileDet = (FileDetails)fileSerializer.Deserialize(stream1);
                Console.WriteLine("Получен файл с расширением " + fileDet.FILETYPE + "размером " + fileDet.FILESIZE.ToString() + "байт");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static void ReceiveFile()
        {
            try
            {
                Console.WriteLine("---***Ожидание получениия файла!! ****---");
                receiveByte = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                Console.WriteLine("--Сохранение полученного файла!!");
                fs = new FileStream("temp." + fileDet.FILETYPE, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Write(receiveByte, 0, receiveByte.Length);
                Console.WriteLine("Файл сохранен");
                Console.WriteLine("---Открытие файла с помощью соответствующей программы----");
                Process.Start(fs.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                fs.Close();
            }
            using (FileStream fstream = new FileStream(@"D:\Study\Разработка РР\Примеры\3-яя лаба\Second\777.txt", FileMode.OpenOrCreate))
            {
                byte[] output = new byte[1024];
                string textFromFile = Encoding.Default.GetString(output);
                fstream.Seek(0, SeekOrigin.Begin);
                output = new byte[fstream.Length];
                fstream.Read(output, 0, output.Length);
                textFromFile = Encoding.Default.GetString(output);
                using (FileStream fstr = new FileStream(@"D:\Study\Разработка РР\Примеры\3-яя лаба\Second\888.html", FileMode.OpenOrCreate))
                {
                    foreach (string word in textFromFile.Split(' '))
                    {
                        String result = "";
                        result = "<h1><i>" + word + "</i></h1>";
                        byte[] input = Encoding.Default.GetBytes(result);
                        fstr.Write(input, 0, input.Length);
                    }
                }
            }
            Console.Read();
        }
    }
}