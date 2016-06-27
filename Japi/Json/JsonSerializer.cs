using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GoorooMania.Japi.Extern;
#if !WINDOWS_UWP
    using GoorooMania.Japi.Json.Expressions;
#endif

using GoorooMania.Japi.Json.Serialization;
using GoorooMania.Japi.Json.Serialization.Handler;

namespace GoorooMania.Japi.Json
{

    /// <summary>
    /// serializes objects to json structures
    /// </summary>
    public class JsonSerializer {
        static readonly TypeHandlerLookup<IJSonSerializationHandler> customserializers = new TypeHandlerLookup<IJSonSerializationHandler>();

        static JsonSerializer() {
            customserializers.SetHandler(typeof(Type), new TypeSerializer());

#if !WINDOWS_UWP
            customserializers.SetHandler(typeof(Expression), new ExpressionSerializer());
            customserializers.SetHandler(typeof(Guid), new GuidSerializer());
            customserializers.SetHandler(typeof(MethodInfo), new MethodInfoSerializer());
            customserializers.SetHandler(typeof(SymbolDocumentInfo), new SymbolDocumentInfoSerializer());            
            customserializers.SetHandler(typeof(PropertyInfo), new PropertyInfoSerializer());
            customserializers.SetHandler(typeof(MemberAssignment), new MemberAssignmentSerializer());
            customserializers.SetHandler(typeof(MemberMemberBinding), new MemberMemberBindingSerializer());
            customserializers.SetHandler(typeof(MemberListBinding), new MemberListBindingSerializer());
            customserializers.SetHandler(typeof(MemberBinding), new MemberBindingSerializer());
            customserializers.SetHandler(typeof(ConstructorInfo), new ConstructorInfoSerializer());
            customserializers.SetHandler(typeof(SwitchCase), new SwitchCaseSerializer());
            customserializers.SetHandler(typeof(CatchBlock), new CatchBlockSerializer());
            customserializers.SetHandler(typeof(FieldInfo), new FieldInfoSerializer());
            customserializers.SetHandler(typeof(MemberInfo), new MemberInfoSerializer());
#endif
        }

        /// <summary>
        /// reads a type from a json node
        /// </summary>
        /// <typeparam name="T">type to read</typeparam>
        /// <param name="node">json node</param>
        /// <returns>instance with data from json node</returns>
        public static T Read<T>(JsonNode node) {
            return (T)Read(typeof(T), node);
        }

        /// <summary>
        /// reads a type from a json node
        /// </summary>
        /// <param name="type">type to read</param>
        /// <param name="node">json node</param>
        /// <returns>instance with data from json node</returns>
        public static object Read(Type type, JsonNode node) {
            return Read(type, node, false);
        }

        /// <summary>
        /// reads a type from a json node
        /// </summary>
        /// <param name="type">type to read</param>
        /// <param name="node">json node</param>
        /// <param name="variant"></param>
        /// <returns>instance with data from json node</returns>
        static object Read(Type type, JsonNode node, bool variant) {
            if(type.GetInterfaces().Contains(typeof(ICustomJsonSerialization))) {
                Type customtype = Read<Type>(node["type"]);
#if WINDOWS_UWP
                ICustomJsonSerialization customobject = (ICustomJsonSerialization)Activator.CreateInstance(customtype);
#else
                ICustomJsonSerialization customobject = (ICustomJsonSerialization)Activator.CreateInstance(customtype, true);
#endif
                customobject.Deserialize(node["data"]);
                return customobject;
            }

            if(node is JsonValue && ((JsonValue)node).Value == null)
                return null;

            if(type.IsArray) {
                if(type.GetElementType() == typeof(byte)) {
                    if(!(node is JsonValue))
                        throw new JsonException("Unable to read value from non value node");
                    string base64value = ((JsonValue)node).Value as string;
                    if(base64value == null)
                        return null;
                    return Convert.FromBase64String(base64value);
                }

                if (!(node is JsonArray))
                    throw new JsonException("Unable to read array from non array json node");
                return ReadArray(type.GetElementType(), (JsonArray)node);
            }

#if WINDOWS_UWP
            if (type.GetTypeInfo().IsValueType || type.GetTypeInfo().IsEnum || type == typeof(string) || type == typeof(Version)) {
#else        
            if(type.IsValueType || type.IsEnum || type == typeof(string) || type == typeof(Version)) {
#endif
                if (!(node is JsonValue))
                    throw new JsonException("Unable to read value from non value node");
                return Converter.Convert(((JsonValue)node).Value, type);
            }

            // check for variant types
            if (variant)
            {
                Type customtype = Read<Type>(node["type"]);
                return Read(customtype, node["data"]);
            }

            if (customserializers.ContainsKey(type))
                return customserializers[type].Deserialize(node);

            if(!(node is JsonObject))
                throw new JsonException("Unable to read object from non object node");
            JsonObject @object = (JsonObject)node;

#if WINDOWS_UWP
            object instance = Activator.CreateInstance(type);
#else
            object instance = Activator.CreateInstance(type, true);
#endif

            foreach(PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                if(!property.CanWrite)
                    continue;

                string key = JsonKeyAttribute.GetKey(property) ?? property.Name.ToLower();

                if(!@object.ContainsKey(key))
                    continue;

                string loader = JsonLoaderAttribute.Get(property);

                object value;
                if(loader != null) {
                    MethodInfo method = type.GetMethod(loader, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    value = method.Invoke(null, new object[] { @object[key] });
                }
                else {
                    value = Read(property.PropertyType, @object[key], VariantAttribute.IsVariant(property));
                }
                property.SetValue(instance, Converter.Convert(value, property.PropertyType), null);
            }
            return instance;
        }

        static object ReadArray(Type elementtype, JsonArray node) {
            Array instance = Array.CreateInstance(elementtype, node.ItemCount);
            for(int i = 0; i < node.ItemCount; ++i)
                instance.SetValue(Read(elementtype, node[i]), i);
            return instance;
        }

        /// <summary>
        /// writes an object to a json structure
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static JsonNode Write(object @object) {
            return Write(@object, false);
        }

        /// <summary>
        /// writes an object to a json structure
        /// </summary>
        /// <param name="object"></param>
        /// <param name="variant"></param>
        /// <returns></returns>
        static JsonNode Write(object @object, bool variant) {
            bool iscustom = @object is ICustomJsonSerialization;
            if (iscustom) {
                JsonObject jsonobject = new JsonObject {
                    ["type"] = Write(@object.GetType()),
                    ["data"] = ((ICustomJsonSerialization)@object).Serialize()
                };
                return jsonobject;
            }

            if(@object is Array) {
                if(@object.GetType().GetElementType() == typeof(byte))
                    return new JsonValue(Convert.ToBase64String((byte[])@object));
                return WriteArray((Array)@object, variant);
            }

#if WINDOWS_UWP
            if (@object == null || @object is string || @object.GetType().GetTypeInfo().IsEnum || @object.GetType().GetTypeInfo().IsValueType || @object is Version)
#else
            if (@object == null || @object is string || @object.GetType().IsEnum || @object.GetType().IsValueType || @object is Version)
#endif
                return new JsonValue(@object);

            // check for variant types
            if (variant) {
                JsonObject jsonobject = new JsonObject {
                    ["type"] = Write(@object.GetType()),
                    ["data"] = Write(@object)
                };
                return jsonobject;
            }

            if (customserializers.ContainsKey(@object.GetType()))
                return customserializers[@object.GetType()].Serialize(@object);

            JsonObject json = new JsonObject();
            foreach(PropertyInfo property in @object.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                if(!property.CanWrite)
                    continue;

                string key = JsonKeyAttribute.GetKey(property) ?? property.Name.ToLower();
                json[key] = Write(property.GetValue(@object), VariantAttribute.IsVariant(property));
            }
            return json;
        }

        static JsonArray WriteArray(Array array, bool variant = false) {
            JsonArray json = new JsonArray();
            for(int i = 0; i < array.Length; ++i)
                json.Add(Write(array.GetValue(i), variant));
            return json;
        }
    }
}