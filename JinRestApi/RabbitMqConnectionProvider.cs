using RabbitMQ.Client;

public class RabbitMqConnectionProvider : IRabbitMqConnectionProvider
{
    public IConnection? Connection { get; }
    public bool IsConnected => Connection?.IsOpen ?? false;

    public RabbitMqConnectionProvider(IConnection? connection)
    {
        Connection = connection;
    }
}
