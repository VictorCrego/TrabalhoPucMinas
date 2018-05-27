using Actors.Interfaces;
using Dominio;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;

namespace Actors
{
    [StatePersistence(StatePersistence.Persisted)]
    public class Thing : Actor, IThing
    {
        private ThingState State = new ThingState();

        const string NomeTable = "victorpuc";
        const string keyTable = "D9cN80DLeOGyENshbh/PyocYoR9r0y8JRFi+VkqjzXMwXvYyVNB6HD01waENi4kxf4wkRqwkzHUvR5LkOljGpQ==";

        public Thing(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            State._telemetry = new List<ThingTelemetry>();
            State._deviceGroupId = ""; // not activated
            return base.OnActivateAsync();
        }

        public Task SendTelemetryAsync(ThingTelemetry telemetry)
        {
            State._telemetry.Add(telemetry); // saving data at the device level
            if (State._deviceGroupId != "")
            {
                var deviceGroup = ActorProxy.Create<IThingGroup>(new ActorId(State._deviceGroupId));
                return deviceGroup.SendTelemetryAsync(telemetry); // sending telemetry data for aggregation
            }
            return Task.FromResult(true);
        }

        public Task ActivateMeAsync(string region, int version)
        {
            try
            {
                StorageCredentials credencial = new StorageCredentials(NomeTable, keyTable);
                CloudStorageAccount conta = new CloudStorageAccount(credencial, true);
                CloudTableClient cliente = conta.CreateCloudTableClient();
                CloudTable Tabela = cliente.GetTableReference("Equipamentos");
                Tabela.CreateIfNotExistsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            /*State._deviceInfo = new ThingInfo()
            {
                DeviceId = Guid.NewGuid().ToString(),
                Region = region,
                Version = version
            };

            // based on the info, assign a group... for demonstration we are assigning a random group
            State._deviceGroupId = region;*/

            var deviceGroup = ActorProxy.Create<IThingGroup>(new ActorId(region));
            return deviceGroup.RegisterDevice(State._deviceInfo);
        }
    }
}