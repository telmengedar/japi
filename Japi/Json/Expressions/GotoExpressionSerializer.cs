using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class GotoExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            GotoExpression gt = (GotoExpression)expression;
            json["value"] = JsonSerializer.Write(gt.Value);
            json["target"] = JsonSerializer.Write(gt.Target);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Goto(
                JsonSerializer.Read<LabelTarget>(json["target"]),
                JsonSerializer.Read<Expression>(json["value"])
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Goto;
            }
        }
    }
}