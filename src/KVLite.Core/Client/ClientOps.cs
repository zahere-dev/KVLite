using System.Net.Sockets;
using System.Text;

namespace KVLite.Core.Client
{
    public class ClientOps
    {

        private const int Port = 6377;
        private const string Host = "localhost";

        public string Set(string key, string value)
        {
            try
            {
                using (var client = new TcpClient(Host, Port))
                using (var stream = client.GetStream())
                using (var writer = new StreamWriter(stream, Encoding.ASCII))
                using (var reader = new StreamReader(stream, Encoding.ASCII))
                {
                    string command = $"{{\"Operation\": \"SET\", \"key\": \"{key}\", \"value\": \"{value}\"}}";
                    writer.Write(command);
                    writer.Flush();

                    string response = reader.ReadLine();
                    Console.WriteLine(response);

                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                return $"Error: {ex.Message}";
            }
        }

        public string BulkSet()
        {
            try
            {
                using (var client = new TcpClient(Host, Port))
                using (var stream = client.GetStream())
                using (var writer = new StreamWriter(stream, Encoding.ASCII))
                using (var reader = new StreamReader(stream, Encoding.ASCII))
                {
                    for (int i = 0; i < 50; i++)
                    {
                        string command = $"{{\"Operation\": \"SET\", \"key\": \"{i}\", \"value\": \"value {i}\"}}";
                        writer.Write(command);
                        writer.Flush();

                        string response = reader.ReadLine();
                        Console.WriteLine(response);
                    }

                    return "Done";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                return $"Error: {ex.Message}";
            }
        }

        public string Get(string key)
        {
            try
            {
                using (var client = new TcpClient(Host, Port))
                using (var stream = client.GetStream())
                using (var writer = new StreamWriter(stream, Encoding.ASCII))
                using (var reader = new StreamReader(stream, Encoding.ASCII))
                {
                    string command = $"{{\"Operation\": \"GET\", \"key\": \"{key}\"}}";
                    writer.Write(command);
                    writer.Flush();

                    string response = reader.ReadLine();
                    Console.WriteLine(response);

                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public string Delete(string key)
        {
            try
            {
                using (var client = new TcpClient(Host, Port))
                using (var stream = client.GetStream())
                using (var writer = new StreamWriter(stream, Encoding.ASCII))
                using (var reader = new StreamReader(stream, Encoding.ASCII))
                {
                    string command = $"{{\"Operation\": \"DELETE\", \"key\": \"{key}\"}}";
                    writer.Write(command);
                    writer.Flush();

                    string response = reader.ReadLine();
                    Console.WriteLine(response);

                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public string Update(string key, string value)
        {
            try
            {
                using (var client = new TcpClient(Host, Port))
                using (var stream = client.GetStream())
                using (var writer = new StreamWriter(stream, Encoding.ASCII))
                using (var reader = new StreamReader(stream, Encoding.ASCII))
                {
                    string command = $"{{\"Operation\": \"UPDATE\", \"key\": \"{key}\", \"value\": \"{value}\"}}";
                    writer.Write(command);
                    writer.Flush();

                    string response = reader.ReadLine();
                    Console.WriteLine(response);

                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }
    }
}
