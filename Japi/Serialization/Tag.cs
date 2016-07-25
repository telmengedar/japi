namespace NightlyCode.Japi.Serialization {

    /// <summary>
    /// item tags
    /// </summary>
    public enum Tag : byte {
        /// <summary>
        /// First tag value.
        ///</summary>
        BASE = 0x70,

        /// <summary>
        /// Null object reference.
        ///</summary>
        NULL = 0x70,

        /// <summary>
        /// Reference to an object already written into the stream.
        /// </summary>
        REFERENCE = 0x71,

        /// <summary>
        /// new Class Descriptor.
        /// </summary>
        CLASSDESC = 0x72,

        /// <summary>
        /// new Object.
        /// </summary>
        OBJECT = 0x73,

        /// <summary>
        /// new String.
        /// </summary>
        STRING = 0x74,

        /// <summary>
        /// new Array.
        /// </summary>
        ARRAY = 0x75,

        /// <summary>
        /// Reference to Class.
        /// </summary>
        CLASS = 0x76,

        /// <summary>
        /// Block of optional data. Byte following tag indicates number
        /// of bytes in this block data.
        /// </summary>
        BLOCKDATA = 0x77,

        /// <summary>
        /// End of optional block data blocks for an object.
        /// </summary>
        ENDBLOCKDATA = 0x78,

        /// <summary>
        /// Reset stream context. All handles written into stream are reset.
        /// </summary>
        RESET = 0x79,

        /// <summary>
        /// long Block data. The long following the tag indicates the
        /// number of bytes in this block data.
        /// </summary>
        BLOCKDATALONG = 0x7A,

        /// <summary>
        /// Exception during write.
        /// </summary>
        EXCEPTION = 0x7B,

        /// <summary>
        /// Long string.
        /// </summary>
        LONGSTRING = 0x7C,

        /// <summary>
        /// new Proxy Class Descriptor.
        /// </summary>
        PROXYCLASSDESC = 0x7D,

        /// <summary>
        /// new Enum constant.
        /// @since 1.5
        /// </summary>
        ENUM = 0x7E,

        /// <summary>
        /// Last tag value.
        /// </summary>
        MAX = 0x7E,

    }
}