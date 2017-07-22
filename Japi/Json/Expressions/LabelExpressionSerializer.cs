#if !FRAMEWORK35

using System.Collections.Generic;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="LabelExpression"/>s
    /// </summary>
    public class LabelExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="LabelExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public LabelExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            LabelExpression label = (LabelExpression)expression;
            json["target"] = serializer.Write(label.Target);
            json["default"] = serializer.Write(label.DefaultValue);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Label(
                serializer.Read<LabelTarget>(json["target"]),
                serializer.Read<Expression>(json["default"])
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

#endif