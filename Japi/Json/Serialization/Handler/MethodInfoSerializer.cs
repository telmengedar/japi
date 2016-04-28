using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class MethodInfoSerializer : IJSonSerializationHandler {
        public JsonNode Serialize(object value) {
            MethodInfo methodinfo = (MethodInfo)value;
            return new JsonObject {
                ["type"]=new JsonValue("method"),
                ["host"] = JsonSerializer.Write(methodinfo.DeclaringType),
                ["method"] = new JsonValue(methodinfo.Name),
                ["parameters"] = new JsonArray(methodinfo.GetParameters().Select(p => JsonSerializer.Write(p.ParameterType))),
                ["genericarguments"] = new JsonArray(methodinfo.GetGenericArguments().Select(JsonSerializer.Write))
            };
        }

        public object Deserialize(JsonNode json) {
            Type host = JsonSerializer.Read<Type>(json["host"]);
            string name = json.SelectValue<string>("method");
            Type[] parameters = json["parameters"].Select(JsonSerializer.Read<Type>).ToArray();
            Type[] generic = json["genericarguments"].Select(JsonSerializer.Read<Type>).ToArray();

            return host.GetMethods().First(m => m.Name == name && IEnumerableEquals(parameters, m.GetParameters().Select(p => p.ParameterType)) && IEnumerableEquals(generic, m.GetGenericArguments()));
        }

        bool IEnumerableEquals<T>(IEnumerable<T> lhs, IEnumerable<T> rhs) {
            T[] lhsarray = lhs is Array ? (T[])lhs : lhs.ToArray();
            T[] rhsarray = rhs is Array ? (T[])rhs : rhs.ToArray();

            if(lhsarray.Length != rhsarray.Length)
                return false;

            for(int i = 0; i < lhsarray.Length; ++i) {
                if(!lhsarray[i].Equals(rhsarray[i]))
                    return false;
            }

            return true;
        }
    }
}