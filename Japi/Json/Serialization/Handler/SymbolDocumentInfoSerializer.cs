using System;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class SymbolDocumentInfoSerializer : IJSonSerializationHandler {

        public JsonNode Serialize(object value) {
            SymbolDocumentInfo document = (SymbolDocumentInfo)value;
            return new JsonObject
            {
                ["documenttype"] = new JsonValue(document.DocumentType.ToString()),
                ["filename"] = new JsonValue(document.FileName),
                ["language"] = new JsonValue(document.Language.ToString()),
                ["languagevendor"] = new JsonValue(document.LanguageVendor.ToString())
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.SymbolDocument(
                json.SelectValue<string>("filename"),
                JsonSerializer.Read<Guid>(json["language"]),
                JsonSerializer.Read<Guid>(json["languagevendor"]),
                JsonSerializer.Read<Guid>(json["documenttype"])
                );
        }
    }
}