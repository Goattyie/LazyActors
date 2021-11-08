using LazyActor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyActors.Tests.Actors
{
    class ConsoleLoggerActor : AbstractActor<string>
    {
        public void Log(string message) => Post(message);
        protected override Task<HandleResult> HandleError(Exception ex, ActorMessage<string> message)
        {
            return (message.Attemp > 10) ? HandleResult.OkTask() : HandleResult.RetryTaskWithDilay(TimeSpan.FromSeconds(1));
        }

        protected override Task HandleMessage(string message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}
