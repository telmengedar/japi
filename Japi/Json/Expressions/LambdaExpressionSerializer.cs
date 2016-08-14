using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="LambdaExpression"/>s
    /// </summary>
    public class LambdaExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="LambdaExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public LambdaExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            LambdaExpression lamda = (LambdaExpression)expression;

            json["parameters"] = new JsonArray(lamda.Parameters.Select(serializer.Write));
            json["body"] = serializer.Write(lamda.Body);
            //json["return"] = JsonSerializer.Write(lamda.ReturnType);
#if !UNITY
            json["name"] = new JsonValue(lamda.Name);
            json["tail"] = new JsonValue(lamda.TailCall);
#endif
        }

        public Expression Deserialize(JsonObject json) {
#if UNITY
            return Expression.Lambda(
                serializer.Read<Expression>(json["body"]),
                json["parameters"].Select(serializer.Read<ParameterExpression>).ToArray()
            );
#else
            return Expression.Lambda(
                serializer.Read<Expression>(json["body"]),
                json.SelectValue<string>("name"),
                json.SelectValue<bool>("tail"),
                json["parameters"].Select(serializer.Read<ParameterExpression>)
                );
#endif
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.Lambda; }
        }
    }
}