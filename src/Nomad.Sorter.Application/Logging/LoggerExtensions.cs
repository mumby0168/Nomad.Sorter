using Microsoft.Extensions.Logging;
using Nomad.Sorter.Domain.Entities.Abstractions;

namespace Nomad.Sorter.Application.Logging;

public static class LoggerExtensions
{
    public static void LogParcelAssociated(this ILogger logger, IParcel parcel) =>
        LoggerMessageDefinitions.ParcelAssociated(logger, parcel.ParcelId, parcel.AssociatedBayId!, null!);
}