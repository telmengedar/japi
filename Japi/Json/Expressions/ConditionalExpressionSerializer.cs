using System.Collections.Generic;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="ConditionalExpression"/>s
    /// </summary>
    public class ConditionalExpressionSerializer : ISpecificExpressionSerializer {
        IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="ConditionalExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public ConditionalExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            ConditionalExpression conditional = (ConditionalExpression)expression;
            json["test"] = serializer.Write(conditional.Test);
            json["iftrue"] = serializer.Write(conditional.IfTrue);
            json["iffalse"] = serializer.Write(conditional.IfFalse);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Condition(
                serializer.Read<Expression>(json["test"]),
                serializer.Read<Expression>(json["iftrue"]),
                serializer.Read<Expression>(json["iffalse"])
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.Conditional; }
        }
    }
}