using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoorooMania.Japi.Extern;

namespace GoorooMania.Japi.Json
{

    /// <summary>
    /// node of a json document
    /// </summary>
    public abstract class JsonNode : IEnumerable<JsonNode> {

        public JsonNode this[string key] => GetNode(key);

        public JsonNode this[int index] => GetNode(index);

        protected abstract JsonNode GetNode(string key);

        protected abstract JsonNode GetNode(int index);

        public abstract object Value { get; }

        public abstract IEnumerator<JsonNode> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerable<JsonNode> Select(string path) {
            foreach(JsonNode node in Select(path.Split('/'), 0))
                yield return node;
        }

        public JsonNode SelectSingle(string path) {
            return Select(path).FirstOrDefault();
        } 

        public object SelectValue(string path) {
            JsonValue value = SelectSingle(path) as JsonValue;
            return value?.Value;
        }

        /// <summary>
        /// selects a value from the json structure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T SelectValue<T>(string path) {
            return Converter.Convert<T>(SelectValue(path));
        }

        public IEnumerable<JsonNode> Select(string[] path, int index) {
            if(this is JsonArray) {
                foreach(JsonNode node in this)
                    foreach(JsonNode child in node.Select(path, index))
                        yield return child;
            }
            else if(this is JsonObject) {
                JsonNode child = ((JsonObject)this).TryGetNode(path[index++]);
                if(index >= path.Length) {
                    if(child is JsonArray)
                        foreach(JsonNode item in child)
                            yield return item;
                    else yield return child;
                }
                else {
                    foreach(JsonNode node in child.Select(path, index))
                        yield return node;
                }
            }
        } 
    }
}