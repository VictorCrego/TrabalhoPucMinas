using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using Actor1.Interfaces;

namespace Actor1
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class Actor1 : Actor, IActor1
    {
        public Actor1(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
            return this.StateManager.TryAddStateAsync("count", 0);
        }

        Task<int> IActor1.GetCountAsync(CancellationToken cancellationToken)
        {
            return this.StateManager.GetStateAsync<int>("count", cancellationToken);
        }

        Task IActor1.SetCountAsync(int count, CancellationToken cancellationToken)
        {
            return this.StateManager.AddOrUpdateStateAsync("count", count, (key, value) => count > value ? count : value, cancellationToken);
        }
    }
}
