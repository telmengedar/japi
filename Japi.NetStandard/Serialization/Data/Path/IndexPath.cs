using System.Collections.Generic;
using System.Linq;

namespace NightlyCode.Japi.Serialization.Data.Path {
    /// <summary>
    /// index of an element
    /// </summary>
    public class IndexPath : IPathElement {

        /// <summary>
        /// creates a new index path
        /// </summary>
        /// <param name="index"></param>
        public IndexPath(int index) {
            Index = index;
        }

        /// <summary>
        /// index to element
        /// </summary>
        public int Index { get; set; }

        public IEnumerable<IJavaData> Select(IJavaData data) {
            IEnumerable<IJavaData> enumeration;
            if(data is JavaArray) {
                enumeration=((JavaArray)data).Items.Skip(Index).Take(1);
            }
            else if(data is JavaObject) {
                enumeration = ((JavaObject)data).Custom.Skip(Index).Take(1);
            }
            else yield break;

            foreach(IJavaData child in enumeration)
                yield return child;
        }
    }
}