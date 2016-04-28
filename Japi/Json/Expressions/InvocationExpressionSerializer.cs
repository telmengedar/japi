using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class InvocationExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            InvocationExpression invocation = (InvocationExpression)expression;
            json["expression"] = JsonSerializer.Write(invocation.Expression);
            json["arguments"] = new JsonArray(invocation.Arguments.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Invoke(
                JsonSerializer.Read<Expression>(json["expression"]),
                json["arguments"].Select(JsonSerializer.Read<Expression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Invoke;
            }
        }
    }
}