using KVLite.Core.Models;
using KVLite.Core.Parser;
using KVLite.Core.Storage;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace KVLite.Core
{
    /// <summary>
    /// Represents a server that listens for incoming connections and handles client requests.
    /// </summary>
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

        /// <summary>
        /// Starts the server and listens for incoming connections.
        /// </summary>
        public void Start()
        {
            listener.Start();
            Console.WriteLine($"Server started. Listening on port {Port}...");

            while (true)
            {
                var client = listener.AcceptTcpClient();
                HandleClient(client);
            }
            // The server will keep running indefinitely until manually stopped.
            // Make sure to handle any necessary cleanup or termination logic.
        }

        /// <summary>
        /// Handles the communication with a connected client.
        /// </summary>
        /// <param name="client">The TcpClient representing the connected client.</param>
        private void HandleClient(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream, Encoding.UTF8);
                var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

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

                // Close the client connection
                client.Close();
            }
            catch (Exception e)
            {
                
                Console.WriteLine(e);
            }
        }


        /// <summary>
        /// Reads a complete message from the network stream.
        /// </summary>
        /// <param name="stream">The NetworkStream to read from.</param>
        /// <returns>A byte array containing the complete message.</returns>
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

                // All data has been read, exit the loop
                break;
            }

            return Encoding.UTF8.GetBytes(messageBuilder.ToString());
        }



        /// <summary>
        /// Processes the incoming request and performs the corresponding operation.
        /// </summary>
        /// <param name="request">The request string to process.</param>
        /// <returns>An object representing the result of the operation.</returns>
        private StatusModel ProcessRequest(string request)
        {
            var parser = new InputParser();
            var command = parser.Parse(request);

            if (command.Operation == "SET")
            {
                double ttl = -1;
                if(!double.TryParse(command.Ttl, out ttl)) ttl= -1;
                return this.keyValueStore.Set(command.Key, command.Value.ToString(), ttl);
            }
            else if (command.Operation == "GET")
            {
                return this.keyValueStore.Get(command.Key);
            }
            else if (command.Operation == "DELETE")
            {
                return this.keyValueStore.Delete(command.Key);
            }
            else if (command.Operation == "UPDATE")
            {
                return this.keyValueStore.Update(command.Key, command.Value.ToString());
            }

            return new StatusModel { Status = StatusConst.Error, Message = "Error"};

        }

    }

}
