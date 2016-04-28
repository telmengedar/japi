using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace GoorooMania.Japi.Json.Expressions {
    public class DynamicExpressionSerializer : ISpecificExpressionSerializer{
        public void Serialize(JsonObject json, Expression expression) {
            DynamicExpression dyn = (DynamicExpression)expression;
            json["binder"] = JsonSerializer.Write(dyn.Binder);
            json["arguments"] = new JsonArray(dyn.Arguments.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Dynamic(
                JsonSerializer.Read<CallSiteBinder>(json["binder"]),
                typeof(object),
                json["arguments"].Select(JsonSerializer.Read<Expression>)
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