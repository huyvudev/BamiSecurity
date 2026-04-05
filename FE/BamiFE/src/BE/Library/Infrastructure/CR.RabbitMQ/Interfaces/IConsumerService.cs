namespace CR.RabbitMQ.Interfaces
{
    public interface IConsumerService
    {
        Task ReceiveMessage(byte[] message);
    }
}
