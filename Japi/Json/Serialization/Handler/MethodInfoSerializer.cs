using System;
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

            foreach(MethodInfo method in host.GetMethods().Where(m => m.Name == name)) {
                Type[] genericarguments = method.GetGenericArguments();
                int index = 0;
                bool matches = true;
                foreach(Type type in genericarguments) {
                    if(!type.IsGenericParameter) {
                        matches = type == generic[index];
                        if(!matches)
                            break;
                    }
                    ++index;
                }

                if(!matches)
                    continue;

                ParameterInfo[] methodparameters = method.GetParameters();
                if(methodparameters.Length != parameters.Length)
                    continue;

                index = 0;
                foreach(ParameterInfo parameterinfo in methodparameters) {
                    if(parameterinfo.ParameterType.IsGenericType)
                        matches = parameterinfo.ParameterType.Namespace == parameters[index].Namespace && parameterinfo.ParameterType.Name == parameters[index].Name;
                    else if(!parameterinfo.ParameterType.IsGenericParameter)
                        matches = parameterinfo.ParameterType == parameters[index];
                    if(!matches)
                        break;
                    ++index;
                }
                if(matches) {
                    if(generic.Length > 0)
                        return method.MakeGenericMethod(generic);
                    return method;
                }
            }
            throw new MissingMethodException("Method not found");
            //return host.GetMethods().First(m => m.Name == name && IEnumerableEquals(parameters, m.GetParameters().Select(p => p.ParameterType)) && IEnumerableEquals(generic, m.GetGenericArguments()));
        }
    }
}