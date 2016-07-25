using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="NewExpression"/>s
    /// </summary>
    public class NewExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="NewExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public NewExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            NewExpression n = (NewExpression)expression;
            json["constructor"] = serializer.Write(n.Constructor);
            json["arguments"] = new JsonArray(n.Arguments.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.New(
                serializer.Read<ConstructorInfo>(json["constructor"]),
                json["arguments"].Select(serializer.Read<Expression>)
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