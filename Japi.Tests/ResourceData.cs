namespace GoorooMania.Japi.Tests {

    /// <summary>
    /// used to link resource names to data (so that tests are more readable)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResourceData<T> {

        /// <summary>
        /// creates new resource data
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="data"></param>
        public ResourceData(string resource, T data) {
            Resource = resource;
            Data = data;
        }

        /// <summary>
        /// name of resource
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// data
        /// </summary>
        public T Data { get; set; }

        public override string ToString() {
            return Resource;
        }
    }
}