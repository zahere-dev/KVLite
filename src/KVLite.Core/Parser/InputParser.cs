using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

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
