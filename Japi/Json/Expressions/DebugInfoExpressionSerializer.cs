using System.Collections.Generic;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serializes <see cref="DebugInfoExpression"/>s
    /// </summary>
    public class DebugInfoExpressionSerializer : ISpecificExpressionSerializer {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="DebugInfoExpressionSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public DebugInfoExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public void Serialize(JsonObject json, Expression expression) {
            DebugInfoExpression debuginfo = (DebugInfoExpression)expression;

            json["startline"] = new JsonValue(debuginfo.StartLine);
            json["startcolumn"] = new JsonValue(debuginfo.StartColumn);
            json["endline"] = new JsonValue(debuginfo.EndLine);
            json["endcolumn"] = new JsonValue(debuginfo.EndColumn);
            json["document"] = serializer.Write(debuginfo.Document);
            json["isclear"] = new JsonValue(debuginfo.IsClear);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.DebugInfo(
                serializer.Read<SymbolDocumentInfo>(json["document"] as JsonNode),
                json.SelectValue<int>("startline"),
                json.SelectValue<int>("startcolumn"),
                json.SelectValue<int>("endline"),
                json.SelectValue<int>("endcolumn")
                );
        }

        public IEnumerable<ExpressionType> Supported
        {
            get { yield return ExpressionType.DebugInfo; }
        }
    }
}