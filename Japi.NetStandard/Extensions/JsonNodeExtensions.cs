using System.Collections.Generic;
using NightlyCode.Japi.Extern;
using NightlyCode.Japi.Json;

namespace NightlyCode.Japi.Extensions {

    /// <summary>
    /// extensions for <see cref="JsonNode"/>s
    /// </summary>
    public static class JsonNodeExtensions {

        public static object SelectValue(this JsonNode node, string key) {
            if (node is JsonObject @object)
                return @object[key];
            throw new JsonException("Node doesn't support access by key");
        }

        public static T SelectValue<T>(this JsonNode node, string key) {
            return Converter.Convert<T>(SelectValue(node, key));
        }

        public static object SelectValue(this JsonNode node) {
            if (node is JsonValue value)
                return value.Value;
            throw new JsonException("Node doesn't support value access");
        }

        public static T SelectValue<T>(this JsonNode node) {
            return Converter.Convert<T>(SelectValue(node));
        }

        public static JsonNode SelectNode(this JsonNode node, string key) {
            if (node is JsonObject @object)
                return @object.GetNode(key);
            throw new JsonException("Node doesn't support access by key");
        }

        public static JsonNode SelectNode(this JsonNode node, int index) {
            if (node is JsonArray array)
                return array.GetNode(index);
            throw new JsonException("Node doesn't support access by index");
        }

        public static IEnumerable<JsonNode> SelectNodes(this JsonNode node, string key) {
            if (node is JsonObject @object) {
                JsonNode resultnode= @object.GetNode(key);
                if (resultnode is JsonArray array) {
                    foreach (JsonNode child in array.Nodes)
                        yield return child;
                }

                yield break;
            }

            throw new JsonException("Node doesn't support access by key");
        }
    }
}