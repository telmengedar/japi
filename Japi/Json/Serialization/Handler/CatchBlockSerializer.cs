using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class CatchBlockSerializer : IJSonSerializationHandler {

        public JsonNode Serialize(object value) {
            CatchBlock catchblock = (CatchBlock)value;
            return new JsonObject {
                ["variable"] = JsonSerializer.Write(catchblock.Variable),
                ["body"] = JsonSerializer.Write(catchblock.Body),
                ["filter"] = JsonSerializer.Write(catchblock.Filter)
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.Catch(
                JsonSerializer.Read<ParameterExpression>(json["variable"]),
                JsonSerializer.Read<Expression>(json["body"]),
                JsonSerializer.Read<Expression>(json["filter"])
                );
        }
    }
}