using NightlyCode.Japi.Serialization.Data;
using NightlyCode.Japi.Serialization.Data.Path;

namespace NightlyCode.Japi.Serialization.Transformation {

    /// <summary>
    /// transforms java pod wrappers to normal pod values
    /// </summary>
    public class PODTransformer : IDataTransformer {

        public IJavaData Convert(JavaObject @object) {

            return new JavaValue(@object.SelectValue<object>("value"));
        }
    }
}