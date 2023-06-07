using System.Net.Sockets;
using System.Text;

namespace KVLite.Client
{
    public class Client
    {
        private const int Port = 6377;
        private const string Host = "localhost";

        public void Set(string key, string value)
        {
            try
            {
                using (var client = new TcpClient(Host, Port))
                using (var stream = client.GetStream())
                using (var writer = new StreamWriter(stream, Encoding.ASCII))
                using (var reader = new StreamReader(stream, Encoding.ASCII))
                {
                    string command = $"*3\r\n$3\r\nSET\r\n${key.Length}\r\n{key}\r\n${value.Length}\r\n{value}\r\n";
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
