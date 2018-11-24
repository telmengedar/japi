using NightlyCode.Japi.Serialization.Data;
using NightlyCode.Japi.Serialization.Data.Path;

namespace NightlyCode.Japi.Serialization.Transformation {

    /// <summary>
    /// writes big decimal values
    /// </summary>
    public class BigDecimalTransformer : IDataTransformer {

        public IJavaData Convert(JavaObject @object) {
            double value = @object.SelectValue<double>("intVal");
            int scale = @object.SelectValue<int>("scale");
            for (int i = 0; i < scale; ++i)
                value /= 10.0;
            return new JavaValue(value);
        }
    }
}