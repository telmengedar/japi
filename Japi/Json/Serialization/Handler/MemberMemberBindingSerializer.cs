using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class MemberMemberBindingSerializer : IJSonSerializationHandler {

        public JsonNode Serialize(object value) {
            MemberMemberBinding binding = (MemberMemberBinding)value;

            return new JsonObject
            {
                ["type"]=new JsonValue("member"),
                ["member"] = JsonSerializer.Write(binding.Member),
                ["bindings"] = new JsonArray(binding.Bindings.Select(JsonSerializer.Write))
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.MemberBind(
                JsonSerializer.Read<MemberInfo>(json["member"]),
                json["bindings"].Select(JsonSerializer.Read<MemberBinding>)
                );
        }
    }
}