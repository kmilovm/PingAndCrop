namespace PingAndCrop.Domain.Constants
{
    public struct StringMessages
    {
        public const string InitiatingRequest = "Initializing request {0} at {1}";
        public const string ErrorProcessingRequest = "An error occurred while processing request {0}";
        public const string NoMessagesAtQueue = "The queue requested: {0} has no pending messages";
        public const string FinalizingRequest = "Finalizing request {0} at {1}";
    }
}
