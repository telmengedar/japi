using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class ParameterExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            ParameterExpression parameter = (ParameterExpression)expression;
            json["paramtype"] = JsonSerializer.Write(parameter.Type);
            json["name"] = JsonSerializer.Write(parameter.Name);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Parameter(
                JsonSerializer.Read<Type>(json["paramtype"]),
                json.SelectValue<string>("name")
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.Parameter; }
        }
    }
}