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
        /// <param name="key"></param>
        /// <returns></returns>
        public new JsonNode this[string key]
        {
            get { return GetNode(key); }
            set { lookup[key] = value; }
        }

        public bool ContainsKey(string key) {
            return lookup.ContainsKey(key);
        }

        public IEnumerable<string> Keys => lookup.Keys;

        public JsonNode TryGetNode(string key) {
            JsonNode node;
            lookup.TryGetValue(key, out node);
            return node;
        }

        protected override JsonNode GetNode(string key) {
            return lookup[key];
        }

        protected override JsonNode GetNode(int index) {
            throw new JsonException("Indexed access only valid for arrays");
        }

        public override object Value { get { throw new JsonException("Value only valid for value nodes"); } }

        public override IEnumerator<JsonNode> GetEnumerator() {
            throw new JsonException("Enumerator only available for arrays");
        }
    }
}