using KVLite.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KVLite.Core.Parser;
using Newtonsoft.Json;

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

            while (true)
            {
                var client = listener.AcceptTcpClient();
                HandleClient(client);
            }
            //while (true)   //we wait for a connection
            //{
            //    TcpClient client = listener.AcceptTcpClient();  //if a connection exists, the server will accept it

            //    NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages
            //    var reader = new StreamReader(ns, Encoding.UTF8);
            //    var writer = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };

            //    byte[] hello = new byte[100];   //any message must be serialized (converted to byte array)
            //    hello = Encoding.Default.GetBytes("hello world");  //conversion string => byte array

            //    ns.Write(hello, 0, hello.Length);     //sending the message
            //    string request;
            //    while ((request = reader.ReadLine()) != null)  //while the client is connected, we look for incoming messages
            //    {
            //        byte[] buffer = new byte[1024];
            //        int bytesRead = ns.Read(buffer, 0, buffer.Length);
            //        byte[] receivedData = new byte[bytesRead];
            //        Array.Copy(buffer, receivedData, bytesRead);

            //        var receivedString = Encoding.Default.GetString(receivedData);
            //        Console.WriteLine(receivedString);

            //        Object response = ProcessRequest(receivedString);
            //        byte[] encodedResponse = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(response));
            //        ns.Write(encodedResponse, 0, encodedResponse.Length);

            //    }
            //}
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream, Encoding.UTF8);
                var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                string request;
                //while (client.Connected)
                //{
                //    try
                //    {
                //        byte[] buffer = new byte[1024];
                //        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                //        byte[] receivedData = new byte[bytesRead];
                //        Array.Copy(buffer, receivedData, bytesRead);

                //        var receivedString = Encoding.Default.GetString(receivedData);
                //        Console.WriteLine(receivedString);

                //        Object response = ProcessRequest(receivedString);
                //        byte[] encodedResponse = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(response));
                //        stream.Write(encodedResponse, 0, encodedResponse.Length);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e);
                //    }
                //}

                while (client.Connected)
                {
                    try
                    {
                        if (!stream.DataAvailable)
                            break;
                        var receivedData = ReadCompleteMessage(stream);
                        var receivedString = Encoding.UTF8.GetString(receivedData);
                        Console.WriteLine(receivedString);

                        Object response = ProcessRequest(receivedString);
                        var encodedResponse = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                        stream.Write(encodedResponse, 0, encodedResponse.Length);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }


                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
            

        private byte[] ReadCompleteMessage(NetworkStream stream)
        {
            var buffer = new byte[1024];
            var messageBuilder = new StringBuilder();

            while (true)
            {
                var bytesRead = stream.Read(buffer, 0, buffer.Length);
                messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                if (stream.DataAvailable)
                    continue;

                break;
            }

            return Encoding.UTF8.GetBytes(messageBuilder.ToString());
        }



        private Object ProcessRequest(string request)
        {
            var parser = new InputParser();
            var command = parser.Parse(request);

            if(command.Operation == "SET")
            {
                this.keyValueStore.Set(command.Key, command.Value.ToString());
                return $" Key: {command.Key} is SET";
            }
            else if (command.Operation == "GET")
            {
                return this.keyValueStore.Get(command.Key);
            }
            else if(command.Operation == "DELETE")
            {
                return this.keyValueStore.Delete(command.Key);
            }
            else if (command.Operation == "UPDATE")
            {
                this.keyValueStore.Update(command.Key, command.Value.ToString());
                return $" Key: {command.Key} is Updated";
            }

            return "Error";
        }
    }

}
