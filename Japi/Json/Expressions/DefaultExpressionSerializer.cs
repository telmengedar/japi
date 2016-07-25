using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="DefaultExpression"/>s
    /// </summary>
    public class DefaultExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="DefaultExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public DefaultExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            DefaultExpression def = (DefaultExpression)expression;
            json["extype"] = serializer.Write(def.Type);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Default(serializer.Read<Type>(json["extype"]));
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Default;
            }
        }
    }
}