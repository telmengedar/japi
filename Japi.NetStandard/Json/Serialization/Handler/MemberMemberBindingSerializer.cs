using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NightlyCode.Japi.Extensions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="MemberMemberBinding"/>s
    /// </summary>
    public class MemberMemberBindingSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="MemberMemberBindingSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public MemberMemberBindingSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            MemberMemberBinding binding = (MemberMemberBinding)value;

            return new JsonObject
            {
                ["type"]=new JsonValue("member"),
                ["member"] = serializer.Write(binding.Member),
                ["bindings"] = new JsonArray(binding.Bindings.Select(serializer.Write))
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.MemberBind(
                serializer.Read<MemberInfo>(json.SelectNode("member")),
                json.SelectNodes("bindings").Select(serializer.Read<MemberBinding>)
            );
        }
    }
}