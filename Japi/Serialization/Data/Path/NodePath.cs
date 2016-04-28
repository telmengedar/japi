using System.Collections.Generic;
using System.Linq;

namespace GoorooMania.Japi.Serialization.Data.Path {

    /// <summary>
    /// path to node
    /// </summary>
    public class NodePath : IPathElement {

        /// <summary>
        /// creates a new node path
        /// </summary>
        /// <param name="name"></param>
        public NodePath(string name) {
            Name = name;
        }

        /// <summary>
        /// name of node
        /// </summary>
        public string Name { get; }

        public IEnumerable<IJavaData> Select(IJavaData data) {
            if(data is JavaArray) {
                foreach(IJavaData child in ((JavaArray)data).Items)
                    foreach(IJavaData result in Select(child))
                        yield return result;
            }
            else if(data is JavaObject) {
                JavaField field = ((JavaObject)data).Fields.FirstOrDefault(f => f.Name == Name);
                if(field != null)
                    yield return field.Value;
            }
        }
    }
}