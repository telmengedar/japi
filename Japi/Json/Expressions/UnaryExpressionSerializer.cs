using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace GoorooMania.Japi.Json.Expressions {
    public class UnaryExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            UnaryExpression unary = (UnaryExpression)expression;
            json["operand"] = JsonSerializer.Write(unary.Operand);
            json["unarytype"] = JsonSerializer.Write(unary.Type);
            json["method"] = JsonSerializer.Write(unary.Method);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.MakeUnary(
                json.SelectValue<ExpressionType>("type"),
                JsonSerializer.Read<Expression>(json["operand"]),
                JsonSerializer.Read<Type>(json["unarytype"]),
                JsonSerializer.Read<MethodInfo>(json["method"])
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