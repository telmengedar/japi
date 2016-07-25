using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="NewArrayExpression"/>s
    /// </summary>
    public class NewArrayExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="NewArrayExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public NewArrayExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            NewArrayExpression array = (NewArrayExpression)expression;
            json["expressions"] = new JsonArray(array.Expressions.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            switch(json.SelectValue<ExpressionType>("type")) {
                case ExpressionType.NewArrayBounds:
                    return Expression.NewArrayBounds(typeof(object), json["expressions"].Select(serializer.Read<Expression>));
                case ExpressionType.NewArrayInit:
                    return Expression.NewArrayInit(typeof(object), json["expressions"].Select(serializer.Read<Expression>));
                default:
                    throw new InvalidOperationException();
            }
            
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.NewArrayBounds;
                yield return ExpressionType.NewArrayInit;
            }
        }
    }
}