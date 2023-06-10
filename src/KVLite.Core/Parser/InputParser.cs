using Newtonsoft.Json;

namespace KVLite.Core.Parser
{
    public class InputParser
    {
        public enum OperationType
        {
            GET,
            SET,
            DELETE,
            UPDATE
        }

        public class Command
        {
            public string Operation { get; set; }
            public string Key { get; set; }
            public Object Value { get; set; }
            public string Ttl { get; set; }
        }

        /// <summary>
        /// Parses the input string into a Command object.
        /// </summary>
        /// <param name="input">The input string to parse.</param>
        /// <returns>A Command object representing the parsed input.</returns>
        public Command Parse(string input)
        {
            var command = new Command();

            try
            {
                command = JsonConvert.DeserializeObject<Command>(input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return command;
        }

    }
}
