using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class ConditionalExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            ConditionalExpression conditional = (ConditionalExpression)expression;
            json["test"] = JsonSerializer.Write(conditional.Test);
            json["iftrue"] = JsonSerializer.Write(conditional.IfTrue);
            json["iffalse"] = JsonSerializer.Write(conditional.IfFalse);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Condition(
                JsonSerializer.Read<Expression>(json["test"]),
                JsonSerializer.Read<Expression>(json["iftrue"]),
                JsonSerializer.Read<Expression>(json["iffalse"])
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.Conditional; }
        }
    }
}