using System.Collections.Generic;

namespace NightlyCode.Japi.Json {

    /// <summary>
    /// json array containing json nodes
    /// </summary>
    public class JsonArray : JsonNode {
        readonly List<JsonNode> nodes = new List<JsonNode>();

        public JsonArray() {}

        public JsonArray(IEnumerable<JsonNode> nodes) {
            foreach(JsonNode node in nodes)
                Add(node);
        }

        /// <summary>
        /// index accessor
        /// </summary>
        /// <param name="index">index of value to get</param>
        /// <returns>node with the specified key</returns>
        public new object this[int index]
        {
            get
            {
                JsonNode node = GetNode(index);
                if (node is JsonValue value)
                    return value.Value;
                return node;
            }
            set
            {
                if (value is JsonNode node)
                    nodes[index] = node;
                else nodes[index] = new JsonValue(value);
            }
        }

        /// <summary>
        /// adds a node to the array
        /// </summary>
        /// <param name="node"></param>
        public void Add(JsonNode node) {
            nodes.Add(node);
        }

        public IEnumerable<JsonNode> Nodes => nodes;

        /// <summary>
        /// number of items in array
        /// </summary>
        public int Count => nodes.Count;

        protected override JsonNode GetNode(string key) {
            throw new JsonException("named access only valid for Json Objects");
        }

        protected override JsonNode GetNode(int index) {
            return nodes[index];
        }

        /// <inheritdoc />
        public override object Value { get { throw new JsonException("Value only valid for value nodes"); } }

        public override IEnumerator<JsonNode> GetEnumerator() {
            return nodes.GetEnumerator();
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"[{string.Join(",", nodes)}]";
        }
    }
}