using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="MemberInitExpression"/>s
    /// </summary>
    public class MemberInitExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="MemberExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public MemberInitExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            MemberInitExpression init = (MemberInitExpression)expression;

            json["new"] = serializer.Write(init.NewExpression);
            json["bindings"] = new JsonArray(init.Bindings.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.MemberInit(
                serializer.Read<NewExpression>(json["new"]),
                json["bindings"].Select(serializer.Read<MemberBinding>).ToArray()
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