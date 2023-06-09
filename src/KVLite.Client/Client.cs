using System.Net.Sockets;
using System.Text;

namespace KVLite.Client
{
    public class Client
    {
        private const int Port = 6377;
        private const string Host = "localhost";

        public static void Set(string key, string value)
        {
            try
            {
                using (var client = new TcpClient(Host, Port))
                using (var stream = client.GetStream())
                using (var writer = new StreamWriter(stream, Encoding.ASCII))
                using (var reader = new StreamReader(stream, Encoding.ASCII))
                {
                    string obj = $"{{\"Operation\": \"SET\", \"key\": \"{key}\", \"value\": \"{value}\"}}";

                    string command = $"{{\"Operation\": \"SET\", \"key\": \"{key}\", \"value\": {obj}}}";
                    writer.Write(command);
                    writer.Flush();

                    string response = reader.ReadLine();
                    Console.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
