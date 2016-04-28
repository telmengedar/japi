using System.Collections;
using System.Collections.Generic;
using GoorooMania.Japi.Serialization.Data;

namespace GoorooMania.Japi.Serialization.Transformation {

    /// <summary>
    /// reduces a java object structure by transforming known structures
    /// </summary>
    /// <remarks>
    /// useful when structure is to be transformed to xml or other formats where simple representations are preferred
    /// </remarks>
    public class StructureReducer {
        readonly ITransformationTable transformationtable;

        /// <summary>
        /// creates a new structure reducer using the default java transformation table
        /// </summary>
        public StructureReducer()
            : this(new JavaTransformationTable())
        { }

        /// <summary>
        /// creates a new structure reducer using a custom transformation table
        /// </summary>
        /// <param name="transformationtable"></param>
        public StructureReducer(ITransformationTable transformationtable) {
            this.transformationtable = transformationtable;
        }

        /// <summary>
        /// reduces the structure of the specified object
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public IJavaData Reduce(JavaObject @object) {
            HashSet<JavaObject> visited = new HashSet<JavaObject>();
            return ReduceObject(@object, visited);
        }

        void ReduceArray(JavaArray array, HashSet<JavaObject> visited) {
            for(int i = array.Items.Count - 1; i >= 0; --i) {
                IJavaData data = array.Items[i];
                if (data is JavaArray)
                    ReduceArray((JavaArray)data, visited);
                else if(data is JavaObject)
                    array.Items[i] = ReduceObject((JavaObject)data, visited);
            }
        }

        IJavaData ReduceObject(JavaObject @object, HashSet<JavaObject> visited) {
            if(visited.Contains(@object))
                return @object;
            visited.Add(@object);

            foreach(JavaField field in @object.Fields) {
                if(field.Value is JavaArray)
                    ReduceArray((JavaArray)field.Value, visited);
                else if(field.Value is JavaObject)
                    field.Value = ReduceObject((JavaObject)field.Value, visited);
            }

            for(int i = @object.Custom.Count-1; i >= 0; --i) {
                IJavaData data = @object.Custom[i];
                if(data is JavaArray)
                    ReduceArray((JavaArray)data, visited);
                else if(data is JavaObject)
                    @object.Custom[i] = ReduceObject((JavaObject)data, visited);
            }

            visited.Remove(@object);

            IDataTransformer transformer = transformationtable.Get(@object.Type);
            if(transformer == null)
                return @object;

            return transformer.Convert(@object);
        }
    }
}