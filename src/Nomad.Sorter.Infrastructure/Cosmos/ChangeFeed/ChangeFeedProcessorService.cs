using System.Collections.Concurrent;
using MassTransit.Util;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.CosmosRepository.Options;
using Microsoft.Azure.CosmosRepository.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Nomad.Sorter.Domain.Entities;

namespace Nomad.Sorter.Infrastructure.Cosmos.ChangeFeed;

public class ChangeFeedProcessorService : BackgroundService
{
    private readonly ILogger<ChangeFeedProcessorService> _logger;
    private readonly IOptions<RepositoryOptions> _repositoryOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICosmosContainerService _containerService;
    private readonly CosmosClient _cosmosClient;
    private static readonly ConcurrentDictionary<Type, Type> Handlers = new();

    public ChangeFeedProcessorService(ILogger<ChangeFeedProcessorService> logger,
        IOptions<RepositoryOptions> repositoryOptions, IServiceProvider serviceProvider,
        ICosmosContainerService containerService)
    {
        _logger = logger;
        _repositoryOptions = repositoryOptions;
        _serviceProvider = serviceProvider;
        _containerService = containerService;
        _cosmosClient = new CosmosClient(repositoryOptions.Value.CosmosConnectionString);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting change feed processor service");
        
        Database database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_repositoryOptions.Value.DatabaseId,
            cancellationToken: stoppingToken);
        Container leastContainer =
            await database.CreateContainerIfNotExistsAsync("lease", "/id", cancellationToken: stoppingToken);

        var parcelsContainer = database.GetContainer("parcels");

        var changeFeedProcessor = parcelsContainer
            .GetChangeFeedProcessorBuilder<JObject>(AppConstants.Name, OnChangesDelegate)
            .WithErrorNotification((token, exception) =>
            {
                _logger.LogError(exception, "Failed {Token}", token);
                return Task.CompletedTask;
            })
            .WithLeaseContainer(leastContainer)
            .WithInstanceName(Environment.MachineName)
            .Build();
        
        await changeFeedProcessor.StartAsync();
        
        _logger.LogInformation("Successfully Started change feed processor service");

        stoppingToken.Register(() =>
        {
            _logger.LogInformation("Stopping change feed processor");
            changeFeedProcessor.StopAsync().Wait(TimeSpan.FromSeconds(5));
            _logger.LogInformation("Stopped change feed processor");
        });
    }

    private async Task OnChangesDelegate(ChangeFeedProcessorContext context, IReadOnlyCollection<JObject> changes,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Changes detected {ChangeCount}", changes.Count);
        foreach (var change in changes)
        {
            var typeToken = change["type"];

            if (typeToken is null)
            {
                _logger.LogInformation("Item does not have required type field {Json}", change.ToString());
            }

            var type = typeToken.Value<string>();

            var task = type switch
            {
                nameof(Parcel) => InvokeHandler(typeof(Parcel), change, cancellationToken),
                _ => NotSupported(type)
            };

            await task;
        }
    }

    private async Task InvokeHandler(Type itemType, JObject instance, CancellationToken cancellationToken)
    {
        var item = instance.ToObject(itemType);

        Type? handlerType = null;

        if (Handlers.ContainsKey(itemType) is false)
        {
            handlerType = typeof(IChangeFeedItemProcessor<>).MakeGenericType(itemType);
            Handlers[itemType] = handlerType;
        }

        handlerType ??= Handlers[itemType];
        
        await (ValueTask) handlerType.GetMethod("HandleAsync")?
            .Invoke(_serviceProvider.GetRequiredService(handlerType), new[] {item, cancellationToken})!;
    }


    private Task NotSupported(string type)
    {
        _logger.LogInformation("No handler for managing changes of type {Type}", type);
        return Task.CompletedTask;
    }
}