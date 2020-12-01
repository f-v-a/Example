using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace server2
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener clientListener = new TcpListener(34567);
            clientListener.Start();
            TcpClient client = clientListener.AcceptTcpClient();
            NetworkStream readerStream = client.GetStream();
            BinaryFormatter outformat = new BinaryFormatter();
            Console.WriteLine("Введите путь");
            Console.WriteLine("Иначе он будет задан автоматически");
            string puti = Console.ReadLine();
            FileStream fs;
            if (puti == "")
            {
                puti = "111.txt";
                fs = new FileStream("111.txt", FileMode.OpenOrCreate);
            }
            else
            {
                fs = new FileStream(puti, FileMode.OpenOrCreate);
            }
            BinaryWriter bw = new BinaryWriter(fs);
            int count;
            count = int.Parse(outformat.Deserialize(readerStream).ToString());
            int i = 0;
            for (; i < count; i += 1024)
            {
                byte[] buf = (byte[])(outformat.Deserialize(readerStream));
                bw.Write(buf);
            }
            bw.Close();
            fs.Close();
            Console.WriteLine("Файл получен");
            /* try
             {
                 using (var sr = new StreamReader("111.txt"))
                 {
                     Console.WriteLine(sr.ReadToEnd());

                 }
                 byte[] info = Encoding.ASCII.GetBytes(sr.ToString());
                 sListener.Send(info);
             }
             catch (IOException e)
             {
                 Console.WriteLine(e.Message);
             }*/
            Encoding code = Encoding.UTF8;
            string filE = File.ReadAllText("111.txt", code); // Метод считывания с файла
            Console.WriteLine(filE);
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(filE);
            readerStream.Write(data, 0, data.Length);
            Console.ReadLine();

        }
    }
}
