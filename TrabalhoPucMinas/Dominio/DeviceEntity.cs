using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class DeviceEntity : TableEntity
    {
        public DeviceEntity(string id, string region)
        {
            
            PartitionKey = id;
            RowKey = region;
        }

        public DeviceEntity() { }

        public string version { get; set; }
    }
}
