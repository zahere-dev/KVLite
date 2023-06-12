using KVLite.Core.Client;
using Microsoft.AspNetCore.Mvc;

namespace KVLite.Client.Playground.API._2.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller, IDisposable
    {
        private readonly KVLiteClient _client;
        public ClientController()
        {
            _client = new KVLiteClient();
        }

        [HttpGet]
        [Route("set")]
        public IActionResult Set([FromQuery] string key, string value, string ttl)
        {
            
            var res = _client.Set(key, value, ttl);
            
            return Ok(res);
        }

        [HttpGet]
        [Route("get")]
        public IActionResult Get([FromQuery] string key)
        {
            
            var res = _client.Get(key);
            
            return Ok(res);
        }

        [HttpGet]
        [Route("delete")]
        public IActionResult Delete([FromQuery] string key)
        {
            
            var res = _client.Delete(key);
            
            return Ok(res);
        }

        [HttpGet]
        [Route("update")]
        public IActionResult Update([FromQuery] string key, string value)
        {
            
            var res = _client.Update(key, value);
            
            return Ok(res);
        }
        
    }
}
