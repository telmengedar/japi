using System;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class LabelTargetSerializer : IJSonSerializationHandler {
        public JsonNode Serialize(object value) {
            LabelTarget target = (LabelTarget)value;
            return new JsonObject {
                ["type"] = JsonSerializer.Write(target.Type),
                ["name"] = new JsonValue(target.Name)
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.Label(
                JsonSerializer.Read<Type>(json["type"]),
                json.SelectValue<string>("name")
                );
        }
    }
}