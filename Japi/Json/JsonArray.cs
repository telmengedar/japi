using System.Collections.Generic;

namespace GoorooMania.Japi.Json {

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
        /// adds a node to the array
        /// </summary>
        /// <param name="node"></param>
        public void Add(JsonNode node) {
            nodes.Add(node);
        }

        public IEnumerable<JsonNode> Items => nodes;

        public int ItemCount => nodes.Count;

        protected override JsonNode GetNode(string key) {
            throw new JsonException("named access only valid for Json Objects");
        }

        protected override JsonNode GetNode(int index) {
            return nodes[index];
        }

        public override object Value { get { throw new JsonException("Value only valid for value nodes"); } }

        public override IEnumerator<JsonNode> GetEnumerator() {
            return nodes.GetEnumerator();
        }
    }
}