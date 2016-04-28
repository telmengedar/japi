using GoorooMania.Japi.Serialization.Data;

namespace GoorooMania.Japi.Serialization {

    /// <summary>
    /// descriptor for class data
    /// </summary>
    public class ClassDescriptor : IJavaData {

        /// <summary>
        /// base class
        /// </summary>
        public ClassDescriptor Base { get; set; }

        /// <summary>
        /// name of class
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// class flags
        /// </summary>
        public ClassFlags Flags { get; set; }

        /// <summary>
        /// sid of class
        /// </summary>
        public long SID { get; set; }

        /// <summary>
        /// interfaces of class
        /// </summary>
        public string[] Interfaces { get; set; }

        /// <summary>
        /// fields in class
        /// </summary>
        public FieldDescriptor[] Fields { get; set; }

        public IJavaData[] Custom { get; set; }

        public override string ToString() {
            return Name;
        }
    }
}