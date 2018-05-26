using System;
using System.Collections.Generic;

namespace Dominio
{
    [Serializable]
    public class ThingGroupState
    {
        public List<ThingInfo> _devices;
        public Dictionary<string, int> _faultsPerRegion;
        public List<ThingInfo> _faultyDevices;
    }
}
