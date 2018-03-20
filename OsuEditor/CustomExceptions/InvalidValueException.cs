using System;
using System.Runtime.Serialization;

namespace OsuEditor.CustomExceptions
{
    [Serializable]
    public class InvalidValueException : Exception
    {
        public InvalidValueException()
        {
        }

        public InvalidValueException(string message) : base(message)
        {
        }

        public InvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        //  This constructor is needed for serialization.
        protected InvalidValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}