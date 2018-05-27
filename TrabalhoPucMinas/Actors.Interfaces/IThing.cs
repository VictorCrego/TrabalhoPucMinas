using Dominio;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Actors.Interfaces
{
    public interface IThing : IActor
    {
        Task ActivateMeAsync(string region, int version);
        Task SendTelemetryAsync(ThingTelemetry telemetry);
    }
}