namespace Exceptions
{
    public class EmptyLogInExcaption : Exception 
    {
        public EmptyLogInExcaption() { }
        public EmptyLogInExcaption(string? message) : base(message) { }
        public EmptyLogInExcaption(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
