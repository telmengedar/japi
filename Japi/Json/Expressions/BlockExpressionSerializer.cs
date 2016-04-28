using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class BlockExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            BlockExpression block = (BlockExpression)expression;
            json["expressions"] = new JsonArray(block.Expressions.Select(JsonSerializer.Write));
            json["variables"] = new JsonArray(block.Variables.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Block(
                json["variables"].Select(JsonSerializer.Read<ParameterExpression>),
                json["expressions"].Select(JsonSerializer.Read<Expression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.Block; }
        }
    }
}