using System;

namespace BlackMirror.Models.Exceptions
{
    [Serializable]
    public class StatusNotChangedException : Exception
    {
        public StatusNotChangedException() : base() { }
        public StatusNotChangedException(string message) : base(message) { }
        public StatusNotChangedException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected StatusNotChangedException(System.Runtime.Serialization.SerializationInfo info,
                                             System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
