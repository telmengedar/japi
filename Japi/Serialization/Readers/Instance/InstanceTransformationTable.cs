using System;
using System.Collections.Generic;
using NightlyCode.Japi.Serialization.Transformation;

namespace NightlyCode.Japi.Serialization.Readers.Instance
{

    /// <summary>
    /// transformation table used for instance creation
    /// </summary>
    public class InstanceTransformationTable : ITransformationTable {
        readonly ITransformationTable basetable;
        readonly Dictionary<string, IDataTransformer> instancetransformers = new Dictionary<string, IDataTransformer>();

        /// <summary>
        /// creates a new instance transformation table using java transformation as base table
        /// </summary>
        /// <param name="instancetypes">types to support when transforming data</param>
        public InstanceTransformationTable(IEnumerable<Type> instancetypes)
            : this(new JavaTransformationTable(), instancetypes)
        { }

        /// <summary>
        /// creates a new instance transformation table using a custom base table
        /// </summary>
        /// <param name="basetable">transformation table for base types</param>
        /// <param name="instancetypes">types to support when transforming data</param>
        public InstanceTransformationTable(ITransformationTable basetable, IEnumerable<Type> instancetypes) {
            this.basetable = basetable;
            foreach(Type type in instancetypes) {
                InstanceTransformationDescriptor descriptor = InstanceTransformationDescriptor.FromType(type);
                instancetransformers[descriptor.JavaType] = new InstanceConverter(descriptor);
            }
        }

        public IDataTransformer Get(string type) {
            IDataTransformer transformer = basetable.Get(type);
            if(transformer != null)
                return transformer;

            instancetransformers.TryGetValue(type, out transformer);
            return transformer;
        }
    }
}