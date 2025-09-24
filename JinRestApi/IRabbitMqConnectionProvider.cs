using RabbitMQ.Client;

public interface IRabbitMqConnectionProvider
{
    IConnection? Connection { get; }
    bool IsConnected { get; }
}
