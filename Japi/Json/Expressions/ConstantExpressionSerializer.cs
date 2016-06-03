using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GoorooMania.Japi.Extern;

namespace GoorooMania.Japi.Json.Expressions {

    /// <summary>
    /// serializer for constant expressions
    /// </summary>
    public class ConstantExpressionSerializer : ISpecificExpressionSerializer {
        // TODO: implement complex values

        public void Serialize(JsonObject json, Expression expression) {
            ConstantExpression constant = (ConstantExpression)expression;
            json["value"] = new JsonValue(constant.Value);
            if(constant.Value != null)
                json["valuetype"] = JsonSerializer.Write(constant.Value.GetType());
        }

        public Expression Deserialize(JsonObject json) {
            object value = json.SelectValue<object>("value");
            if(value != null) {
                Type valuetype = JsonSerializer.Read<Type>(json["valuetype"]);
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