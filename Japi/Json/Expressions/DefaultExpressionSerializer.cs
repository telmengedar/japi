using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class DefaultExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            DefaultExpression def = (DefaultExpression)expression;
            json["extype"] = JsonSerializer.Write(def.Type);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Default(JsonSerializer.Read<Type>(json["extype"]));
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Default;
            }
        }
    }
}