namespace KVLite.Core.Storage
{
    public class KeyValueStore
    {
        private readonly Dictionary<string, string> keyValuePairs;

        public KeyValueStore()
        {
            keyValuePairs = new Dictionary<string, string>();
        }


        public void Set(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            keyValuePairs[key] = value;
        }


        public string? Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return keyValuePairs.TryGetValue(key, out var value) ? value : null;
        }

    }
}
    