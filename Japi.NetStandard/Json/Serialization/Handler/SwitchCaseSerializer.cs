#if !FRAMEWORK35

using System.Linq;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="SwitchCase"/>s
    /// </summary>
    public class SwitchCaseSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="SwitchCaseSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public SwitchCaseSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            SwitchCase sc = (SwitchCase)value;
            return new JsonObject {
                ["values"] = new JsonArray(sc.TestValues.Select(serializer.Write)),
                ["body"] = serializer.Write(sc.Body)
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.SwitchCase(
                serializer.Read<Expression>(json["body"]),
                json["values"].Select(serializer.Read<Expression>)
                );
        }
    }
}

#endif