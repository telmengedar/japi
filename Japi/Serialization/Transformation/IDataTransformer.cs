using GoorooMania.Japi.Serialization.Data;

namespace GoorooMania.Japi.Serialization.Transformation {

    /// <summary>
    /// interface for java data conversion
    /// </summary>
    public interface IDataTransformer {

        /// <summary>
        /// transforms an object structure
        /// </summary>
        /// <param name="object">object to transform</param>
        /// <returns></returns>
        IJavaData Convert(JavaObject @object);
    }
}