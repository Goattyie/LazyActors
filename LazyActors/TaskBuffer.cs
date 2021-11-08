using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LazyActor
{
    public class TaskBuffer<T>
    {
        private readonly BufferBlock<T> _buffer;
        public TaskBuffer()
        {
            _buffer = new BufferBlock<T>();
        }
        public int TaskCount => _buffer.Count;
        public Task<TResponse> PostAndReply<TResponse>(Func<ReplyChannel<TResponse>, T> msgFabric)
        {
            var rc = new ReplyChannel<TResponse>();
            var msg = msgFabric(rc);

            Post(msg);

            return rc.GetReply;
        }
        public Task<T> ReceiveAsync() => _buffer.ReceiveAsync();
        public void Post(T message) => _buffer.Post(message);
        public void Clear() => _buffer.TryReceiveAll(out _);
    }
}
