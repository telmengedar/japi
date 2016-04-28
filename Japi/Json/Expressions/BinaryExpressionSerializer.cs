using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace GoorooMania.Japi.Json.Expressions {

    /// <summary>
    /// serializes binary expressions
    /// </summary>
    public class BinaryExpressionSerializer : ISpecificExpressionSerializer {

        /// <summary>
        /// serializes an expression to json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="expression"></param>
        public void Serialize(JsonObject json, Expression expression) {
            BinaryExpression e = (BinaryExpression)expression;
            json["left"] = JsonSerializer.Write(e.Left);
            json["right"] = JsonSerializer.Write(e.Right);
            json["method"] = JsonSerializer.Write(e.Method);
            json["conversion"] = JsonSerializer.Write(e.Conversion);
            json["isliftedtonull"] = new JsonValue(e.IsLiftedToNull);
        }

        /// <summary>
        /// deserializes an expression from json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public Expression Deserialize(JsonObject json) {
            return Expression.MakeBinary(
                json.SelectValue<ExpressionType>("type"),
                JsonSerializer.Read<Expression>(json["left"]),
                JsonSerializer.Read<Expression>(json["right"]),
                json.SelectValue<bool>("isliftedtonull"),
                JsonSerializer.Read<MethodInfo>(json["method"]),
                JsonSerializer.Read<LambdaExpression>(json["conversion"])
                );
        }

        /// <summary>
        /// get supported expression types
        /// </summary>
        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Add;
                yield return ExpressionType.AddChecked;
                yield return ExpressionType.Divide;
                yield return ExpressionType.Modulo;
                yield return ExpressionType.Multiply;
                yield return ExpressionType.MultiplyChecked;
                yield return ExpressionType.Power;
                yield return ExpressionType.Subtract;
                yield return ExpressionType.SubtractChecked;

                yield return ExpressionType.And;
                yield return ExpressionType.Or;
                yield return ExpressionType.ExclusiveOr;

                yield return ExpressionType.LeftShift;
                yield return ExpressionType.RightShift;

                yield return ExpressionType.AndAlso;
                yield return ExpressionType.OrElse;

                yield return ExpressionType.Equal;
                yield return ExpressionType.NotEqual;
                yield return ExpressionType.GreaterThanOrEqual;
                yield return ExpressionType.GreaterThan;
                yield return ExpressionType.LessThan;
                yield return ExpressionType.LessThanOrEqual;

                yield return ExpressionType.Coalesce;

                yield return ExpressionType.ArrayIndex;
            }
        }
    }
}