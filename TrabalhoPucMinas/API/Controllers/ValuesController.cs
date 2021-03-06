﻿using System;
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
    [Route("api/ActivateMe")]
    public class ValuesController : Controller
    {
        // GET /values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /region/device/version
        [HttpGet("{region}/{device}/{version}")]
        public async Task<string> GetAsync(string region, string device, int version)
        {
                var actor = ActorProxy.Create<IThing>(new ActorId(device), new Uri("fabric:/TrabalhoPucMinas/ThingActorService"));
                await actor.ActivateMeAsync(region, device, version);
                string versionString = Convert.ToString(version);
                return "Região: " + region + " - Versão: " + versionString;
        }

        // POST /values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT /values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE /values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
