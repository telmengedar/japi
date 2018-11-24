#if !FRAMEWORK35

using System;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="SymbolDocumentInfo"/>s
    /// </summary>
    public class SymbolDocumentInfoSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="SymbolDocumentInfoSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public SymbolDocumentInfoSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

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
                serializer.Read<Guid>(json["language"]),
                serializer.Read<Guid>(json["languagevendor"]),
                serializer.Read<Guid>(json["documenttype"])
                );
        }
    }
}

#endif