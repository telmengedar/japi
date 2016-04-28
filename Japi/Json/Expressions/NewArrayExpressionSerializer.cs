using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class NewArrayExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            NewArrayExpression array = (NewArrayExpression)expression;
            json["expressions"] = new JsonArray(array.Expressions.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            switch(json.SelectValue<ExpressionType>("type")) {
                case ExpressionType.NewArrayBounds:
                    return Expression.NewArrayBounds(typeof(object), json["expressions"].Select(JsonSerializer.Read<Expression>));
                case ExpressionType.NewArrayInit:
                    return Expression.NewArrayInit(typeof(object), json["expressions"].Select(JsonSerializer.Read<Expression>));
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