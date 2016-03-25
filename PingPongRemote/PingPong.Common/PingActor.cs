using Akka.Actor;
using System;

namespace PingPong.Common
{
    public class PingActor: ReceiveActor
    {
        private readonly IActorRef _pongActor;

        public PingActor(IActorRef pongActor)
        {
            _pongActor = pongActor;
            Receive<PingMessage>(message => ProcessMessage(message));
        }

        private void ProcessMessage(PingMessage message)
        {
            Console.WriteLine(message.Message);

            _pongActor.Tell(new PongMessage());
        }
    }
}
