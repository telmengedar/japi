using System;

namespace NightlyCode.Japi.Json {

    /// <summary>
    /// interface for a json serializer
    /// </summary>
    public interface IJsonSerializer {

        /// <summary>
        /// reads a type from a json node
        /// </summary>
        /// <typeparam name="T">type to read</typeparam>
        /// <param name="node">json node</param>
        /// <returns>instance with data from json node</returns>
        T Read<T>(JsonNode node);

        /// <summary>
        /// reads a type from a json node
        /// </summary>
        /// <param name="type">type to read</param>
        /// <param name="node">json node</param>
        /// <returns>instance with data from json node</returns>
        object Read(Type type, JsonNode node);

        /// <summary>
        /// writes an object to a json structure
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        JsonNode Write(object @object);
    }
}