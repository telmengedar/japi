using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NightlyCode.Japi.Extensions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="MemberListBinding"/>s
    /// </summary>
    public class MemberListBindingSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="MemberBindingSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public MemberListBindingSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            MemberListBinding list = (MemberListBinding)value;
            return new JsonObject {
                ["type"]=new JsonValue("list"),
                ["member"] = serializer.Write(list.Member),
                ["initializers"] = new JsonArray(list.Initializers.Select(serializer.Write))
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.ListBind(
                serializer.Read<MemberInfo>(json.SelectNode("member")),
                json.SelectNodes("initializers").Select(serializer.Read<ElementInit>)
                );
        }
    }
}