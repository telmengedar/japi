using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GoorooMania.Japi.Json.Expressions {
    public class MethodCallExpressionSerializer : ISpecificExpressionSerializer {
        public void Serialize(JsonObject json, Expression expression) {
            MethodCallExpression call = (MethodCallExpression)expression;
            json["host"] = JsonSerializer.Write(call.Object);
            json["method"] = JsonSerializer.Write(call.Method);
            json["arguments"] = new JsonArray(call.Arguments.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Call(
                JsonSerializer.Read<Expression>(json["host"]),
                JsonSerializer.Read<MethodInfo>(json["method"]),
                json["arguments"].Select(JsonSerializer.Read<Expression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Call;
            }
        }
    }
}