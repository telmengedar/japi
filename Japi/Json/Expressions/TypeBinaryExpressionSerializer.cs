using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class TypeBinaryExpressionSerializer : ISpecificExpressionSerializer{
        public void Serialize(JsonObject json, Expression expression) {
            TypeBinaryExpression type = (TypeBinaryExpression)expression;
            json["expression"] = JsonSerializer.Write(type.Expression);
            json["operand"] = JsonSerializer.Write(type.TypeOperand);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.TypeIs(
                JsonSerializer.Read<Expression>(json["expression"]),
                JsonSerializer.Read<Type>(json["operand"])
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.TypeIs;
            }
        }
    }
}