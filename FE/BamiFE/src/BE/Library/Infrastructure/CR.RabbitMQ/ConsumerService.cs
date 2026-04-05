using CR.RabbitMQ.Configs;
using CR.RabbitMQ.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CR.RabbitMQ
{
    public abstract class ConsumerService
        : RabbitMqService,
            IConsumerService,
            IHostedService,
            IDisposable
    {
        private readonly ILogger _logger;
        protected IModel _model;
        protected IConnection _connection;
        protected readonly string _queueName;
        protected readonly string _exchangeName;
        protected readonly string _routingKey;
        private readonly string _consumerName;

        protected ConsumerService(
            ILogger logger,
            IOptions<RabbitMqConfig> config,
            string queueName,
            string exchangeName,
            string routingKey
        )
            : base(config)
        {
            _logger = logger;
            _connection = CreateConnection();
            _model = _connection.CreateModel();
            //var queueArgs = new Dictionary<string, object>
            //{
            //    { "x-queue-type", "quorum" } // Đặt loại hàng đợi thành quorum
            //};
            //_model.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: queueArgs);
            //_model.ExchangeDeclare(RabbitExchangeNames.Name, ExchangeType.Direct, durable: true, autoDelete: false);
            _queueName = queueName;
            _exchangeName = exchangeName;
            _routingKey = routingKey;
            _consumerName =
                $"Consumer(queueName: {_queueName}, exchangeName: {_exchangeName}, routingKey: {_routingKey})";
            _model.QueueBind(_queueName, exchangeName, routingKey);
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
            if (_connection.IsOpen)
                _connection.Close();
        }

        protected async Task ReadMessages()
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (sender, @event) =>
            {
                var body = @event.Body.ToArray();
                await ReceiveMessage(body);
                _model.BasicAck(@event.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{_consumerName} Hosted Service running.");
            await ReadMessages();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{_consumerName} Hosted Service is stopping.");
            return Task.CompletedTask;
        }

        public abstract Task ReceiveMessage(byte[] message);
    }
}
