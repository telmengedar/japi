#if !FRAMEWORK35

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {


    /// <summary>
    /// serializes <see cref="BlockExpression"/>s
    /// </summary>
    public class BlockExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="BlockExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public BlockExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            BlockExpression block = (BlockExpression)expression;
            json["expressions"] = new JsonArray(block.Expressions.Select(serializer.Write));
            json["variables"] = new JsonArray(block.Variables.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Block(
                json["variables"].Select(serializer.Read<ParameterExpression>),
                json["expressions"].Select(serializer.Read<Expression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.Block; }
        }
    }
}

#endif