namespace KEGE_Participants.Models.Exceptions
{
    internal class ConfigurationException : Exception
    {
        public ConfigurationException() { }
        public ConfigurationException(string? message) : base(message) { }
        public ConfigurationException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
