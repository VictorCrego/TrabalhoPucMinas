using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    [Serializable]
    public class ThingInfo
    {
        public string DeviceId { get; set; }
        public string Region { get; set; }
        public int Version { get; set; }
    }
}
