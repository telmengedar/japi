using System;
using System.Linq.Expressions;
using System.Reflection;
using NightlyCode.Core.Collections;
using NightlyCode.Japi.Json.Expressions;
using NightlyCode.Japi.Json.Serialization.Handler;

namespace NightlyCode.Japi.Json.Serialization {
    public class CustomerSerializerCollection {
        readonly TypeHandlerLookup<IJSonSerializationHandler> customserializers = new TypeHandlerLookup<IJSonSerializationHandler>();

        public CustomerSerializerCollection(IJsonSerializer serializer)
        {
            customserializers.SetHandler(typeof(Type), new TypeSerializer());

#if !WINDOWS_UWP
            customserializers.SetHandler(typeof(Expression), new ExpressionSerializer(serializer));
            customserializers.SetHandler(typeof(Guid), new GuidSerializer());
            customserializers.SetHandler(typeof(MethodInfo), new MethodInfoSerializer(serializer));
            customserializers.SetHandler(typeof(SymbolDocumentInfo), new SymbolDocumentInfoSerializer(serializer));
            customserializers.SetHandler(typeof(PropertyInfo), new PropertyInfoSerializer(serializer));
            customserializers.SetHandler(typeof(MemberAssignment), new MemberAssignmentSerializer(serializer));
            customserializers.SetHandler(typeof(MemberMemberBinding), new MemberMemberBindingSerializer(serializer));
            customserializers.SetHandler(typeof(MemberListBinding), new MemberListBindingSerializer(serializer));
            customserializers.SetHandler(typeof(MemberBinding), new MemberBindingSerializer(serializer));
            customserializers.SetHandler(typeof(ConstructorInfo), new ConstructorInfoSerializer(serializer));
            customserializers.SetHandler(typeof(SwitchCase), new SwitchCaseSerializer(serializer));
            customserializers.SetHandler(typeof(CatchBlock), new CatchBlockSerializer(serializer));
            customserializers.SetHandler(typeof(FieldInfo), new FieldInfoSerializer(serializer));
            customserializers.SetHandler(typeof(MemberInfo), new MemberInfoSerializer(serializer));
#endif
        }

        public bool Contains(Type type) {
            return customserializers.ContainsKey(type);
        }

        public IJSonSerializationHandler Get(Type type) {
            return customserializers[type];
        }
    }
}