namespace NightlyCode.Japi.Json.Serialization {

    /// <summary>
    /// interface for objects which support custom json serialization
    /// </summary>
    public interface ICustomJsonSerialization {

        /// <summary>
        /// serializes the object to json
        /// </summary>
        /// <returns></returns>
        JsonNode Serialize();

        /// <summary>
        /// deserializes the object from json
        /// </summary>
        /// <param name="node">node which contains json data</param>
        void Deserialize(JsonNode node);
    }
}