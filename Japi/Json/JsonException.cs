using System;
using System.Runtime.Serialization;

namespace NightlyCode.Japi.Json
{

    /// <summary>
    /// exception during handling of json data
    /// </summary>
    public class JsonException : Exception {

        /// <summary>
        /// creates a new json exception
        /// </summary>
        public JsonException(string message)
            : base(message) {}

        /// <summary>
        /// creates a new json exception
        /// </summary>
        public JsonException(string message, Exception innerException)
            : base(message, innerException) {}

#if !WINDOWS_UWP
        /// <summary>
        /// creates a new json exception
        /// </summary>
        protected JsonException(SerializationInfo info, StreamingContext context)
            : base(info, context) {}
#endif
    }
}