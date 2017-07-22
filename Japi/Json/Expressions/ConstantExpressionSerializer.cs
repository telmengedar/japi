using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NightlyCode.Japi.Extern;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="ConstantExpression"/>s
    /// </summary>
    public class ConstantExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;
        // TODO: implement complex values

        /// <summary>
        /// creates a new <see cref="ConstantExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public ConstantExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            ConstantExpression constant = (ConstantExpression)expression;
            json["value"] = new JsonValue(constant.Value);
            if(constant.Value != null)
                json["valuetype"] = serializer.Write(constant.Value.GetType());
        }

        public Expression Deserialize(JsonObject json) {
            object value = json.SelectValue<object>("value");
            if(value != null) {
                Type valuetype = serializer.Read<Type>(json["valuetype"] as JsonNode);
                return Expression.Constant(Converter.Convert(value, valuetype), valuetype);
            }
            return Expression.Constant(null);
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.Constant; }
        }
    }
}