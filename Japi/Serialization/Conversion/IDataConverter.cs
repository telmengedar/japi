using GoorooMania.Japi.Serialization.Data;

namespace GoorooMania.Japi.Serialization.Conversion {

    /// <summary>
    /// interface for a data converter which converts java data to another representation
    /// </summary>
    /// <typeparam name="T">type of target data
    /// </typeparam>
    public interface IDataConverter<T> {

        /// <summary>
        /// converts java data to another representation
        /// </summary>
        /// <param name="data">data to convert</param>
        /// <returns>converted data</returns>
        T Convert(IJavaData data);
    }
}