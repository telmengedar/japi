using System.IO;
using NightlyCode.Japi.Serialization.Conversion;
using NightlyCode.Japi.Serialization.Data;
using NightlyCode.Japi.Serialization.Transformation;

namespace NightlyCode.Japi.Serialization.Readers {

    /// <summary>
    /// reads objects from object streams
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectReader<T> {
        readonly Stream basestream;
        readonly ObjectStream stream;
        readonly StructureReducer reducer;
        readonly IDataConverter<T> converter;

        /// <summary>
        /// creates a new xml object reader
        /// </summary>
        /// <param name="stream">source stream of serialized java data</param>
        /// <param name="converter">converter used to convert data to target representation</param>
        /// <param name="reducer">reduces java data structures to a more compact format (optional but necessary for specific converters to work)</param>
        public ObjectReader(Stream stream, IDataConverter<T> converter, StructureReducer reducer = null) {
            basestream = stream;
            this.stream = new ObjectStream(stream);
            this.converter = converter;
            this.reducer = reducer;
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
        public T Read() {
            IJavaData data = stream.ReadObject();
            if(reducer != null && data is JavaObject)
                data = reducer.Reduce((JavaObject)data);
            return converter.Convert(data);
        }
    }
}