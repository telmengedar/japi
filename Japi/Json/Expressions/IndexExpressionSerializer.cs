using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GoorooMania.Japi.Json.Expressions {
    public class IndexExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            IndexExpression index = (IndexExpression)expression;
            json["object"] = JsonSerializer.Write(index.Object);
            json["indexer"] = JsonSerializer.Write(index.Indexer);
            json["arguments"] = new JsonArray(index.Arguments.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.MakeIndex(
                JsonSerializer.Read<Expression>(json["object"]),
                JsonSerializer.Read<PropertyInfo>(json["indexer"]),
                json["arguments"].Select(JsonSerializer.Read<Expression>).ToArray()
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Index;
            }
        }
    }
}