using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="ParameterExpression"/>s
    /// </summary>
    public class ParameterExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="ParameterExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public ParameterExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            ParameterExpression parameter = (ParameterExpression)expression;
            json["paramtype"] = serializer.Write(parameter.Type);
            json["name"] = serializer.Write(parameter.Name);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.Parameter(
                serializer.Read<Type>(json["paramtype"]),
                json.SelectValue<string>("name")
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.Parameter; }
        }
    }
}