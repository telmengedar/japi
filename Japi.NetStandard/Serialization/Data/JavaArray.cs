using System.Collections.Generic;

namespace NightlyCode.Japi.Serialization.Data {

    /// <summary>
    /// array of java data
    /// </summary>
    public class JavaArray : IJavaData {

        /// <summary>
        /// creates a new java array
        /// </summary>
        public JavaArray() {
            Items=new List<IJavaData>();
        }

        /// <summary>
        /// creates a new java array
        /// </summary>
        /// <param name="items"></param>
        public JavaArray(IEnumerable<IJavaData> items)
            : this() {
            Items.AddRange(items);
        }

        /// <summary>
        /// items in array
        /// </summary>
        public List<IJavaData> Items { get; private set; }
    }
}