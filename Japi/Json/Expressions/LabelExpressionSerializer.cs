using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class LabelExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            LabelExpression label = (LabelExpression)expression;
            json["target"] = JsonSerializer.Write(label.Target);
            json["default"] = JsonSerializer.Write(label.DefaultValue);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Label(
                JsonSerializer.Read<LabelTarget>(json["target"]),
                JsonSerializer.Read<Expression>(json["default"])
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Label;
            }
        }
    }
}