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
        public IActionResult Set([FromQuery] string key, string value)
        {
            var client = new ClientOps();
            var res = client.Set(key, value);
            return Ok(res);
        }

        [HttpGet]
        [Route("get")]
        public IActionResult Get([FromQuery] string key)
        {
            var client = new ClientOps();
            var res = client.Get(key);
            return Ok(res);
        }

        [HttpGet]
        [Route("delete")]
        public IActionResult Delete([FromQuery] string key)
        {
            var client = new ClientOps();
            var res = client.Delete(key);
            return Ok(res);
        }

        [HttpGet]
        [Route("update")]
        public IActionResult Update([FromQuery] string key, string value)
        {
            var client = new ClientOps();
            var res = client.Update(key, value);
            return Ok(res);
        }

        [HttpGet]
        [Route("bulk-set")]
        public IActionResult BulkSet()
        {
            var client = new ClientOps();
            client.BulkSet();
            return Ok("Done");
        }
    }
}
