using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class TryExpressionSerializer : ISpecificExpressionSerializer {

        public void Serialize(JsonObject json, Expression expression) {
            TryExpression @try = (TryExpression)expression;
            json["trytype"] = JsonSerializer.Write(@try.Type);
            json["body"] = JsonSerializer.Write(@try.Body);
            json["finally"] = JsonSerializer.Write(@try.Finally);
            json["fault"] = JsonSerializer.Write(@try.Fault);
            json["handlers"] = new JsonArray(@try.Handlers.Select(JsonSerializer.Write));
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.MakeTry(
                JsonSerializer.Read<Type>(json["trytype"]),
                JsonSerializer.Read<Expression>(json["body"]),
                JsonSerializer.Read<Expression>(json["finally"]),
                JsonSerializer.Read<Expression>(json["fault"]),
                json["handlers"].Select(JsonSerializer.Read<CatchBlock>)
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