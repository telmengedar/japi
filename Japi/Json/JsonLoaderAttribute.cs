using System;
using System.Reflection;

namespace GoorooMania.Japi.Json {

    /// <summary>
    /// attribute used to specify custom load methods
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonLoaderAttribute : Attribute{

        /// <summary>
        /// creates a new json loader
        /// </summary>
        /// <param name="method"></param>
        public JsonLoaderAttribute(string method) {
            Method = method;
        }

        /// <summary>
        /// name of method
        /// </summary>
        public string Method { get; }

        public static string Get(PropertyInfo property) {
            JsonLoaderAttribute attribute=GetCustomAttribute(property, typeof(JsonLoaderAttribute)) as JsonLoaderAttribute;
            return attribute?.Method;
        }
    }
}