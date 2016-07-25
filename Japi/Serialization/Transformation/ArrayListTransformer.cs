using System.Linq;
using NightlyCode.Japi.Serialization.Data;

namespace NightlyCode.Japi.Serialization.Transformation {

    /// <summary>
    /// transforms a serialized array list to an array type
    /// </summary>
    public class ArrayListTransformer : IDataTransformer {

        public IJavaData Convert(JavaObject @object) {
            return new JavaArray(@object.Custom.Skip(1));
        }
    }
}