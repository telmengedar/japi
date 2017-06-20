using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="DynamicExpression"/>s
    /// </summary>
    public class DynamicExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="DynamicExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public DynamicExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            DynamicExpression dyn = (DynamicExpression)expression;
            json["binder"] = serializer.Write(dyn.Binder);
            json["arguments"] = new JsonArray(dyn.Arguments.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Dynamic(
                serializer.Read<CallSiteBinder>(json["binder"] as JsonNode),
                typeof(object),
                json["arguments"].Select(serializer.Read<Expression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Dynamic;
            }
        }
    }
}