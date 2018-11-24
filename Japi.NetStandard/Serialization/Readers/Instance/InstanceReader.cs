using System;
using System.IO;
using NightlyCode.Japi.Serialization.Data;
using NightlyCode.Japi.Serialization.Transformation;

namespace NightlyCode.Japi.Serialization.Readers.Instance {

    /// <summary>
    /// reads instances from java structure data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InstanceReader<T> {
        readonly Stream basestream;
        readonly ObjectStream stream;
        readonly StructureReducer reducer;

        /// <summary>
        /// creates a new instance reader
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="types"></param>
        public InstanceReader(Stream stream, params Type[] types) {
            basestream = stream;
            this.stream = new ObjectStream(stream);
            reducer = new StructureReducer(new InstanceTransformationTable(types));
        }

        /// <summary>
        /// determines whether there is more serialized data available
        /// </summary>
        /// <remarks>
        /// only available when stream supports length and position, else stream will throw exception here
        /// </remarks>
        public bool ContainsData => basestream.Position < basestream.Length;

        /// <summary>
        /// reads an object
        /// </summary>
        /// <returns></returns>
        public T Read()
        {
            IJavaData data = stream.ReadObject();
            if (reducer != null && data is JavaObject)
                data = reducer.Reduce((JavaObject)data);

            JavaValue value = data as JavaValue;
            if(value == null)
                throw new InvalidOperationException("Stream contains no object of the specified type");
            return value.Get<T>();
        }

    }
}