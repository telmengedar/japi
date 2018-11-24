using System;
using System.Linq;
using NightlyCode.Japi.Serialization.Data;
using NightlyCode.Japi.Serialization.Data.Path;

namespace NightlyCode.Japi.Serialization.Transformation {

    /// <summary>
    /// transforms timestamp structures to timestamp values
    /// </summary>
    public class TimeStampTransformer : IDataTransformer {
        static readonly DateTime unix = new DateTime(1970, 1, 1);

        public IJavaData Convert(JavaObject @object) {
            byte[] data = @object.SelectValue<byte[]>("[0]");
            long milli = BitConverter.ToInt64(data.Reverse().ToArray(), 0);
            return new JavaValue(unix + TimeSpan.FromMilliseconds(milli));
        }
    }
}