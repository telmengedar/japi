using System;

namespace GoorooMania.Japi.Serialization.Readers.Instance {

    /// <summary>
    /// specifies a field the property is mapped to
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute {

        /// <summary>
        /// creates a new field attribute
        /// </summary>
        /// <param name="field"></param>
        public FieldAttribute(string field) {
            Field = field;
        }

        /// <summary>
        /// name of field property is mapped to
        /// </summary>
        public string Field { get; private set; }
    }
}