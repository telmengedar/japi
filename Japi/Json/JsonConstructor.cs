namespace NightlyCode.Japi.Json {

    /// <summary>
    /// json object for easy object creation using indexer
    /// </summary>
    public class JsonConstructor : JsonObject {

        /// <summary>
        /// index accessor
        /// </summary>
        /// <param name="key">key of the node to access</param>
        /// <returns>node with the specified key</returns>
        public new object this[string key]
        {
            get { return GetNode(key); }
            set
            {
                if (value is JsonNode)
                    base[key] = (JsonNode)value;
                else
                    base[key] = JSON.Serializer.Write(value);
            }
        }
    }
}