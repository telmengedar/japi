using GoorooMania.Japi.Serialization.Data;
using GoorooMania.Japi.Serialization.Data.Path;

namespace GoorooMania.Japi.Serialization.Transformation {

    /// <summary>
    /// transforms java pod wrappers to normal pod values
    /// </summary>
    public class PODTransformer : IDataTransformer {

        public IJavaData Convert(JavaObject @object) {

            return new JavaValue(@object.SelectValue<object>("value"));
        }
    }
}