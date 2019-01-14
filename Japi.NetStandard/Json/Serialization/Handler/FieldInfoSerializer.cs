using System;
using System.Linq;
using System.Reflection;
using NightlyCode.Japi.Extensions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="FieldInfo"/>s
    /// </summary>
    public class FieldInfoSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="FieldInfoSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public FieldInfoSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            FieldInfo fieldinfo = (FieldInfo)value;
            return new JsonObject {
                ["type"] = new JsonValue("field"),
                ["host"] = serializer.Write(fieldinfo.DeclaringType),
                ["name"] = new JsonValue(fieldinfo.Name)
            };
        }

        public object Deserialize(JsonNode json) {
            Type host = json.SelectValue<Type>("host");
            string name = json.SelectValue<string>("name");
            return host.GetFields().First(h => h.Name == name);
        }
    }
}