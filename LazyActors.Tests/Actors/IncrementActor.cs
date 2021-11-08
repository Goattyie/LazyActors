using LazyActor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyActors.Tests.Actors
{
    class IncrementActor : AbstractActor<object>
    {
        private int _state;
        public int GetState() => _state;
        public void Add() => Post(null);
        public Task WaitEndTasks() => WaitWindowEmpty();
        protected override Task<HandleResult> HandleError(Exception ex, ActorMessage<object> message)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleMessage(object message)
        {
            _state++;
            return Task.CompletedTask;
        }
    }
}
