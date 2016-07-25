using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="MemberExpression"/>s
    /// </summary>
    public class MemberExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="MemberExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public MemberExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            MemberExpression member = (MemberExpression)expression;

            if(member.Expression.NodeType == ExpressionType.Parameter) {
                json["expressiontype"] = new JsonValue("parameter");
                json["expression"] = serializer.Write(member.Expression);
                json["member"] = serializer.Write(member.Member);
            }
            else {
                object host = GetHost(member.Expression);

                object value;
                if (member.Member is PropertyInfo)
                    value = ((PropertyInfo)member.Member).GetValue(host, null);
                else if (member.Member is FieldInfo)
                    value = ((FieldInfo)member.Member).GetValue(host);
                else throw new NotImplementedException();

                json["value"] = serializer.Write(value);
                if (value != null)
                    json["valuetype"] = serializer.Write(value.GetType());
            }
        }

        public Expression Deserialize(JsonObject json) {
            if(json.SelectValue<string>("expressiontype") == "parameter") {
                return Expression.MakeMemberAccess(
                    serializer.Read<Expression>(json["expression"]),
                    serializer.Read<MemberInfo>(json["member"])
                    );
            }

            if (json.ContainsKey("valuetype"))
            {
                Type type = serializer.Read<Type>(json["valuetype"]);
                return Expression.Constant(serializer.Read(type, json["value"]));
            }
            return Expression.Constant(null);
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.MemberAccess; }
        }

        object GetHost(Expression expression)
        {
            if (expression == null)
                return null;

            if (expression is ConstantExpression)
                return ((ConstantExpression)expression).Value;
            if (expression is MemberExpression)
            {
                MemberExpression memberexpression = (MemberExpression)expression;
                object host = GetHost(memberexpression.Expression);
                if (memberexpression.Member is PropertyInfo)
                    return ((PropertyInfo)memberexpression.Member).GetValue(host, null);
                if (memberexpression.Member is FieldInfo)
                    return ((FieldInfo)memberexpression.Member).GetValue(host);
                throw new NotImplementedException();
            }
            if (expression is LambdaExpression)
                return expression;
            if (expression is UnaryExpression)
                return ((UnaryExpression)expression).Operand;
            if (expression is MethodCallExpression)
            {
                MethodCallExpression methodcall = (MethodCallExpression)expression;
                return methodcall.Method.Invoke(GetHost(methodcall.Object), methodcall.Arguments.Select(GetHost).ToArray());
            }
            throw new NotImplementedException();
        }

    }
}