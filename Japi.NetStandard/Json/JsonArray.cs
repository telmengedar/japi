using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NightlyCode.Japi.Json {

    /// <summary>
    /// json array containing json nodes
    /// </summary>
    public class JsonArray : JsonNode, IEnumerable<JsonNode> {
        readonly List<JsonNode> nodes = new List<JsonNode>();

        /// <summary>
        /// creates a new <see cref="JsonArray"/>
        /// </summary>
        public JsonArray() {}

        /// <summary>
        /// creates a new <see cref="JsonArray"/> using an enumeration of values to contain
        /// </summary>
        /// <param name="values">values for array to contain</param>
        public JsonArray(IEnumerable<object> values) {
            foreach (object value in values)
                if (value is JsonNode node)
                    Add(node);
                else
                    Add(new JsonValue(value));
        }

        /// <summary>
        /// index accessor
        /// </summary>
        /// <param name="index">index of value to get</param>
        /// <returns>node with the specified key</returns>
        public override object this[int index]
        {
            get
            {
                JsonNode node = nodes[index];
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

        public override object this[string key]
        {
            get => throw new JsonException("named access only valid for Json Objects");
            set => throw new JsonException("named access only valid for Json Objects");
        }

        /// <summary>
        /// get node at specified index
        /// </summary>
        /// <param name="index">index where node is stored</param>
        /// <returns>node at specified index</returns>
        public JsonNode GetNode(int index) {
            return nodes[index];
        }

        /// <summary>
        /// adds a value to the array
        /// </summary>
        /// <param name="value">value to add to the array</param>
        public void Add(object value) {
            if (value is JsonNode node)
                nodes.Add(node);
            else nodes.Add(new JsonValue(value));
        }

        /// <summary>
        /// nodes in json array
        /// </summary>
        public IEnumerable<JsonNode> Nodes => nodes;

        /// <summary>
        /// values in json array
        /// </summary>
        public IEnumerable<object> Values => Nodes.Select(n => (n is JsonValue value) ? value.Value : n);

        /// <summary>
        /// number of items in array
        /// </summary>
        public int Count => nodes.Count;

        /// <inheritdoc />
        public override object Value => throw new JsonException("Value only valid for value nodes");

        /// <inheritdoc />
        public IEnumerator<JsonNode> GetEnumerator() {
            return nodes.GetEnumerator();
        }

        /// <inheritdoc />
        public override string ToString() {
            return JSON.Writer.WriteString(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}