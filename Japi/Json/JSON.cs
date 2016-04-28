using System;
using System.IO;

namespace GoorooMania.Japi.Json {

    /// <summary>
    /// base operations for json data
    /// </summary>
    public static class JSON {

        /// <summary>
        /// read data from a json stream
        /// </summary>
        /// <typeparam name="T">type of data to read</typeparam>
        /// <param name="stream">stream from which to read</param>
        /// <returns>deserialized data</returns>
        public static T Read<T>(Stream stream) {
            return (T)Read(typeof(T), stream);
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
        /// read data from a json stream
        /// </summary>
        /// <param name="type">type of data to read</param>
        /// <param name="stream">stream from which to read</param>
        /// <returns>deserialized data</returns>
        public static object Read(Type type, Stream stream) {
            return JsonSerializer.Read(type, JsonWriter.Read(stream));
        }

        /// <summary>
        /// read data from json data
        /// </summary>
        /// <param name="type">type of data to read</param>
        /// <param name="data">data from which to read</param>
        /// <returns>deserialized data</returns>
        public static object Read(Type type, string data)
        {
            return JsonSerializer.Read(type, JsonWriter.Read(data));
        }

        /// <summary>
        /// writes an object to a stream in json format
        /// </summary>
        /// <param name="object">object to write</param>
        /// <param name="stream">stream to write to</param>
        public static void Write(object @object, Stream stream) {
            JsonWriter.Write(JsonSerializer.Write(@object), stream);
        }

        /// <summary>
        /// writes an object to a stream in json format
        /// </summary>
        /// <param name="object">object to write</param>
        public static string WriteString(object @object) {
            return JsonWriter.WriteString(JsonSerializer.Write(@object));
        }
    }
}