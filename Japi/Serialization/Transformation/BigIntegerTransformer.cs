using System.Linq;
using GoorooMania.Japi.Serialization.Data;
using GoorooMania.Japi.Serialization.Data.Path;

namespace GoorooMania.Japi.Serialization.Transformation {

    /// <summary>
    /// writer for big integers
    /// </summary>
    public class BigIntegerTransformer : IDataTransformer {

        public IJavaData Convert(JavaObject @object) {
            byte[] bytes = @object.SelectValues<byte>("magnitude").ToArray();
            long signum = @object.SelectValue<long>("signum");
            if(bytes.Length == 0)
                return new JavaValue(signum);

            foreach(byte field in bytes)
                signum = (signum << 8) | field;
            return new JavaValue(signum);
        }
    }
}