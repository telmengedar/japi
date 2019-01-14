#if !FRAMEWORK35

using System.Linq.Expressions;
using NightlyCode.Japi.Extensions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="CatchBlock"/>s
    /// </summary>
    public class CatchBlockSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="CatchBlockSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public CatchBlockSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            CatchBlock catchblock = (CatchBlock)value;
            return new JsonObject {
                ["variable"] = serializer.Write(catchblock.Variable),
                ["body"] = serializer.Write(catchblock.Body),
                ["filter"] = serializer.Write(catchblock.Filter)
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.Catch(
                serializer.Read<ParameterExpression>(json.SelectNode("variable")),
                serializer.Read<Expression>(json.SelectNode("body")),
                serializer.Read<Expression>(json.SelectNode("filter"))
            );
        }
    }
}

#endif