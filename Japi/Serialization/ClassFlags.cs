using System;

namespace GoorooMania.Japi.Serialization {

    /// <summary>
    /// Bit masks for ObjectStreamClass flag
    /// </summary>
    [Flags]
    public enum ClassFlags {

        /// <summary>
        /// Bit mask for ObjectStreamClass flag. Indicates a Serializable class
        /// defines its own writeObject method.
        /// </summary>
        WRITE_METHOD = 0x01,

        /// <summary>
        /// Bit mask for ObjectStreamClass flag. Indicates class is Serializable.
        /// </summary>
        SERIALIZABLE = 0x02,

        /// <summary>
        /// Bit mask for ObjectStreamClass flag. Indicates class is Externalizable.
        /// </summary>
        EXTERNALIZABLE = 0x04,

        /// <summary>
        /// Bit mask for ObjectStreamClass flag. Indicates Externalizable data
        /// written in Block Data mode.
        /// Added for PROTOCOL_VERSION_2.
        ///
        /// @see #PROTOCOL_VERSION_2
        /// @since 1.2
        /// </summary>
        BLOCK_DATA = 0x08,

        /// <summary>
        /// Bit mask for ObjectStreamClass flag. Indicates class is an enum type.
        /// @since 1.5
        /// </summary>
        ENUM = 0x10
    }
}