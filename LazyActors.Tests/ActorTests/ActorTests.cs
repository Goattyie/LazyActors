using LazyActors.Tests.Actors;
using NUnit.Framework;
using System.Threading.Tasks;

namespace LazyActors.ActorTests
{
    public class ActorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConsoleLoggerTest()
        {
            var actor = new ConsoleLoggerActor();
            actor.Log("Hello world");
            actor.Log("Bye");
        }

        [Test]
        public async Task IncrementTest()
        {
            var actor = new IncrementActor();
            for(int i = 0; i < 100; i++)
            {
                actor.Add();
            }
            await actor.WaitEndTasks();
            Assert.AreEqual(100, actor.GetState());
        }
        [Test]
        public async Task ErrorActorTest()
        {
            var actor = new ErrorActor();
            for (int i = 0; i < 100; i++)
            {
                actor.Add(i);
            }
            await actor.WaitEndTasks();
            Assert.AreEqual(20, actor.GetErrors);
        }
        [Test]
        public async Task SumActorTest()
        {
            var actor = new SumatorActor();
            var sum = await actor.Sum(3, 4, 10);
            await Task.Delay(2000);
            Assert.AreEqual(17, sum);
        }
    }
}