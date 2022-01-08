using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Nomad.Testing.MassTransit;

public class MassTransitConsumerInvoker
{
    private readonly IServiceProvider _serviceProvider;

    public MassTransitConsumerInvoker(IServiceProvider serviceProvider) => 
        _serviceProvider = serviceProvider;

    public async Task Invoke<TMessage, TConsumer>(TMessage message) where TMessage : class where TConsumer : IConsumer<TMessage>
    {
        using var scope  = _serviceProvider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
        var context = new Mock<ConsumeContext<TMessage>>();
        context.SetupGet(o => o.Message).Returns(message);
        await consumer.Consume(context.Object);
    }
}