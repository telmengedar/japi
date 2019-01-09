using System.Collections.Generic;

namespace NightlyCode.Japi.Json
{

    /// <summary>
    /// object containing json data
    /// </summary>
    public class JsonObject : JsonNode {
        readonly Dictionary<string, JsonNode> lookup = new Dictionary<string, JsonNode>();

        /// <summary>
        /// index accessor
        /// </summary>
        /// <param name="key">key of the node to access</param>
        /// <returns>node with the specified key</returns>
        public new object this[string key] {
            get {
                JsonNode node = GetNode(key);
                if (node is JsonValue value)
                    return value.Value;
                return node;
            }
            set {
                if (value is JsonNode node)
                    lookup[key] = node;
                else lookup[key] = new JsonValue(value);
            }
        }

        /// <summary>
        /// determines whether the object contains a node with the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key) {
            return lookup.ContainsKey(key);
        }

        /// <summary>
        /// removes a node from the object
        /// </summary>
        /// <param name="key">key to the object</param>
        public void Remove(string key) {
            lookup.Remove(key);
        }

        /// <summary>
        /// keys in object
        /// </summary>
        public IEnumerable<string> Keys => lookup.Keys;

        /// <summary>
        /// tries to get a node from the object
        /// </summary>
        /// <param name="key">key of the object to get</param>
        /// <returns>a <see cref="JsonNode"/> when something is found using the key, null otherwise</returns>
        public JsonNode TryGetNode(string key) {
            lookup.TryGetValue(key, out JsonNode node);
            return node;
        }

        protected override JsonNode GetNode(string key) {
            return lookup[key];
        }

        /// <summary>
        /// always throws an exception since a jsonobject has no <see cref="int"/> indexer
        /// </summary>
        /// <param name="index">index to access</param>
        /// <returns>this method always throws an exception</returns>
        protected override JsonNode GetNode(int index) {
            throw new JsonException("Indexed access only valid for arrays");
        }

        public override object Value => throw new JsonException("Value only valid for value nodes");

        public override IEnumerator<JsonNode> GetEnumerator() {
            throw new JsonException("Enumerator only available for arrays");
        }

        /// <inheritdoc />
        public override string ToString() {
            return JSON.Writer.WriteString(this);
        }
    }
}