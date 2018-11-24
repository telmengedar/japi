using System;
using System.Linq;
using System.Reflection;

namespace NightlyCode.Japi.Json {

    /// <summary>
    /// attribute used for variant serialization types
    /// </summary>
    /// <remarks>
    /// variant types are stored with their type info to ensure proper deserialization
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class VariantAttribute : Attribute {

        /// <summary>
        /// determines whether the specified property has a variant attribute
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsVariant(PropertyInfo property) {
#if FRAMEWORK35
            return property.GetCustomAttributes(typeof(VariantAttribute), false).Any();
#else
            return property.GetCustomAttribute<VariantAttribute>() != null;
#endif
        }
    }
}