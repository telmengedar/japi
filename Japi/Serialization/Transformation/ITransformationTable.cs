namespace GoorooMania.Japi.Serialization.Transformation {

    /// <summary>
    /// interface for a table of data transformers
    /// </summary>
    public interface ITransformationTable {

        /// <summary>
        /// get transformer for specified type
        /// </summary>
        /// <param name="type">type to transform</param>
        /// <returns></returns>
        IDataTransformer Get(string type);
    }
}