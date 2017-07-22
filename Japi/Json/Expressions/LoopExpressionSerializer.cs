#if !FRAMEWORK35

using System.Collections.Generic;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="LoopExpression"/>s
    /// </summary>
    public class LoopExpressionSerializer : ISpecificExpressionSerializer {
        IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="LoopExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public LoopExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            LoopExpression loop = (LoopExpression)expression;
            json["body"] = serializer.Write(loop.Body);
            json["break"] = serializer.Write(loop.BreakLabel);
            json["continue"] = serializer.Write(loop.ContinueLabel);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Loop(
                serializer.Read<Expression>(json["body"]),
                serializer.Read<LabelTarget>(json["break"]),
                serializer.Read<LabelTarget>(json["continue"])
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

#endif