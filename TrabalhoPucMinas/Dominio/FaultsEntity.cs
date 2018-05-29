using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class FaultsEntity : TableEntity
    {
        public FaultsEntity(string id, string device)
        {
            PartitionKey = id;
            RowKey = device;
        }

        public int Version { get; set; }
    }
}
