namespace CR.RabbitMQ.Interfaces
{
    public interface IProducerService
    {
        void PublishMessage<TEntity>(TEntity? entity, string exchangeName, string bindingKey)
            where TEntity : class;
    }
}
