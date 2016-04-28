using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class LambdaExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            LambdaExpression lamda = (LambdaExpression)expression;

            json["parameters"] = new JsonArray(lamda.Parameters.Select(JsonSerializer.Write));
            json["name"] = new JsonValue(lamda.Name);
            json["body"] = JsonSerializer.Write(lamda.Body);
            //json["return"] = JsonSerializer.Write(lamda.ReturnType);
            json["tail"] = new JsonValue(lamda.TailCall);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Lambda(
                JsonSerializer.Read<Expression>(json["body"]),
                json.SelectValue<string>("name"),
                json.SelectValue<bool>("tail"),
                json["parameters"].Select(JsonSerializer.Read<ParameterExpression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.Lambda; }
        }
    }
}