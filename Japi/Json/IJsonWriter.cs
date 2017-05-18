using System.IO;

namespace NightlyCode.Japi.Json {

    /// <summary>
    /// interface for a reader and writer of <see cref="JsonNode"/>s
    /// </summary>
    public interface IJsonWriter {

        /// <summary>
        /// reads json data
        /// </summary>
        /// <param name="data">string which contains a json structure</param>
        /// <returns><see cref="JsonNode"/> containing the read json data</returns>
        JsonNode Read(string data);

        /// <summary>
        /// reads json data
        /// </summary>
        /// <param name="data">data which contains a json structure</param>
        /// <returns><see cref="JsonNode"/> containing the read json data</returns>
        JsonNode Read(byte[] data);

#if UNITY
        /// <summary>
        /// reads json data
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        JsonNode Read(Stream stream);
#else
        /// <summary>
        /// reads json data
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="leaveopen">whether to leave <see cref="Stream"/> open after reading</param>
        /// <returns></returns>
        JsonNode Read(Stream stream, bool leaveopen=false);
#endif

        /// <summary>
        /// writes a json node to string
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        string WriteString(JsonNode node);

#if UNITY
        /// <summary>
        /// writes a json node to a stream
        /// </summary>
        /// <param name="node"></param>
        /// <param name="target"></param>
        void Write(JsonNode node, Stream target);
#else
        /// <summary>
        /// writes a json node to a stream
        /// </summary>
        /// <param name="node"></param>
        /// <param name="target"></param>
        /// <param name="leaveopen">whether to leave <see cref="Stream"/> open after writing</param>
        void Write(JsonNode node, Stream target, bool leaveopen);
#endif
    }
}