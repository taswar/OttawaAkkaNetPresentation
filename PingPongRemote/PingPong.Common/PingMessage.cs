namespace PingPong.Common
{
    public class PingMessage
    {
        public string Message { get; private set; }

        public PingMessage()
        {
            Message = "ping";
        }
    }
}