using System;
using NightlyCode.Core.Conversion;

namespace NightlyCode.Japi.Serialization.Data {

    /// <summary>
    /// single value from java environment
    /// </summary>
    public class JavaValue : IJavaData {

        /// <summary>
        /// creates a new java value
        /// </summary>
        /// <param name="value"></param>
        public JavaValue(object value) {
            if(value is IJavaData)
                throw new InvalidOperationException("value must not be some kind of IJavaData");
            Value = value;
        }

        /// <summary>
        /// value
        /// </summary>
        public object Value { get; set; }

        public T Get<T>() {
            return Converter.Convert<T>(Value);
        }

        public override string ToString() {
            return Value?.ToString() ?? "<null>";
        }
    }
}