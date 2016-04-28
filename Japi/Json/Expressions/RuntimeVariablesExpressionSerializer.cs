using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class RuntimeVariablesExpressionSerializer : ISpecificExpressionSerializer {
        public void Serialize(JsonObject json, Expression expression) {
            RuntimeVariablesExpression rtv = (RuntimeVariablesExpression)expression;
            json["variables"] = new JsonArray(rtv.Variables.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.RuntimeVariables(
                json["variables"].Select(JsonSerializer.Read<ParameterExpression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.RuntimeVariables;
            }
        }
    }
}