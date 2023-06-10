using KVLite.Core.Models;

namespace KVLite.Core.Storage
{
    public class KeyValueStore
    {
        private readonly Dictionary<string, object> keyValuePairs;
        private readonly Dictionary<string, DateTime> expirationTimes;
        private readonly TimeSpan defaultTtl = TimeSpan.MaxValue;

        public KeyValueStore()
        {
            keyValuePairs = new Dictionary<string, Object>();
            expirationTimes = new Dictionary<string, DateTime>();

        }

        /// <summary>
        /// Sets a value in the key-value store with an optional time-to-live (TTL).
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="ttl">The optional time-to-live (TTL) in seconds.</param>
        /// <returns>A StatusModel indicating the status of the set operation.</returns>
        public StatusModel Set(string key, object value, double ttl = -1)
        {
            var status = new StatusModel();

            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    status.Status = StatusConst.Error;
                    status.Message = "Key is null or empty"; 
                    return status;
                }

                if (Exists(key))
                {
                    status.Status = StatusConst.Error;
                    status.Message = "Key already exists";
                    return status;
                }

                var dateTime = (ttl == -1) ? DateTime.MaxValue : DateTime.UtcNow.Add(TimeSpan.FromSeconds(ttl));

                keyValuePairs[key] = value;
                expirationTimes[key] = dateTime;
            }
            catch (Exception ex)
            {
                status.Status = StatusConst.Error;
                status.Message = ex.Message;
            }

            status.Message = "Successfully Stored ";
            return status;
        }



        /// <summary>
        /// Retrieves the value associated with the specified key from the key-value store.
        /// </summary>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>A StatusModel indicating the status of the get operation and the retrieved value.</returns>
        public StatusModel Get(string key)
        {
            var status = new StatusModel();

            if (string.IsNullOrEmpty(key))
            {
                status.Status = StatusConst.Error;
                status.Message = "Key is null or empty";
                return status;
            }

            if (!Exists(key))
            {
                status.Status = StatusConst.Error;
                status.Message = "Key does not exist";
                return status;
            }

            if (expirationTimes[key] >= DateTime.UtcNow)
            {
                status.Status = StatusConst.Success;
                status.Message = StatusConst.Success;
                status.Value = keyValuePairs[key];
                return status;
            }

            // Key has expired, remove it
            RemoveExpiredKey(key);

            status.Status = StatusConst.Error;
            status.Message = "Key does not exist";
            return status;
        }


        /// <summary>
        /// Checks if the specified key exists in the key-value store.
        /// </summary>
        /// <param name="key">The key to check for existence.</param>
        /// <returns>True if the key exists, false otherwise.</returns>
        public bool Exists(string key)
        {
            return keyValuePairs.ContainsKey(key) && expirationTimes.ContainsKey(key);
        }


        /// <summary>
        /// Deletes the value associated with the specified key from the key-value store.
        /// </summary>
        /// <param name="key">The key of the value to delete.</param>
        /// <returns>A StatusModel indicating the status of the delete operation.</returns>
        public StatusModel Delete(string key)
        {
            var status = new StatusModel();

            if (string.IsNullOrEmpty(key))
            {
                status.Status = StatusConst.Error;
                status.Message = "Key is null or empty";
                return status;
            }

            if (!Exists(key))
            {
                status.Status = StatusConst.Error;
                status.Message = "Key does not exist";
                return status;
            }

            RemoveExpiredKey(key);

            if (keyValuePairs.Remove(key))
            {
                expirationTimes.Remove(key);
            }

            status.Status = StatusConst.Success;
            status.Message = "Key removed successfully";
            return status;
        }


        /// <summary>
        /// Updates the value associated with the specified key in the key-value store.
        /// </summary>
        /// <param name="key">The key of the value to update.</param>
        /// <param name="value">The new value to set for the key.</param>
        /// <returns>A StatusModel indicating the status of the update operation.</returns>
        public StatusModel Update(string key, object value)
        {
            var status = new StatusModel();

            if (string.IsNullOrEmpty(key))
            {
                status.Status = StatusConst.Error;
                status.Message = "Key is null or empty";
                return status;
            }

            if (!Exists(key))
            {
                status.Status = StatusConst.Error;
                status.Message = "Key does not exist";
                return status;
            }

            if (expirationTimes[key] < DateTime.UtcNow)
            {
                RemoveExpiredKey(key);
                status.Status = StatusConst.Error;
                status.Message = "Key does not exist";
                return status;
            }

            keyValuePairs[key] = value;
            status.Message = "Updated Successfully";
            return status;
        }



        private void RemoveExpiredKey(string key)
        {
            if (expirationTimes[key] < DateTime.UtcNow)
            {
                keyValuePairs.Remove(key);
                expirationTimes.Remove(key);
            }
        }

    }
}
    