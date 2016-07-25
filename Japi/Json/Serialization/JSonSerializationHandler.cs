namespace NightlyCode.Japi.Json.Serialization {

    /// <summary>
    /// base class for serialization handlers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class JSonSerializationHandler<T> : IJSonSerializationHandler {

        public JsonNode Serialize(object value) {
            return SerializeValue((T)value);
        }

        public object Deserialize(JsonNode json) {
            return DeserializeValue(json);
        }

        /// <summary>
        /// serializes a value to json
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract JsonNode SerializeValue(T value);

        /// <summary>
        /// deserializes a json structure to a value
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public abstract T DeserializeValue(JsonNode json);


    }
}