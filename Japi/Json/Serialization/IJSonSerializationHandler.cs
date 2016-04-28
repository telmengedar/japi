namespace GoorooMania.Japi.Json.Serialization {

    /// <summary>
    /// serialization handler for custom types
    /// </summary>
    public interface IJSonSerializationHandler {

        /// <summary>
        /// serializes a value to json
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        JsonNode Serialize(object value);

        /// <summary>
        /// deserializes a value from json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        object Deserialize(JsonNode json);
    }
}