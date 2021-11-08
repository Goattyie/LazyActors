using LazyActor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyActors.Tests.Actors
{
    class ErrorActor : AbstractActor<int>
    {
        private int _errors = 0;
        public int GetErrors => _errors;
        public void Add(int value) => Post(value);
        public Task WaitEndTasks() => WaitWindowEmpty();
        protected override Task<HandleResult> HandleError(Exception ex, ActorMessage<int> message)
        {
            _errors++;
            Console.WriteLine($"Ошибка {_errors}: {message.Message}");
            return message.Attemp > 1 ? HandleResult.OkTask() : HandleResult.RetryTaskWithDilay(TimeSpan.FromSeconds(2));
        }

        protected override Task HandleMessage(int message)
        {
            if (message % 10 == 0)
                throw new Exception();
            return Task.CompletedTask;
        }
    }
}
