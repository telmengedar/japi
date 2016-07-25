using NightlyCode.Japi.Serialization.Data;

namespace NightlyCode.Japi.Serialization.Transformation {

    /// <summary>
    /// transforms hashmap data to meaningful structures
    /// </summary>
    public class HashMapTransformer : IDataTransformer {

        public IJavaData Convert(JavaObject @object) {
            JavaObject transformed = new JavaObject(@object.Type);

            // the fields of hashmap have no informational value
            //transformed.Fields.AddRange(@object.Fields);

            for(int i = 1; i < @object.Custom.Count; i += 2) {
                JavaValue value= @object.Custom[i] as JavaValue;
                if(value == null)
                    continue;
                JavaField field = new JavaField(value.Get<string>(), @object.Custom[i + 1]);
                transformed.Fields.Add(field);
            }
            return transformed;
        }
    }
}