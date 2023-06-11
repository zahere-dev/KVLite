namespace KVLite.Core.Models
{
    public class StatusModel
    {
        public string Status { get; set; } = StatusConst.Success;
        public string Message { get; set; }
        public object Value { get; set; }
    }
}
