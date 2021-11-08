using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyActor
{
    public class ReplyChannel<TResponse>
    {
        private readonly TaskCompletionSource<TResponse> _tcs;
        public ReplyChannel()
        {
            _tcs = new TaskCompletionSource<TResponse>();
        }
        public void SetReply(TResponse response) => _tcs.SetResult(response);
        public Task<TResponse> GetReply => _tcs.Task;
    }
}
