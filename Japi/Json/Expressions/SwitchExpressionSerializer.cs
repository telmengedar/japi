using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="SwitchExpression"/>s
    /// </summary>
    public class SwitchExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="SwitchExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public SwitchExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            SwitchExpression @switch = (SwitchExpression)expression;
            json["value"] = serializer.Write(@switch.SwitchValue);
            json["default"] = serializer.Write(@switch.DefaultBody);
            json["comparison"] = serializer.Write(@switch.Comparison);
            json["cases"] = new JsonArray(@switch.Cases.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Switch(
                serializer.Read<Expression>(json["value"]),
                serializer.Read<Expression>(json["default"]),
                serializer.Read<MethodInfo>(json["comparison"]),
                json["cases"].Select(serializer.Read<SwitchCase>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Switch;
            }
        }
    }
}