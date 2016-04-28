using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GoorooMania.Japi.Json.Expressions {
    public class SwitchExpressionSerializer : ISpecificExpressionSerializer {
        public void Serialize(JsonObject json, Expression expression) {
            SwitchExpression @switch = (SwitchExpression)expression;
            json["value"] = JsonSerializer.Write(@switch.SwitchValue);
            json["default"] = JsonSerializer.Write(@switch.DefaultBody);
            json["comparison"] = JsonSerializer.Write(@switch.Comparison);
            json["cases"] = new JsonArray(@switch.Cases.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Switch(
                JsonSerializer.Read<Expression>(json["value"]),
                JsonSerializer.Read<Expression>(json["default"]),
                JsonSerializer.Read<MethodInfo>(json["comparison"]),
                json["cases"].Select(JsonSerializer.Read<SwitchCase>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Switch;
            }
        }
    }
}