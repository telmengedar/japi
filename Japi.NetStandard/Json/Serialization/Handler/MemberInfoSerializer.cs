using System;
using System.Reflection;
using NightlyCode.Japi.Extensions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="MemberInfo"/>s
    /// </summary>
    /// <remarks>
    /// as with memberbindings this serializer just deserializes existing data
    /// </remarks>
    public class MemberInfoSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="MemberInfoSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public MemberInfoSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            throw new NotImplementedException("Use specific serializer");
        }

        public object Deserialize(JsonNode json) {
            switch(json.SelectValue<string>("type")) {
                case "field":
                    return serializer.Read<FieldInfo>(json);
                case "property":
                    return serializer.Read<PropertyInfo>(json);
                case "method":
                    return serializer.Read<MethodInfo>(json);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}