using System.IO;
using System.Xml;
using NightlyCode.Japi.Serialization.Conversion;
using NightlyCode.Japi.Serialization.Transformation;

namespace NightlyCode.Japi.Serialization.Readers {

    /// <summary>
    /// reads xml from java object streams
    /// </summary>
    public class XmlObjectReader : ObjectReader<XmlDocument> {

        /// <summary>
        /// creates a new xml object reader
        /// </summary>
        /// <param name="stream">stream to read data from</param>
        public XmlObjectReader(Stream stream)
            : this(stream, new JavaTransformationTable()) {}

        /// <summary>
        /// creates a new xml object reader
        /// </summary>
        /// <param name="stream">stream to read data from</param>
        /// <param name="transformationtable">transformation table used for reduction</param>
        public XmlObjectReader(Stream stream, ITransformationTable transformationtable)
            : base(stream, new XmlDataConverter(), new StructureReducer(transformationtable))
        { }
    }
}