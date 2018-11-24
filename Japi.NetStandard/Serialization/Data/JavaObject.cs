using System;
using System.Collections.Generic;
using System.Linq;

namespace NightlyCode.Japi.Serialization.Data {

    /// <summary>
    /// object from java environment
    /// </summary>
    public class JavaObject : IJavaData {

        /// <summary>
        /// creates a new java object
        /// </summary>
        /// <param name="type"></param>
        public JavaObject(string type) {
            if(string.IsNullOrEmpty(type))
                throw new InvalidOperationException("type must contain a valid name");

            Type = type;
            Fields = new List<JavaField>();
            Custom = new List<IJavaData>();
        }

        /// <summary>
        /// access to field values
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IJavaData this[string name]
        {
            get { return Fields.First(f => f.Name == name).Value; }
        }

        /// <summary>
        /// typed access to field values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetField<T>(string name)
            where T : IJavaData {
            return (T)this[name];
        }

        /// <summary>
        /// get data of field if it exists or null otherwise
        /// </summary>
        /// <param name="name">name of field</param>
        /// <returns>data contained in field or null if no field with the specified name exists</returns>
        public IJavaData GetFieldOrDefault(string name) {
            return Fields.FirstOrDefault(f => f.Name == name)?.Value;
        }

        /// <summary>
        /// get custom field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetCustom<T>(int index)
            where T:IJavaData {
            return (T)Custom[index];
        }

        /// <summary>
        /// type of object
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// fields
        /// </summary>
        public List<JavaField> Fields { get; set; }

        /// <summary>
        /// custom data
        /// </summary>
        public List<IJavaData> Custom { get; set; }

        public override string ToString() {
            return Type;
        }
    }
}