using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="RuntimeVariablesExpression"/>s
    /// </summary>
    public class RuntimeVariablesExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="RuntimeVariablesExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public RuntimeVariablesExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            RuntimeVariablesExpression rtv = (RuntimeVariablesExpression)expression;
            json["variables"] = new JsonArray(rtv.Variables.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.RuntimeVariables(
                json["variables"].Select(serializer.Read<ParameterExpression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.RuntimeVariables;
            }
        }
    }
}