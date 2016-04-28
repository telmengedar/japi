using System;
using System.Linq.Expressions;
using System.Reflection;
using GoorooMania.Core.Collections;
using GoorooMania.Core.Conversion;
using GoorooMania.Japi.Json.Expressions;
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
            customserializers.SetHandler(typeof(Expression), new ExpressionSerializer());
            customserializers.SetHandler(typeof(Guid), new GuidSerializer());
            customserializers.SetHandler(typeof(MethodInfo), new MethodInfoSerializer());
            customserializers.SetHandler(typeof(SymbolDocumentInfo), new SymbolDocumentInfoSerializer());
            customserializers.SetHandler(typeof(Type), new TypeSerializer());
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

            if(type.IsValueType || type.IsEnum || type == typeof(string)) {
                if(!(node is JsonValue))
                    throw new JsonException("Unable to read value from non value node");
                return Converter.Convert(((JsonValue)node).Value, type);
            }

            if(customserializers.ContainsKey(type))
                return customserializers[type].Deserialize(node);

            if(!(node is JsonObject))
                throw new JsonException("Unable to read object from non object node");
            JsonObject @object = (JsonObject)node;

            object instance = Activator.CreateInstance(type);
            foreach(PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                if(!property.CanWrite|| !@object.ContainsKey(property.Name.ToLower()))
                    continue;
                object value = Read(property.PropertyType, @object[property.Name.ToLower()]);
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
            if(@object is Array) {
                if(@object.GetType().GetElementType() == typeof(byte))
                    return new JsonValue(Convert.ToBase64String((byte[])@object));
                return WriteArray((Array)@object);
            }

            if(@object == null || @object is string || @object.GetType().IsEnum || @object.GetType().IsValueType)
                return new JsonValue(@object);

            if(customserializers.ContainsKey(@object.GetType()))
                return customserializers[@object.GetType()].Serialize(@object);

            JsonObject json = new JsonObject();
            foreach(PropertyInfo property in @object.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                if(!property.CanWrite)
                    continue;
                json[property.Name.ToLower()] = Write(property.GetValue(@object));
            }
            return json;
        }

        static JsonArray WriteArray(Array array) {
            JsonArray json = new JsonArray();
            for(int i = 0; i < array.Length; ++i)
                json.Add(Write(array.GetValue(i)));
            return json;
        }
    }
}