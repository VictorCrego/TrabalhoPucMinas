using Actors.Interfaces;
using Dominio;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Actors
{
    [StatePersistence(StatePersistence.Persisted)]
    public class Thing : Actor, IThing
    {
        private readonly String storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=victorpuc;AccountKey=D9cN80DLeOGyENshbh/PyocYoR9r0y8JRFi+VkqjzXMwXvYyVNB6HD01waENi4kxf4wkRqwkzHUvR5LkOljGpQ==;EndpointSuffix=core.windows.net";
        private ThingState State = new ThingState();

        public Thing(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            State._telemetry = new List<ThingTelemetry>();
            State._deviceGroupId = ""; // not activated
            return base.OnActivateAsync();
        }

        public Task SendTelemetryAsync(bool developedFault, string device, string region)
        {
            ThingTelemetry telemetry = new ThingTelemetry() { DevelopedFault = developedFault, DeviceId = device, Region = region };
            State._telemetry.Add(telemetry); // saving data at the device level
            if (State._deviceGroupId != "")
            {
                var deviceGroup = ActorProxy.Create<IThingGroup>(new ActorId(State._deviceGroupId));
                return deviceGroup.SendTelemetryAsync(telemetry); // sending telemetry data for aggregation
            }
            return Task.FromResult(true);
        }

        public Task ActivateMeAsync(string region, string device, int version)
        {
            CloudTable cloudTable;
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            cloudTable = cloudTableClient.GetTableReference("Equipamentos");
            cloudTable.CreateIfNotExistsAsync();

            var Device = new DeviceEntity(device, region)
            {
                Version = version.ToString()
            };
            TableOperation insertOperation = TableOperation.InsertOrReplace(Device);
            cloudTable.ExecuteAsync(insertOperation);

            State._deviceInfo = new ThingInfo()
            {
                DeviceId = device,
                Region = region,
                Version = version
            };

            // based on the info, assign a group... for demonstration we are assigning a random group
            State._deviceGroupId = region;

            var deviceGroup = ActorProxy.Create<IThingGroup>(new ActorId(region));
            return deviceGroup.RegisterDevice(State._deviceInfo);
        }
    }
}