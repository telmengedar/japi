using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="UnaryExpression"/>s
    /// </summary>
    public class UnaryExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="UnaryExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public UnaryExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            UnaryExpression unary = (UnaryExpression)expression;
            json["operand"] = serializer.Write(unary.Operand);
            json["unarytype"] = serializer.Write(unary.Type);
            json["method"] = serializer.Write(unary.Method);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.MakeUnary(
                json.SelectValue<ExpressionType>("type"),
                serializer.Read<Expression>(json["operand"]),
                serializer.Read<Type>(json["unarytype"]),
                serializer.Read<MethodInfo>(json["method"])
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.ArrayLength;
                yield return ExpressionType.Convert;
                yield return ExpressionType.ConvertChecked;
                yield return ExpressionType.Negate;
                yield return ExpressionType.NegateChecked;
                yield return ExpressionType.Not;
                yield return ExpressionType.Quote;
                yield return ExpressionType.TypeAs;
                yield return ExpressionType.UnaryPlus;
            }
        }
    }
}