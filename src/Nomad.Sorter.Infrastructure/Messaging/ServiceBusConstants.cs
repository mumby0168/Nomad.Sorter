namespace Nomad.Sorter.Infrastructure.Messaging;

internal static class ServiceBusConstants
{
    internal const string AppName = "nomad-sorter";
    
    internal static class Queues
    {
        public const string ParcelPreAdviceQueue = $"{AppName}/parcel-pre-advice";
    }
}