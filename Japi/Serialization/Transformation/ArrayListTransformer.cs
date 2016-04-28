using System.Linq;
using GoorooMania.Japi.Serialization.Data;

namespace GoorooMania.Japi.Serialization.Transformation {

    /// <summary>
    /// transforms a serialized array list to an array type
    /// </summary>
    public class ArrayListTransformer : IDataTransformer {

        public IJavaData Convert(JavaObject @object) {
            return new JavaArray(@object.Custom.Skip(1));
        }
    }
}