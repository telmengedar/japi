using System;
using System.Linq;
using System.Reflection;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class PropertyInfoSerializer : IJSonSerializationHandler {

        public JsonNode Serialize(object value) {
            PropertyInfo property = (PropertyInfo)value;
            return new JsonObject {
                ["type"]=new JsonValue("property"),
                ["host"] = JsonSerializer.Write(property.DeclaringType),
                ["name"] = new JsonValue(property.Name),
            };
        }

        public object Deserialize(JsonNode json) {
            Type host = JsonSerializer.Read<Type>(json["host"]);
            string name = json.SelectValue<string>("name");
            return host.GetProperties().First(h => h.Name == name);
        }
    }
}