using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyActor
{
    public class ActorMessage<T>
    {
        public T Message { get; set; }
        public int Attemp { get; set; }
        public DateTime EnterTime { get; set; }
        public Exception LastError { get; set; }
        public ActorMessage(T message)
        {
            EnterTime = DateTime.Now;
            Message = message;
            Attemp = 0;
        }
        public void OnError(Exception ex)
        {
            Attemp++;
            LastError = ex;
        }
    }
}
