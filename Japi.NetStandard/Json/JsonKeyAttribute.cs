using System;
using System.Reflection;

namespace NightlyCode.Japi.Json {

    /// <summary>
    /// used to specify a json path for a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonKeyAttribute : Attribute {

        /// <summary>
        /// creates a new json key attribute
        /// </summary>
        /// <param name="key"></param>
        public JsonKeyAttribute(string key) {
            Key = key;
        }

        /// <summary>
        /// name of json key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// get json key of a property
        /// </summary>
        /// <param name="property">name of property</param>
        /// <returns>name of key</returns>
        public static string GetKey(PropertyInfo property) {
#if WINDOWS_UWP
            JsonKeyAttribute attribute = property.GetCustomAttribute<JsonKeyAttribute>();
#else
            JsonKeyAttribute attribute = GetCustomAttribute(property, typeof(JsonKeyAttribute)) as JsonKeyAttribute;
#endif
            return attribute?.Key;
        }
    }
}