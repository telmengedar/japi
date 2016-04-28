using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class LoopExpressionSerializer : ISpecificExpressionSerializer {
        public void Serialize(JsonObject json, Expression expression) {
            LoopExpression loop = (LoopExpression)expression;
            json["body"] = JsonSerializer.Write(loop.Body);
            json["break"] = JsonSerializer.Write(loop.BreakLabel);
            json["continue"] = JsonSerializer.Write(loop.ContinueLabel);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Loop(
                JsonSerializer.Read<Expression>(json["body"]),
                JsonSerializer.Read<LabelTarget>(json["break"]),
                JsonSerializer.Read<LabelTarget>(json["continue"])
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Loop;
            }
        }
    }
}