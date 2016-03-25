using Akka.Actor;
using PingPong.Common;
using System;

namespace PingPongLocal
{
    class Program
    {
        static void Main(string[] args)
        {

            var system = ActorSystem.Create("pingSystem");

            var pongActor = system.ActorOf(Props.Create<PongActor>(), "pongActor");

            var pingActor = system.ActorOf(Props.Create(() => new PingActor(pongActor)), "pingActor");

            string input = Console.ReadLine();

            while (input != "exit")
            {                
                pingActor.Tell(new PingMessage());
                input = Console.ReadLine();
            }

            system.Terminate();

        }
    }
}
