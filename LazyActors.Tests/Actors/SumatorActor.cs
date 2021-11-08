using LazyActor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyActors.Tests.Actors
{
    record SumatorMessage(ReplyChannel<int> rc, int[] args);
    class SumatorActor : AbstractActor<SumatorMessage>
    {
        public Task<int> Sum(params int[] args) => PostWithReply<int>(rc => new SumatorMessage(rc, args));

        protected override Task<HandleResult> HandleError(Exception ex, ActorMessage<SumatorMessage> message)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleMessage(SumatorMessage message)
        {
            var sum = message.args.Sum();
            message.rc.SetReply(sum);
            return Task.CompletedTask;
        }
    }
}
