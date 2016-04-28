using System;
using System.Linq;
using System.Reflection;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class FieldInfoSerializer : IJSonSerializationHandler {
        public JsonNode Serialize(object value) {
            FieldInfo fieldinfo = (FieldInfo)value;
            return new JsonObject {
                ["type"] = new JsonValue("field"),
                ["host"] = JsonSerializer.Write(fieldinfo.DeclaringType),
                ["name"] = new JsonValue(fieldinfo.Name)
            };
        }

        public object Deserialize(JsonNode json) {
            Type host = JsonSerializer.Read<Type>(json["host"]);
            string name = json.SelectValue<string>("name");
            return host.GetFields().First(h => h.Name == name);
        }
    }
}