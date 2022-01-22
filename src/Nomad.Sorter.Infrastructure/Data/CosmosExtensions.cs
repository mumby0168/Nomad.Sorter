using Microsoft.Azure.CosmosRepository.AspNetCore.Extensions;
using Microsoft.Azure.CosmosRepository.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Infrastructure.Data.Items;
using Nomad.Sorter.Infrastructure.Data.Repositories;
using Nomad.Sorter.Infrastructure.Extensions;

namespace Nomad.Sorter.Infrastructure.Data;

public static class CosmosExtensions
{
    public static IServiceCollection AddCosmos(this IServiceCollection services, IHostEnvironment hostEnvironment)
    {
        services.AddCosmosRepository(repositoryOptions =>
        {
            repositoryOptions.ContainerPerItemType = true;
            repositoryOptions.DatabaseId = CosmosConstants.DatabaseName;

            repositoryOptions
                .ConfigureParcelsContainer()
                .ConfigureBaysContainer();
        });

        if (hostEnvironment.IsNotFunctionalTests())
        {
            services.AddCosmosRepositoryChangeFeedHostedService();
        }

        services.AddCosmosRepositoryItemChangeFeedProcessors(typeof(CosmosExtensions).Assembly);
        services.AddSingleton<IParcelRepository, ParcelRepository>();
        services.AddSingleton<IBayRepository, BayRepository>();

        return services;
    }

    private static RepositoryOptions ConfigureBaysContainer(this RepositoryOptions repositoryOptions)
    {
        var containerBuilder = repositoryOptions.ContainerBuilder;

        containerBuilder.Configure<Bay>(bayContainerOptions => bayContainerOptions
            .WithContainer(CosmosConstants.Containers.Bays)
            .WithPartitionKey(CosmosConstants.DefaultPartitionKey));

        return repositoryOptions;
    }

    private static RepositoryOptions ConfigureParcelsContainer(this RepositoryOptions repositoryOptions)
    {
        var containerBuilder = repositoryOptions.ContainerBuilder;
        var parcelsContainerTimeToLive = TimeSpan.FromDays(3);

        containerBuilder.Configure<Parcel>(parcelContainerOptions =>
            parcelContainerOptions
                .WithContainer(CosmosConstants.Containers.Parcels)
                .WithPartitionKey(CosmosConstants.DefaultPartitionKey)
                .WithContainerDefaultTimeToLive(parcelsContainerTimeToLive)
                .WithChangeFeedMonitoring(options =>
                {
                    options.PollInterval = TimeSpan.FromSeconds(2);
                }));

        containerBuilder.Configure<ParcelLookupByParcelIdItem>(parcelContainerOptions =>
            parcelContainerOptions
                .WithContainer(CosmosConstants.Containers.Parcels)
                .WithPartitionKey(CosmosConstants.DefaultPartitionKey)
                .WithContainerDefaultTimeToLive(parcelsContainerTimeToLive));

        return repositoryOptions;
    }

    public static Parcel ToCosmosModel(this IParcel parcel) =>
        parcel as Parcel ??
        throw new InvalidOperationException(
            "An IParcel must be convertible to a Parcel in order to be stored in Cosmos DB");

    public static Bay ToCosmosModel(this IBay bay) =>
        bay as Bay ??
        throw new InvalidOperationException(
            "An IBay must be convertible to a Bay in order to be stored in Cosmos DB");

    public static ParcelLookupByParcelIdItem ToParcelLookupByParcelIdItem(this IParcel parcel) =>
        new(parcel.ParcelId, parcel.DeliveryInformation.RegionId);
}