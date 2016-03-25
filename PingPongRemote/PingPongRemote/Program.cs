using Akka.Actor;
using System;

namespace PingPongRemote
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Remote ping system starting");

            var system = ActorSystem.Create("pingSystem");
          
            Console.ReadLine();

            system.Terminate();
        }
    }
}
