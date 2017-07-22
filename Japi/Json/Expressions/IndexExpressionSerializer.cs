#if !FRAMEWORK35

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="IndexExpression"/>s
    /// </summary>
    public class IndexExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="IndexExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public IndexExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            IndexExpression index = (IndexExpression)expression;
            json["object"] = serializer.Write(index.Object);
            json["indexer"] = serializer.Write(index.Indexer);
            json["arguments"] = new JsonArray(index.Arguments.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.MakeIndex(
                serializer.Read<Expression>(json["object"]),
                serializer.Read<PropertyInfo>(json["indexer"]),
                json["arguments"].Select(serializer.Read<Expression>).ToArray()
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

#endif