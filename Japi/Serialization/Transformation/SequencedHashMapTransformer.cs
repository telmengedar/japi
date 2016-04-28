using GoorooMania.Japi.Serialization.Data;

namespace GoorooMania.Japi.Serialization.Transformation {

    /// <summary>
    /// transforms sequenced hash map to meaningful structures
    /// </summary>
    public class SequencedHashMapTransformer : IDataTransformer {

        public IJavaData Convert(JavaObject @object)
        {
            JavaObject transformed = new JavaObject(@object.Type);
            transformed.Fields.AddRange(@object.Fields);
            for (int i = 1; i < @object.Custom.Count; i += 2)
            {
                JavaValue value = @object.Custom[i] as JavaValue;
                if (value == null)
                    continue;
                JavaField field = new JavaField(value.Get<string>(), @object.Custom[i + 1]);
                transformed.Fields.Add(field);
            }
            return transformed;
        }

    }
}