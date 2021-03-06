namespace Nomad.Sorter.Infrastructure.Messaging;

public static class ServiceBusConstants
{
    internal const string AppName = "nomad-sorter";

    public static class Services
    {
        public const string ThirdPartyMachinery = "tpm";
    }
    
    public static class Queues
    {
        public const string ParcelPreAdviceQueue = $"{AppName}/parcel-pre-advice";
    }

    public static class Topics
    {
        public const string ParcelInductedTopic = $"{Services.ThirdPartyMachinery}/parcel-inducted";
    }
}