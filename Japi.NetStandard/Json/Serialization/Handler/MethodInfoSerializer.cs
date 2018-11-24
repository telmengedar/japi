using System;
using System.Linq;
using System.Reflection;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="MethodInfo"/>s
    /// </summary>
    public class MethodInfoSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="MethodInfoSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public MethodInfoSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            MethodInfo methodinfo = (MethodInfo)value;
            return new JsonObject {
                ["type"]=new JsonValue("method"),
                ["host"] = serializer.Write(methodinfo.DeclaringType),
                ["method"] = new JsonValue(methodinfo.Name),
                ["parameters"] = new JsonArray(methodinfo.GetParameters().Select(p => serializer.Write(p.ParameterType))),
                ["genericarguments"] = new JsonArray(methodinfo.GetGenericArguments().Select(serializer.Write))
            };
        }

        public object Deserialize(JsonNode json) {
            Type host = serializer.Read<Type>(json["host"]);
            string name = json.SelectValue<string>("method");
            Type[] parameters = json["parameters"].Select(serializer.Read<Type>).ToArray();
            Type[] generic = json["genericarguments"].Select(serializer.Read<Type>).ToArray();

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