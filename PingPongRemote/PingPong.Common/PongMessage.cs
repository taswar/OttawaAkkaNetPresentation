namespace PingPong.Common
{
    public class PongMessage
    {
        public string Message { get; private set; }
        public PongMessage()
        {
            Message = "pong";
        }
    }
}