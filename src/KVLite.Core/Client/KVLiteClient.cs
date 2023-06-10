using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;

namespace KVLite.Core.Client
{
    public class KVLiteClient : IKVLiteClient
    {
        private const int Port = 6377;
        private const string Host = "localhost";
        private TcpClient client;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;        

        public KVLiteClient()
        {
            Console.WriteLine($"KVLite Client initiated for {Host}:{Port}");
            client = new TcpClient(Host, Port);
            stream = client.GetStream();
            writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
            reader = new StreamReader(stream, Encoding.ASCII);
        }

        public string Set(string key, string value)
        {
            string command = $"{{\"Operation\": \"SET\", \"key\": \"{key}\", \"value\": \"{value}\"}}";
            writer.WriteLine(command);

            string response = reader.ReadLine();
            Console.WriteLine(response);

            return response;
        }

        public string BulkSet()
        {
            for (int i = 1; i < 5; i++)
            {
                string command = $"{{\"Operation\": \"SET\", \"key\": \"{i}\", \"value\": \"value {i}\"}}";
                writer.WriteLine(command);
                Console.WriteLine(reader.ReadLine());
            }

            return "Done";
        }

        public string Get(string key)
        {
            string command = $"{{\"Operation\": \"GET\", \"key\": \"{key}\"}}";
            writer.WriteLine(command);

            string response = reader.ReadLine();
            Console.WriteLine(response);

            return response;
        }

        public string Delete(string key)
        {
            string command = $"{{\"Operation\": \"DELETE\", \"key\": \"{key}\"}}";
            writer.WriteLine(command);

            string response = reader.ReadLine();
            Console.WriteLine(response);

            return response;
        }

        public string Update(string key, string value)
        {
            string command = $"{{\"Operation\": \"UPDATE\", \"key\": \"{key}\", \"value\": \"{value}\"}}";
            writer.WriteLine(command);

            string response = reader.ReadLine();
            Console.WriteLine(response);

            return response;
        }

        public void Dispose()
        {
            writer.Dispose();
            reader.Dispose();
            stream.Dispose();
            client.Close();
        }
    }
}