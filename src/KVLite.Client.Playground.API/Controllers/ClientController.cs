using KVLite.Core.Client;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace KVLite.Client.Playground.API.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {        

        [HttpGet]
        [Route("set")]
        public IActionResult Set([FromQuery] string key, string value, string ttl)
        {
            var client = new KVLiteClient();
            var res = client.Set(key, value, ttl);
            client.Dispose();
            return Ok(res);
        }

        [HttpGet]
        [Route("get")]
        public IActionResult Get([FromQuery] string key)
        {
            var _client = new KVLiteClient();
            var res = _client.Get(key);
            _client.Dispose();
            return Ok(res);
        }

        [HttpGet]
        [Route("delete")]
        public IActionResult Delete([FromQuery] string key)
        {
            var _client = new KVLiteClient();
            var res = _client.Delete(key);
            _client.Dispose();
            return Ok(res);
        }

        [HttpGet]
        [Route("update")]
        public IActionResult Update([FromQuery] string key, string value)
        {
            var _client = new KVLiteClient();
            var res = _client.Update(key, value);
            _client.Dispose();
            return Ok(res);
        }

        [HttpGet]
        [Route("bulk-set")]
        public IActionResult BulkSet()
        {
            var _client = new KVLiteClient();
            _client.BulkSet();
            _client.Dispose();
            return Ok("Done");

        }


        //public void Dispose()
        //{
        //    _client.Dispose();
        //}
    }
}
