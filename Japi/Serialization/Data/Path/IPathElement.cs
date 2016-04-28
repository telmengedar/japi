using System.Collections.Generic;

namespace GoorooMania.Japi.Serialization.Data.Path {

    /// <summary>
    /// identification interface for a path element
    /// </summary>
    public interface IPathElement {

        /// <summary>
        /// selects all child nodes of data which match the path description
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IEnumerable<IJavaData> Select(IJavaData data);
    }
}