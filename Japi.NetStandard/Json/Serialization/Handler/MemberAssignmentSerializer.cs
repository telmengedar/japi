using System.Linq.Expressions;
using System.Reflection;
using NightlyCode.Japi.Extensions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="MemberAssignment"/>s
    /// </summary>
    public class MemberAssignmentSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="MemberAssignmentSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public MemberAssignmentSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            MemberAssignment assignment = (MemberAssignment)value;
            return new JsonObject {
                ["type"]=new JsonValue("assignment"),
                ["member"] = serializer.Write(assignment.Member),
                ["expression"] = serializer.Write(assignment.Expression)
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.Bind(
                serializer.Read<MemberInfo>(json.SelectNode("member")),
                serializer.Read<Expression>(json.SelectNode("expression"))
                );
        }
    }
}