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
        public override object this[string key] {
            get {
                JsonNode node = lookup[key];
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

        /// <inheritdoc />
        public override object this[int index] {
            get => throw new JsonException("Indexed access only valid for arrays");
            set => throw new JsonException("Indexed access only valid for arrays");
        }

        /// <summary>
        /// determines whether the object contains a node with the specified key
        /// </summary>
        /// <param name="key">key to check for</param>
        /// <returns>true if object contains value with the specified key, false otherwise</returns>
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
        public object TryGetValue(string key) {
            lookup.TryGetValue(key, out JsonNode node);
            if (node is JsonValue value)
                return value.Value;
            return node;
        }

        /// <summary>
        /// get the node stored under the specified key
        /// </summary>
        /// <param name="key">key under which node is stored</param>
        /// <returns><see cref="JsonNode"/> stored under specified key</returns>
        public JsonNode GetNode(string key) {
            return lookup[key];
        }

        /// <inheritdoc />
        public override object Value => throw new JsonException("Value only valid for value nodes");

        /// <inheritdoc />
        public override string ToString() {
            return JSON.Writer.WriteString(this);
        }
    }
}