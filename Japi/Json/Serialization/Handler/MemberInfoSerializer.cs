using System;
using System.Reflection;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class MemberInfoSerializer : IJSonSerializationHandler {
        public JsonNode Serialize(object value) {
            throw new NotImplementedException("Use specific serializer");
        }

        public object Deserialize(JsonNode json) {
            switch(json.SelectValue<string>("type")) {
                case "field":
                    return JsonSerializer.Read<FieldInfo>(json);
                case "property":
                    return JsonSerializer.Read<PropertyInfo>(json);
                case "method":
                    return JsonSerializer.Read<MethodInfo>(json);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}