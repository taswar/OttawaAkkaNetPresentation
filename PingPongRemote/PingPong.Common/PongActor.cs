using Akka.Actor;
using System;

namespace PingPong.Common
{
    public class PongActor : ReceiveActor
    {
        public PongActor()
        {

            Receive<PongMessage>(message => ProcessMessage(message));
        }

        private void ProcessMessage(PongMessage message)
        {
            Console.WriteLine(message.Message);
        }
    }

}