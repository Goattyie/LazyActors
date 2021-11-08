using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyActor
{
    public abstract class AbstractActor<T>
    {
        private TaskBuffer<ActorMessage<T>> _taskBuffer;
        protected AbstractActor()
        {
            _taskBuffer = new TaskBuffer<ActorMessage<T>>();

            Task.Run(async () =>
            {
                while (true)
                {
                    var result = await _taskBuffer.ReceiveAsync();
                    await Handle(result);
                }
            });
        }
        private async Task Handle(ActorMessage<T> message)
        {
            try
            {
                await HandleMessage(message.Message);
            }
            catch (Exception ex)
            {
                message.OnError(ex);
                var result = await HandleError(ex, message);
                switch (result)
                {
                    case OkHandleResult:
                        break;

                    case NeedRetry:
                        _taskBuffer.Post(message);
                        break;

                    case NeedRetryWithDelay x:
                        await Task.Delay(x.Delay);
                        _taskBuffer.Post(message);
                        break;

                    default: throw new NotImplementedException("Not implemented handler for result");
                }

            }
        }
        /// <summary>
        /// Указывает есть ли задачи, которые нужно выполнять.
        /// </summary>
        protected bool IsBisy => _taskBuffer.TaskCount > 0;
        /// <summary>
        /// Добавляет в буфер задачу, значение которой нужно вернуть после выполнения с результатом TResponse.
        /// </summary>
        protected Task<TResponse> PostWithReply<TResponse>(Func<ReplyChannel<TResponse>, T> msgFabric) => _taskBuffer.PostAndReply<TResponse>(rc => new ActorMessage<T>(msgFabric(rc)));
        /// <summary>
        /// Обрабатывает сообщение.
        /// </summary>
        protected abstract Task HandleMessage(T message);
        /// <summary>
        /// Обработка ошибки при неудачном HandleMessage.
        /// </summary>
        protected abstract Task<HandleResult> HandleError(Exception ex, ActorMessage<T> message);
        /// <summary>
        /// Добавляет сообщение в очередь
        /// </summary>
        /// <param name="message">Сообщение</param>
        protected void Post(T message)
        {
            _taskBuffer.Post(new ActorMessage<T>(message));
        }
        /// <summary>
        /// Удаляет все сообщения из очереди
        /// </summary>
        protected void Clear()
        {
            _taskBuffer.Clear();
        }
        /// <summary>
        /// Метод ожидания отсутствия задач.
        /// </summary>
        /// <returns></returns>
        protected async Task WaitWindowEmpty()
        {
            while (IsBisy)
            {
                await Task.Delay(10);
            }
        }

    }
}
