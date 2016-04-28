using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GoorooMania.Japi.Json.Expressions {
    public class NewExpressionSerializer : ISpecificExpressionSerializer {
        public void Serialize(JsonObject json, Expression expression) {
            NewExpression n = (NewExpression)expression;
            json["constructor"] = JsonSerializer.Write(n.Constructor);
            json["arguments"] = new JsonArray(n.Arguments.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.New(
                JsonSerializer.Read<ConstructorInfo>(json["constructor"]),
                json["arguments"].Select(JsonSerializer.Read<Expression>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.New;
            }
        }
    }
}