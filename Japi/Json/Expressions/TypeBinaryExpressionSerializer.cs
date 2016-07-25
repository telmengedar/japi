using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="TypeBinaryExpression"/>s
    /// </summary>
    public class TypeBinaryExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="TypeBinaryExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public TypeBinaryExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            TypeBinaryExpression type = (TypeBinaryExpression)expression;
            json["expression"] = serializer.Write(type.Expression);
            json["operand"] = serializer.Write(type.TypeOperand);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.TypeIs(
                serializer.Read<Expression>(json["expression"]),
                serializer.Read<Type>(json["operand"])
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