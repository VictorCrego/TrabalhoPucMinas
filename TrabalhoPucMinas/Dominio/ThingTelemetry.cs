using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    [Serializable]
    public class ThingTelemetry
    {
        public bool DevelopedFault { get; set; }
        public string Region { get; set; }
        public string DeviceId { get; set; }
    }
}
