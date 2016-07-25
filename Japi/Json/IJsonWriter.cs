using System.IO;

namespace NightlyCode.Japi.Json {

    /// <summary>
    /// interface for a reader and writer of <see cref="JsonNode"/>s
    /// </summary>
    public interface IJsonWriter {
        /// <summary>
        /// reads json data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        JsonNode Read(string data);

        /// <summary>
        /// reads json data
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        JsonNode Read(Stream stream);

        /// <summary>
        /// writes a json node to string
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        string WriteString(JsonNode node);

        /// <summary>
        /// writes a json node to a stream
        /// </summary>
        /// <param name="node"></param>
        /// <param name="target"></param>
        void Write(JsonNode node, Stream target);
    }
}