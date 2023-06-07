using KVLite.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KVLite.Core
{
    public class Server
    {
        private const int Port = 6377;

        private readonly TcpListener listener;
        private readonly KeyValueStore keyValueStore;

        public Server(KeyValueStore keyValueStore)
        {
            this.keyValueStore = keyValueStore;
            listener = new TcpListener(IPAddress.Any, Port);
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine($"Server started. Listening on port {Port}...");

            //while (true)
            //{
            //    var client = listener.AcceptTcpClient();
            //    HandleClient(client);
            //}
            while (true)   //we wait for a connection
            {
                TcpClient client = listener.AcceptTcpClient();  //if a connection exists, the server will accept it

                NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages
                var writer = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };

                byte[] hello = new byte[100];   //any message must be serialized (converted to byte array)
                hello = Encoding.Default.GetBytes("hello world");  //conversion string => byte array

                ns.Write(hello, 0, hello.Length);     //sending the message

                while (client.Connected)  //while the client is connected, we look for incoming messages
                {
                    byte[] msg = new byte[1024];     //the messages arrive as byte array
                    ns.Read(msg, 0, msg.Length);   //the same networkstream reads the message sent by the client
                    Console.WriteLine(Encoding.Default.GetString(msg)); //now , we write the message as string
                    string response = ProcessRequest(Encoding.Default.GetString(msg));
                    writer.WriteLine(response);

                }
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream, Encoding.UTF8);
                var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                string request;
                while ((request = reader.ReadLine()) != null)
                {
                    string response = ProcessRequest(request);
                    writer.Write(response);
                }

                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private string ProcessRequest(string request)
        {
            // Parse and process the request using your command handling logic
            // For example, you can invoke the appropriate methods in your KeyValueStore
            // based on the received request.

            // Sample logic to handle GET command
            if (request.StartsWith("GET"))
            {
                string key = request.Split(' ')[1];
                string value = keyValueStore.Get(key);
                return value ?? "(nil)";
            }

            // Sample logic to handle SET command
            if (request.StartsWith("SET"))
            {
                string[] parts = request.Split(' ');
                string key = parts[1];
                string value = parts[2];
                keyValueStore.Set(key, value);
                return "OK";
            }

            // Handle other commands as needed

            // Return an error message for unsupported commands
            return "Ok";
        }
    }

}
