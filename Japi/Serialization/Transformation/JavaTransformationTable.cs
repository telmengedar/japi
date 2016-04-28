
namespace GoorooMania.Japi.Serialization.Transformation {

    /// <summary>
    /// writer mapping for known java types
    /// </summary>
    public class JavaTransformationTable : TransformationTable {

        /// <summary>
        /// creates a new java writer mapping
        /// </summary>
        public JavaTransformationTable() {
            Add("java.lang.Boolean", new PODTransformer());
            Add("java.lang.Integer", new PODTransformer());
            Add("java.math.BigInteger", new BigIntegerTransformer());
            Add("java.math.BigDecimal", new BigDecimalTransformer());
            Add("java.util.Locale", new LocaleTransformer());
            Add("java.util.ArrayList", new ArrayListTransformer());
            Add("java.util.HashMap", new HashMapTransformer());
            Add("org.apache.commons.collections.SequencedHashMap", new SequencedHashMapTransformer());
            Add("java.sql.Timestamp", new TimeStampTransformer());
        }
    }
}