namespace GoorooMania.Japi.Serialization.Data {

    /// <summary>
    /// exception in java stream
    /// </summary>
    public class JavaException : IJavaData {

        /// <summary>
        /// creates a new java exception
        /// </summary>
        /// <param name="object"></param>
        public JavaException(IJavaData @object) {
            Object = @object;
        }

        /// <summary>
        /// exception object
        /// </summary>
        public IJavaData Object { get; set; } 
    }
}