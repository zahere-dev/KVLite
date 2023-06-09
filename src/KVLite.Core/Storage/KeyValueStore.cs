using Newtonsoft.Json.Linq;

namespace KVLite.Core.Storage
{
    public class KeyValueStore
    {
        private readonly Dictionary<string, Object> keyValuePairs;

        public KeyValueStore()
        {
            keyValuePairs = new Dictionary<string, Object>();
        }


        public void Set(string key, Object value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if(Exists(key))
                throw new Exception($"Key > {key} exists");

            keyValuePairs[key] = value;
        }


        public Object? Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return keyValuePairs.TryGetValue(key, out var value) ? value : null;
        }

        public bool Exists(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return keyValuePairs.ContainsKey(key);
        }

        public bool Delete(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return keyValuePairs.Remove(key);
        }

        public void Update(string key, Object value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (!Exists(key))
                throw new Exception($"Ket > {key} doesn't exist");

            keyValuePairs[key] = value;
        }

    }
}
    