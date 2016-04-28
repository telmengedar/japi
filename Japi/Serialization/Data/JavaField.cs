namespace GoorooMania.Japi.Serialization.Data {

    /// <summary>
    /// field of java object
    /// </summary>
    public class JavaField {

        /// <summary>
        /// creates a new java field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public JavaField(string name, IJavaData value) {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// name of field
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// value
        /// </summary>
        public IJavaData Value { get; set; }

        public override string ToString() {
            return Name;
        }
    }
}