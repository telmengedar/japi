using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="InvocationExpression"/>s
    /// </summary>
    public class InvocationExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="InvocationExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public InvocationExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            InvocationExpression invocation = (InvocationExpression)expression;
            json["expression"] = serializer.Write(invocation.Expression);
            json["arguments"] = new JsonArray(invocation.Arguments.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Invoke(
                serializer.Read<Expression>(json["expression"]),
                json["arguments"].Select(serializer.Read<Expression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Invoke;
            }
        }
    }
}