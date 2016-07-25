using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="ListInitExpression"/>s
    /// </summary>
    public class ListInitExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="ListInitExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public ListInitExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            ListInitExpression listinit = (ListInitExpression)expression;
            json["new"] = serializer.Write(listinit.NewExpression);
            json["initializers"] = new JsonArray(listinit.Initializers.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.ListInit(
                serializer.Read<NewExpression>(json["new"]),
                json["initializers"].Select(serializer.Read<ElementInit>)
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