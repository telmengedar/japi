using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class MemberListBindingSerializer : IJSonSerializationHandler {

        public JsonNode Serialize(object value) {
            MemberListBinding list = (MemberListBinding)value;
            return new JsonObject {
                ["type"]=new JsonValue("list"),
                ["member"] = JsonSerializer.Write(list.Member),
                ["initializers"] = new JsonArray(list.Initializers.Select(JsonSerializer.Write))
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.ListBind(
                JsonSerializer.Read<MemberInfo>(json["member"]),
                json["initializers"].Select(JsonSerializer.Read<ElementInit>)
                );
        }
    }
}