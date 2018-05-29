using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Actors.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace API.Controllers
{
    [Route("api/Telemetry")]
    public class TelemetryController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /region/version
        [HttpGet("{region}/{device}/{developedFault}")]
        public async Task<string> GetAsync(string region, string device, bool developedFault)
        {
            var actor = ActorProxy.Create<IThing>(new ActorId(device), new Uri("fabric:/TrabalhoPucMinas/ThingActorService"));
            await actor.SendTelemetryAsync(developedFault, device, region);
            return "Região: " + region + " - Dispositivo: " + device + " - Falha: " + developedFault;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}