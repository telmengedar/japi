using System.Collections.Generic;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="GotoExpression"/>s
    /// </summary>
    public class GotoExpressionSerializer : ISpecificExpressionSerializer {
        IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="GotoExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public GotoExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            GotoExpression gt = (GotoExpression)expression;
            json["value"] = serializer.Write(gt.Value);
            json["target"] = serializer.Write(gt.Target);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Goto(
                serializer.Read<LabelTarget>(json["target"]),
                serializer.Read<Expression>(json["value"])
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