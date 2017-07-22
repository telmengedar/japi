using System.Reflection;
using NightlyCode.Japi.Extern;
using NightlyCode.Japi.Serialization.Data;
using NightlyCode.Japi.Serialization.Transformation;

namespace NightlyCode.Japi.Serialization.Readers.Instance {

    /// <summary>
    /// transforms java object data to an instance value
    /// </summary>
    public class InstanceConverter : IDataTransformer {
        readonly InstanceTransformationDescriptor descriptor;

        /// <summary>
        /// creates a new instance transformer
        /// </summary>
        /// <param name="descriptor"></param>
        public InstanceConverter(InstanceTransformationDescriptor descriptor) {
            this.descriptor = descriptor;
        }

        public IJavaData Convert(JavaObject @object) {
            object instance = descriptor.CreateInstance();
            foreach(JavaField field in @object.Fields) {
                if(!(field.Value is JavaValue))
                    continue;

                PropertyInfo property = descriptor.GetProperty(field.Name);
                property?.SetValue(instance, Converter.Convert(((JavaValue)field.Value).Value, property.PropertyType, true), null);                
            }
            return new JavaValue(instance);
        }
    }
}