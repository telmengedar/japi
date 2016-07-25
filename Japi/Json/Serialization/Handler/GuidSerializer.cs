using System;

namespace NightlyCode.Japi.Json.Serialization.Handler {
    public class GuidSerializer : IJSonSerializationHandler {
        public JsonNode Serialize(object value) {
            return new JsonValue(((Guid)value).ToString());
        }

        public object Deserialize(JsonNode json) {
            return new Guid((string)json.Value);
        }
    }
}