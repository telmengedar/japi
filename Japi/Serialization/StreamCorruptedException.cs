using System;
using System.Runtime.Serialization;

namespace NightlyCode.Japi.Serialization {

    /// <summary>
    /// indicates that the stream is corrupted
    /// </summary>
    public class StreamCorruptedException : Exception {

        /// <summary>
        /// creates a new stream corrupted exception
        /// </summary>
        public StreamCorruptedException(string message)
            : base(message) {}

        /// <summary>
        /// creates a new stream corrupted exception
        /// </summary>
        public StreamCorruptedException(string message, Exception innerException)
            : base(message, innerException) {}

        /// <summary>
        /// creates a new stream corrupted exception
        /// </summary>
        protected StreamCorruptedException(SerializationInfo info, StreamingContext context)
            : base(info, context) {}
    }
}