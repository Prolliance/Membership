using System;

namespace Prolliance.Membership.Business.Exceptions
{
    public class ObjectExistsException : Exception
    {
        public ObjectExistsException()
        { }

        public ObjectExistsException(string message)
            : base(message) { }

        public ObjectExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
