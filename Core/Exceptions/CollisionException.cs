namespace Viv2.API.Core.Exceptions
{
    public class CollisionException : System.Exception
    {
        public CollisionException() : base() {}
        public CollisionException(string message) : base(message) {}
    }
}