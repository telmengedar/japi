using System;
using System.Collections.Generic;
using System.Reflection;

namespace NightlyCode.Japi.Serialization.Readers.Instance {

    /// <summary>
    /// descriptor for instance transformation
    /// </summary>
    public class InstanceTransformationDescriptor {
        readonly Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();


        InstanceTransformationDescriptor(string javatype, Type instancetype, IEnumerable<Tuple<string, PropertyInfo>> properties) {
            JavaType = javatype;
            foreach(Tuple<string, PropertyInfo> property in properties)
                this.properties[property.Item1] = property.Item2;
        }

        /// <summary>
        /// name of type instance is mapped to
        /// </summary>
        public string JavaType { get; private set; }

        /// <summary>
        /// type of instance
        /// </summary>
        public Type InstanceType { get; set; }

        /// <summary>
        /// get property mapped to field name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PropertyInfo GetProperty(string name) {
            PropertyInfo property;
            properties.TryGetValue(name, out property);
            return property;
        }

        /// <summary>
        /// creates a new instance
        /// </summary>
        /// <returns></returns>
        public object CreateInstance() {
            return Activator.CreateInstance(InstanceType, true);
        }

        public static InstanceTransformationDescriptor FromType(Type type) {
            TypeAttribute typeattribute = Attribute.GetCustomAttribute(type, typeof(TypeAttribute)) as TypeAttribute;
            if(typeattribute == null)
                throw new InvalidOperationException("'" + type.Name + "' contains no type mapping information.");

            return new InstanceTransformationDescriptor(typeattribute.Type, type, ReadProperties(type));
        }

        static IEnumerable<Tuple<string, PropertyInfo>> ReadProperties(Type type) {
            foreach(PropertyInfo property in type.GetProperties()) {
                if(!property.CanWrite)
                    continue;

                FieldAttribute fieldattribute=Attribute.GetCustomAttribute(property, typeof(FieldAttribute)) as FieldAttribute;
                if(fieldattribute == null)
                    continue;
                yield return new Tuple<string, PropertyInfo>(fieldattribute.Field, property);
            }
        }
    }
}