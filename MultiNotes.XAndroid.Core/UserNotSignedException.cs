using System;
using System.Runtime.Serialization;

namespace MultiNotes.XAndroid.Core
{
    [Serializable]
    internal class UserNotSignedException : Exception
    {
        public UserNotSignedException() : this("Trying to perform an operation that requires signed user.")
        {
        }

        public UserNotSignedException(string message) : base(message)
        {
        }

        public UserNotSignedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotSignedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}