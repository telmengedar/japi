using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NightlyCode.Japi.Extensions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="ElementInit"/>s
    /// </summary>
    public class ElementInitSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="ElementInitSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public ElementInitSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            ElementInit init = (ElementInit)value;
            return new JsonObject {
                ["method"] = serializer.Write(init.AddMethod),
                ["arguments"] = new JsonArray(init.Arguments.Select(serializer.Write))
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.ElementInit(
                serializer.Read<MethodInfo>(json.SelectNode("method")),
                json.SelectNodes("arguments").Select(serializer.Read<Expression>)
            );
        }
    }
}