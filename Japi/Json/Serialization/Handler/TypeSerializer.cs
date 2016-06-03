using System;
using System.Reflection;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class TypeSerializer : IJSonSerializationHandler {

        public JsonNode Serialize(object value) {
            Type type = (Type)value;
#if WINDOWS_UWP
            return new JsonObject
            {
                ["assembly"] = new JsonValue(type.GetTypeInfo().Assembly.GetName().Name),
                ["namespace"] = new JsonValue(type.Namespace),
                ["name"] = new JsonValue(type.Name)
            };
#else
            return new JsonObject {
                ["assembly"] = new JsonValue(type.Assembly.GetName().Name),
                ["namespace"] = new JsonValue(type.Namespace),
                ["name"] = new JsonValue(type.Name)
            };
#endif
        }

        public object Deserialize(JsonNode json) {
            return Type.GetType(json.SelectValue<string>("namespace") + "." + json.SelectValue<string>("name") + ", " + json.SelectValue<string>("assembly"));
        }
    }
}