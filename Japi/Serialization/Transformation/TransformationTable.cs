using System.Collections.Generic;

namespace GoorooMania.Japi.Serialization.Transformation {

    /// <summary>
    /// base implementation of a transformation table
    /// </summary>
    public abstract class TransformationTable : ITransformationTable {
        readonly Dictionary<string, IDataTransformer> transformers = new Dictionary<string, IDataTransformer>();

        /// <summary>
        /// adds an transformer to the table
        /// </summary>
        /// <param name="type"></param>
        /// <param name="transformer"></param>
        protected void Add(string type, IDataTransformer transformer) {
            transformers[type] = transformer;
        }

        /// <summary>
        /// get transformer for specified type
        /// </summary>
        /// <param name="type">type to transform</param>
        /// <returns></returns>
        public IDataTransformer Get(string type) {
            IDataTransformer transformer;
            transformers.TryGetValue(type, out transformer);
            return transformer;
        }
    }
}