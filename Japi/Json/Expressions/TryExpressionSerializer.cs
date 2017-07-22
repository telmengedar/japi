#if !FRAMEWORK35

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="TryExpression"/>s
    /// </summary>
    public class TryExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="TryExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public TryExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            TryExpression @try = (TryExpression)expression;
            json["trytype"] = serializer.Write(@try.Type);
            json["body"] = serializer.Write(@try.Body);
            json["finally"] = serializer.Write(@try.Finally);
            json["fault"] = serializer.Write(@try.Fault);
            json["handlers"] = new JsonArray(@try.Handlers.Select(serializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.MakeTry(
                serializer.Read<Type>(json["trytype"]),
                serializer.Read<Expression>(json["body"]),
                serializer.Read<Expression>(json["finally"]),
                serializer.Read<Expression>(json["fault"]),
                json["handlers"].Select(serializer.Read<CatchBlock>)
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get
            {
                yield return ExpressionType.Try;
            }
        }
    }
}

#endif