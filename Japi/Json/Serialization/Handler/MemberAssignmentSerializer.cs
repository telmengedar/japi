using System.Linq.Expressions;
using System.Reflection;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class MemberAssignmentSerializer : IJSonSerializationHandler {
        public JsonNode Serialize(object value) {
            MemberAssignment assignment = (MemberAssignment)value;
            return new JsonObject {
                ["type"]=new JsonValue("assignment"),
                ["member"] = JsonSerializer.Write(assignment.Member),
                ["expression"] = JsonSerializer.Write(assignment.Expression)
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.Bind(
                JsonSerializer.Read<MemberInfo>(json["member"]),
                JsonSerializer.Read<Expression>(json["expression"])
                );
        }
    }
}