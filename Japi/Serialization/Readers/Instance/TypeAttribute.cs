using System;

namespace GoorooMania.Japi.Serialization.Readers.Instance {

    /// <summary>
    /// used to specify a type mapped to an object
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TypeAttribute : Attribute {

        /// <summary>
        /// creates a new type attribute
        /// </summary>
        /// <param name="type"></param>
        public TypeAttribute(string type) {
            Type = type;
        }

        /// <summary>
        /// type the class is mapped to
        /// </summary>
        public string Type { get; private set; }
    }
}