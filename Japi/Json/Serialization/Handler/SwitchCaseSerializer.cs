using System.Linq;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class SwitchCaseSerializer : IJSonSerializationHandler {

        public JsonNode Serialize(object value) {
            SwitchCase sc = (SwitchCase)value;
            return new JsonObject {
                ["values"] = new JsonArray(sc.TestValues.Select(JsonSerializer.Write)),
                ["body"] = JsonSerializer.Write(sc.Body)
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.SwitchCase(
                JsonSerializer.Read<Expression>(json["body"]),
                json["values"].Select(JsonSerializer.Read<Expression>)
                );
        }
    }
}