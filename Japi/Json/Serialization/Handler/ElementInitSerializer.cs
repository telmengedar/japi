using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class ElementInitSerializer : IJSonSerializationHandler {
        public JsonNode Serialize(object value) {
            ElementInit init = (ElementInit)value;
            return new JsonObject {
                ["method"] = JsonSerializer.Write(init.AddMethod),
                ["arguments"] = new JsonArray(init.Arguments.Select(JsonSerializer.Write))
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.ElementInit(
                JsonSerializer.Read<MethodInfo>(json["method"]),
                json["arguments"].Select(JsonSerializer.Read<Expression>)
                );
        }
    }
}