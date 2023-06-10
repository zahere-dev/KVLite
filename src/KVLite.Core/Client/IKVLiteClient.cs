namespace KVLite.Core.Client
{
    public interface IKVLiteClient
    {
        string BulkSet();
        string Delete(string key);
        void Dispose();
        string Get(string key);
        string Set(string key, string value);
        string Update(string key, string value);
    }
}