namespace Viv2.API.Core.Exceptions
{
    public class UserStoreException : System.Exception
    {
        public UserStoreException() : base() {}
        public UserStoreException(string message) : base(message) {}
    }
}