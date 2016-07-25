using System.Collections.Generic;
using System.Linq;

namespace NightlyCode.Japi.Serialization.Data.Path {

    /// <summary>
    /// operations used for java data
    /// </summary>
    public static class DataOperations {
        static readonly DataPathParser parser = new DataPathParser();

        /// <summary>
        /// select child data using a xpath like path specification
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path">path to data</param>
        /// <returns>data if path leads to a valid node, null otherwise</returns>
        public static IEnumerable<IJavaData> SelectNodes(this IJavaData data, string path) {
            IPathElement[] elements = parser.ParsePath(path).ToArray();
            return SelectNodes(data, elements, 0);
        }

        /// <summary>
        /// selects nodes using a path
        /// </summary>
        /// <typeparam name="T">type of nodes to select</typeparam>
        /// <param name="data">data from which to select</param>
        /// <param name="path">path to nodes</param>
        /// <returns>nodes of the selected type which match the path</returns>
        public static IEnumerable<T> SelectNodes<T>(this IJavaData data, string path)
            where T : IJavaData {
            return SelectNodes(data, path).Where(n => n is T).Cast<T>();
        }

        /// <summary>
        /// selects a single node of a java structure
        /// </summary>
        /// <typeparam name="T">type of node to select</typeparam>
        /// <param name="data">data to select node from</param>
        /// <param name="path">path to target node</param>
        /// <returns></returns>
        public static T SelectNode<T>(this IJavaData data, string path)
            where T : IJavaData {
            return SelectNodes<T>(data, path).FirstOrDefault();
        }

        /// <summary>
        /// select a single node of a java structure which matches the specified path
        /// </summary>
        /// <param name="data">root to select data from</param>
        /// <param name="path">path to data</param>
        /// <returns></returns>
        public static IJavaData SelectNode(this IJavaData data, string path) {
            return SelectNodes(data, path).FirstOrDefault();
        }

        /// <summary>
        /// selects values residing in a java structure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">data to select values from</param>
        /// <param name="path">path to values</param>
        /// <returns></returns>
        public static IEnumerable<T> SelectValues<T>(this IJavaData data, string path) {
            foreach(IJavaData child in SelectNodes(data, path)) {
                if(child is JavaValue)
                    yield return ((JavaValue)child).Get<T>();
                else if(child is JavaArray)
                    foreach(IJavaData result in ((JavaArray)child).Items)
                        if(result is JavaValue)
                            yield return ((JavaValue)result).Get<T>();
            }
        }

        /// <summary>
        /// selects a single value residing in a java structure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">data to select values from</param>
        /// <param name="path">path to value</param>
        /// <returns></returns>
        public static T SelectValue<T>(this IJavaData data, string path) {
            return SelectValues<T>(data, path).FirstOrDefault();
        }

        static IEnumerable<IJavaData> SelectNodes(IJavaData data, IPathElement[] path, int pathindex) {

            // path was traversed successfully
            // return found data
            if (pathindex >= path.Length)
            {
                yield return data;
            }
            else {
                foreach (IJavaData child in path[pathindex].Select(data))
                    foreach(IJavaData result in SelectNodes(child, path, pathindex + 1))
                        yield return result;
            }               
        }

    }
}