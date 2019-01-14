using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NightlyCode.Japi.Extern;

namespace NightlyCode.Japi.Json
{

    /// <summary>
    /// node of a json document
    /// </summary>
    public abstract class JsonNode : IEnumerable<JsonNode> {

        /// <summary>
        /// indexer using a string as key used for <see cref="JsonObject"/>s
        /// </summary>
        /// <param name="key">key under which value is stored</param>
        /// <returns>value stored under specified key</returns>
        public abstract object this[string key] { get; set; }

        /// <summary>
        /// indexer using an int as key used for <see cref="JsonArray"/>s
        /// </summary>
        /// <param name="index">index to element</param>
        /// <returns>value at specified index</returns>
        public abstract object this[int index] { get; set; }

        /// <summary>
        /// value of node
        /// </summary>
        public abstract object Value { get; }

        /// <inheritdoc />
        public abstract IEnumerator<JsonNode> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// selects nodes using a path from a json node
        /// </summary>
        /// <param name="path">path to nodes to select</param>
        /// <returns>enumeration of nodes matching path</returns>
        public IEnumerable<object> Select(string path) {
            foreach(JsonNode node in Select(path.Split('/'), 0))
                if (node is JsonValue value)
                    yield return value.Value;
                else yield return node;
        }

        /// <summary>
        /// selects the first value which matches the specified path
        /// </summary>
        /// <param name="path">path to node to select</param>
        /// <returns>first value in json structure which matches the path</returns>
        public object SelectSingle(string path) {
            return Select(path).FirstOrDefault();
        }

        /// <summary>
        /// selects a value from the json structure
        /// </summary>
        /// <typeparam name="T">type of value to select</typeparam>
        /// <param name="path">path to node to select</param>
        /// <returns>typed value</returns>
        public T SelectSingle<T>(string path) {
            return Converter.Convert<T>(SelectSingle(path));
        }

        /// <summary>
        /// selects nodes using a path from a json node
        /// </summary>
        /// <param name="path">path to nodes to select</param>
        /// <returns>enumeration of nodes matching path</returns>
        public IEnumerable<T> Select<T>(string path) {
            return Select(path).Select(n => Converter.Convert<T>(n));
        }

        IEnumerable<object> Select(string[] path, int index) {
            if(this is JsonArray) {
                foreach(JsonNode node in this)
                    foreach(object child in node.Select(path, index))
                        yield return child;
            }
            else if(this is JsonObject @object) {
                object child = @object[path[index++]];
                if(index >= path.Length) {
                    if(child is JsonArray array)
                        foreach(JsonNode item in array)
                            yield return item;
                    else yield return child;
                }
                else {
                    if (!(child is JsonNode node))
                        yield break;

                    foreach(object value in node.Select(path, index))
                        yield return value;
                }
            }
            else if (this is JsonValue value)
                yield return value.Value;
        } 
    }
}