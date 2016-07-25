using System;
using System.Linq;
using System.Reflection;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="PropertyInfo"/>s
    /// </summary>
    public class PropertyInfoSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="PropertyInfoSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public PropertyInfoSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            PropertyInfo property = (PropertyInfo)value;
            return new JsonObject {
                ["type"]=new JsonValue("property"),
                ["host"] = serializer.Write(property.DeclaringType),
                ["name"] = new JsonValue(property.Name),
            };
        }

        public object Deserialize(JsonNode json) {
            Type host = serializer.Read<Type>(json["host"]);
            string name = json.SelectValue<string>("name");
            return host.GetProperties().First(h => h.Name == name);
        }
    }
}