using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Expressions {
    public class DebugInfoExpressionSerializer : ISpecificExpressionSerializer {
        public void Serialize(JsonObject json, Expression expression) {
            DebugInfoExpression debuginfo = (DebugInfoExpression)expression;

            json["startline"] = new JsonValue(debuginfo.StartLine);
            json["startcolumn"] = new JsonValue(debuginfo.StartColumn);
            json["endline"] = new JsonValue(debuginfo.EndLine);
            json["endcolumn"] = new JsonValue(debuginfo.EndColumn);
            json["document"] = JsonSerializer.Write(debuginfo.Document);
            json["isclear"] = new JsonValue(debuginfo.IsClear);
        }

        public Expression Deserialize(JsonObject json) {
            return Expression.DebugInfo(
                JsonSerializer.Read<SymbolDocumentInfo>(json["document"]),
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