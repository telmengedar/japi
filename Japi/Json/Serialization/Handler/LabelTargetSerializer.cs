#if !FRAMEWORK35

using System;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="LabelTarget"/>s
    /// </summary>
    public class LabelTargetSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="LabelTargetSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public LabelTargetSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            LabelTarget target = (LabelTarget)value;
            return new JsonObject {
                ["type"] = serializer.Write(target.Type),
                ["name"] = new JsonValue(target.Name)
            };
        }

        public object Deserialize(JsonNode json) {
            return Expression.Label(
                serializer.Read<Type>(json["type"]),
                json.SelectValue<string>("name")
                );
        }
    }
}

#endif