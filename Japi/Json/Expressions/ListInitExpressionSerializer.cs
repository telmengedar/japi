using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class ListInitExpressionSerializer : ISpecificExpressionSerializer {
        public void Serialize(JsonObject json, Expression expression) {
            ListInitExpression listinit = (ListInitExpression)expression;
            json["new"] = JsonSerializer.Write(listinit.NewExpression);
            json["initializers"] = new JsonArray(listinit.Initializers.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.ListInit(
                JsonSerializer.Read<NewExpression>(json["new"]),
                json["initializers"].Select(JsonSerializer.Read<ElementInit>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.ListInit;
            }
        }
    }
}