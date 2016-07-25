using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="MethodCallExpression"/>s
    /// </summary>
    public class MethodCallExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="MethodCallExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public MethodCallExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            MethodCallExpression call = (MethodCallExpression)expression;
            json["host"] = serializer.Write(call.Object);
            json["method"] = serializer.Write(call.Method);
            json["arguments"] = new JsonArray(call.Arguments.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Call(
                serializer.Read<Expression>(json["host"]),
                serializer.Read<MethodInfo>(json["method"]),
                json["arguments"].Select(serializer.Read<Expression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Call;
            }
        }
    }
}