using System;
using System.IO;

namespace NightlyCode.Japi.Json {

    /// <summary>
    /// base operations for json data
    /// </summary>
    public static class JSON {
        static readonly IJsonSerializer serializer;
        static readonly IJsonWriter writer;

        static JSON() {
            serializer = new JsonSerializer();
            writer = new JsonWriter();
        }

        /// <summary>
        /// access to serializer
        /// </summary>
        public static IJsonSerializer Serializer => serializer;

        /// <summary>
        /// access to writer
        /// </summary>
        public static IJsonWriter Writer => writer;

        /// <summary>
        /// read data from a json stream
        /// </summary>
        /// <typeparam name="T">type of data to read</typeparam>
        /// <param name="stream">stream from which to read</param>
        /// <param name="leaveopen">whether to leave <see cref="Stream"/> open after reading</param>
        /// <returns>deserialized data</returns>
        public static T Read<T>(Stream stream, bool leaveopen=false) {
            return (T)Read(typeof(T), stream, leaveopen);
        }

        /// <summary>
        /// read data from json data
        /// </summary>
        /// <typeparam name="T">type of data to read</typeparam>
        /// <param name="data">stream from which to read</param>
        /// <returns>deserialized data</returns>
        public static T Read<T>(string data)
        {
            return (T)Read(typeof(T), data);
        }

        /// <summary>
        /// read an object from json bytes
        /// </summary>
        /// <typeparam name="T">type of object to read</typeparam>
        /// <param name="data">bytes which contain json data</param>
        /// <returns></returns>
        public static T Read<T>(byte[] data) {
            using(MemoryStream ms = new MemoryStream(data))
                return Read<T>(ms);
        }

#if FRAMEWORK35
        /// <summary>
        /// read data from a json stream
        /// </summary>
        /// <param name="type">type of data to read</param>
        /// <param name="stream">stream from which to read</param>
        /// <param name="leaveopen">whether to leave <see cref="Stream"/> open after reading</param>
        /// <returns>deserialized data</returns>
        public static object Read(Type type, Stream stream, bool leaveopen = false)
        {
            return serializer.Read(type, writer.Read(stream));
        }

        /// <summary>
        /// writes an object to a stream in json format
        /// </summary>
        /// <param name="object">object to write</param>
        /// <param name="stream">stream to write to</param>
        public static void Write(object @object, Stream stream)
        {
            writer.Write(serializer.Write(@object), stream);
        }

        /// <summary>
        /// writes a <see cref="JsonNode"/> to a stream
        /// </summary>
        /// <param name="node">node to be written</param>
        /// <param name="target">stream to write node into</param>
        public static void WriteNodeToStream(JsonNode node, Stream target)
        {
            writer.Write(node, target);
        }

        /// <summary>
        /// reads a json node from a stream
        /// </summary>
        /// <param name="stream">stream to read json node from</param>
        /// <returns></returns>
        public static JsonNode ReadNodeFromStream(Stream stream)
        {
            return writer.Read(stream);
        }
#else
        /// <summary>
        /// read data from a json stream
        /// </summary>
        /// <param name="type">type of data to read</param>
        /// <param name="stream">stream from which to read</param>
        /// <param name="leaveopen">whether to leave <see cref="Stream"/> open after reading</param>
        /// <returns>deserialized data</returns>
        public static object Read(Type type, Stream stream, bool leaveopen=false) {
            return serializer.Read(type, writer.Read(stream, leaveopen));
        }

        /// <summary>
        /// writes an object to a stream in json format
        /// </summary>
        /// <param name="object">object to write</param>
        /// <param name="stream">stream to write to</param>
        /// <param name="leaveopen">whether to leave <see cref="Stream"/> open after writing</param>
        public static void Write(object @object, Stream stream, bool leaveopen=false) {
            writer.Write(serializer.Write(@object), stream, leaveopen);
        }

        /// <summary>
        /// writes a <see cref="JsonNode"/> to a stream
        /// </summary>
        /// <param name="node">node to be written</param>
        /// <param name="target">stream to write node into</param>
        /// <param name="leaveopen">whether to leave <see cref="Stream"/> open after writing</param>
        public static void WriteNodeToStream(JsonNode node, Stream target, bool leaveopen=false) {
            writer.Write(node, target, leaveopen);
        }

        /// <summary>
        /// reads a json node from a stream
        /// </summary>
        /// <param name="stream">stream to read json node from</param>
        /// <param name="leaveopen">whether to leave <see cref="Stream"/> open after reading</param>
        /// <returns></returns>
        public static JsonNode ReadNodeFromStream(Stream stream, bool leaveopen=false) {
            return writer.Read(stream, leaveopen);
        }
#endif

        /// <summary>
        /// read data from json data
        /// </summary>
        /// <param name="type">type of data to read</param>
        /// <param name="data">data from which to read</param>
        /// <returns>deserialized data</returns>
        public static object Read(Type type, string data)
        {
            return serializer.Read(type, writer.Read(data));
        }


        /// <summary>
        /// writes an object to a stream in json format
        /// </summary>
        /// <param name="object">object to write</param>
        public static string WriteString(object @object) {
            return writer.WriteString(serializer.Write(@object));
        }

        /// <summary>
        /// writes a <see cref="JsonNode"/> to a string
        /// </summary>
        /// <param name="node">node to write</param>
        /// <returns>json string with node data</returns>
        public static string WriteNodeToString(JsonNode node) {
            return writer.WriteString(node);
        }

        /// <summary>
        /// reads a json node from a string
        /// </summary>
        /// <param name="data">string containing json data</param>
        /// <returns></returns>
        public static JsonNode ReadNodeFromString(string data) {
            return writer.Read(data);
        }
    }
}