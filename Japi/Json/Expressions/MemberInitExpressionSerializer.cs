using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class MemberInitExpressionSerializer : ISpecificExpressionSerializer {
        public void Serialize(JsonObject json, Expression expression) {
            MemberInitExpression init = (MemberInitExpression)expression;

            json["new"] = JsonSerializer.Write(init.NewExpression);
            json["bindings"] = new JsonArray(init.Bindings.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.MemberInit(
                JsonSerializer.Read<NewExpression>(json["new"]),
                json["bindings"].Select(JsonSerializer.Read<MemberBinding>).ToArray()
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.MemberInit;
            }
        }
    }
}