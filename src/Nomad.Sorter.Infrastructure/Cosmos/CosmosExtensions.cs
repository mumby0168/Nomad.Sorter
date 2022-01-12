using Microsoft.Azure.CosmosRepository.AspNetCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nomad.Sorter.Application.Infrastructure;
using Nomad.Sorter.Domain.Entities;
using Nomad.Sorter.Domain.Entities.Abstractions;
using Nomad.Sorter.Infrastructure.Cosmos.Items;
using Nomad.Sorter.Infrastructure.Cosmos.Processors;
using Nomad.Sorter.Infrastructure.Cosmos.Repositories;
using Nomad.Sorter.Infrastructure.Extensions;

namespace Nomad.Sorter.Infrastructure.Cosmos;

public static class CosmosExtensions
{
    public static IServiceCollection AddCosmos(this IServiceCollection services, IHostEnvironment hostEnvironment)
    {
        services.AddCosmosRepository(x =>
        {
            x.ContainerPerItemType = true;
            x.DatabaseId = CosmosConstants.DatabaseName;

            var parcelsContainerTimeToLive = TimeSpan.FromDays(3);

            x.ContainerBuilder.Configure<Parcel>(parcelContainerOptions => parcelContainerOptions
                .WithContainer(CosmosConstants.Containers.Parcels)
                .WithPartitionKey(CosmosConstants.DefaultPartitionKey)
                .WithContainerDefaultTimeToLive(parcelsContainerTimeToLive)
                .WithChangeFeedMonitoring());

            x.ContainerBuilder.Configure<ParcelLookupByParcelIdItem>(parcelContainerOptions => parcelContainerOptions
                .WithContainer(CosmosConstants.Containers.Parcels)
                .WithPartitionKey(CosmosConstants.DefaultPartitionKey)
                .WithContainerDefaultTimeToLive(parcelsContainerTimeToLive));
        });

        if (hostEnvironment.IsNotFunctionalTests())
        {
            services.AddCosmosRepositoryChangeFeedHostedService();   
        }
        
        services.AddCosmosRepositoryItemChangeFeedProcessors(typeof(CosmosExtensions).Assembly);
        services.AddSingleton<IParcelRepository, ParcelRepository>();

        return services;
    }

    public static Parcel ToCosmosModel(this IParcel parcel) =>
        parcel as Parcel ??
        throw new InvalidOperationException(
            "a IParcel must be convertible to a Parcel in order to be stored in Cosmos DB");

    public static ParcelLookupByParcelIdItem ToParcelLookupByParcelIdItem(this IParcel parcel) =>
        new(parcel.ParcelId, parcel.DeliveryInformation.RegionId);
}