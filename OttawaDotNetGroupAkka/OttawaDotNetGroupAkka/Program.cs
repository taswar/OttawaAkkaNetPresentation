using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;

namespace OttawaDotNetGroupAkka
{
    class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("pingSystem");

            var pongActor = system.ActorOf(Props.Create<PongActorManager>(), "pongActorManager");
            
            var pingActor = system.ActorOf(Props.Create(() => new PingActor(pongActor)), "pingActor");           

            var input = Console.ReadLine();

            while (input != "exit")
            {
                pingActor.Tell(new PingMessage());
                input = Console.ReadLine();
            }


            system.Terminate();

        }
    }

    public class PongActorManager : ReceiveActor
    {
        public PongActorManager()
        {
            var child = Context.ActorOf(Props.Create<PongActor>(), "pongActor");

            Receive<PongMessage>(x => child.Tell(x));
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(// or AllForOneStrategy
            100,
            TimeSpan.FromSeconds(3),
            x =>
            {
                // Error that we can't recover from, stop the failing child
                if (x is NotSupportedException) return Directive.Stop;

                // otherwise restart the failing child
                return Directive.Restart;
            });
        }
    }
    


    public class PingActor : ReceiveActor
    {
        private readonly IActorRef _pongActor;

        public PingActor(IActorRef pongActor)
        {
            _pongActor = pongActor;
            //Receive<PingMessage>(message => Process(message));
            Receive<PingMessage>(message => ProcessMessage(message));
        }

        private void ProcessMessage(PingMessage message)
        {
            Console.WriteLine(message.Message);
            _pongActor.Tell(new PongMessage());
        }
    }

    public class PingMessage
    {
        public string Message { get; private set; }

        public PingMessage()
        {
            Message = "ping";
        }
    }

    public class PongActor : ReceiveActor
    {

        private int _counter = 1;
        public PongActor()
        {
            //Receive<PingMessage>(message => Process(message));
            Receive<PongMessage>(message => ProcessMessage(message));
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Restarting PongActor************************");
            // put message back in mailbox for re-processing after restart            
            Self.Tell(message, Sender);
        }

        private void ProcessMessage(PongMessage message)
        {

            if (_counter%4 == 0)
            {
                throw new Exception("ahhhhhhh");
            }
            Console.WriteLine(message.Message + " " + Self.Path.ToStringWithAddress() + " " + _counter);
            _counter++;
            //Sender.Tell(new PingMessage());
        }
    }

    public class PongMessage
    {
        public string Message { get; private set; }

        public PongMessage()
        {
            Message = "pong";
        }
    }
}
