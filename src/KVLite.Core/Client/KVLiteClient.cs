using System.Net.Sockets;
using System.Text;

namespace KVLite.Core.Client
{
    public class KVLiteClient
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
            /* The `GetStream()` method returns a `NetworkStream` object that provides a way to send and receive data over the network connection established by the `TcpClient`. */

            writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
            /* The `StreamWriter` class enables writing characters to a stream, in this case, the `NetworkStream`.
             * It takes the `stream` as the first argument and `Encoding.ASCII` as the second argument to specify the character encoding to be used.
             * The `{ AutoFlush = true }` part sets the `AutoFlush` property of the `StreamWriter` to `true`, which means that the buffer will be automatically flushed after each write operation. */

            reader = new StreamReader(stream, Encoding.ASCII);
            /* The `StreamReader` class enables reading characters from a stream, in this case, the `NetworkStream`.
             * It takes the `stream` as the first argument and `Encoding.ASCII` as the second argument to specify the character encoding to be used. */

        }

        /// <summary>
        /// Sets the value of a key in the key-value store.
        /// </summary>
        /// <param name="key">The key to set the value for.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="timeToLive">The time-to-live (TTL) for the key-value pair.</param>
        /// <returns>A string representing the response from the server.</returns>
        public string Set(string key, string value, string timeToLive)
        {
            string command = $"{{\"Operation\": \"SET\", \"key\": \"{key}\", \"value\": \"{value}\",\"ttl\": \"{timeToLive}\"}}";
            writer.WriteLine(command);

            string response = reader.ReadLine();
            Console.WriteLine(response);

            return response;
        }

        /// <summary>
        /// Retrieves the value of a key from the key-value store.
        /// </summary>
        /// <param name="key">The key to retrieve the value for.</param>
        /// <returns>A string representing the response from the server.</returns>
        public string Get(string key)
        {
            string command = $"{{\"Operation\": \"GET\", \"key\": \"{key}\"}}";
            writer.WriteLine(command);

            string response = reader.ReadLine();
            Console.WriteLine(response);

            return response;
        }

        /// <summary>
        /// Deletes a key from the key-value store.
        /// </summary>
        /// <param name="key">The key to delete.</param>
        /// <returns>A string representing the response from the server.</returns>
        public string Delete(string key)
        {
            string command = $"{{\"Operation\": \"DELETE\", \"key\": \"{key}\"}}";
            writer.WriteLine(command);

            string response = reader.ReadLine();
            Console.WriteLine(response);

            return response;
        }

        /// <summary>
        /// Updates the value of a key in the key-value store.
        /// </summary>
        /// <param name="key">The key to update.</param>
        /// <param name="value">The new value for the key.</param>
        /// <returns>A string representing the response from the server.</returns>
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