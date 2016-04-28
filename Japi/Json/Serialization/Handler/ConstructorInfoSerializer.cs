using System;
using System.Linq;
using System.Reflection;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class ConstructorInfoSerializer : IJSonSerializationHandler {

        public JsonNode Serialize(object value) {
            ConstructorInfo ctor = (ConstructorInfo)value;
            return new JsonObject {
                ["host"] = JsonSerializer.Write(ctor.DeclaringType),
                ["parameters"] = new JsonArray(ctor.GetParameters().Select(p => JsonSerializer.Write(p.ParameterType)))
            };
        }

        public object Deserialize(JsonNode json) {
            Type host = JsonSerializer.Read<Type>(json["host"]);
            Type[] arguments = json["parameters"].Select(JsonSerializer.Read<Type>).ToArray();

            foreach(ConstructorInfo ctor in host.GetConstructors()) {
                if(CompareArguments(ctor.GetParameters(), arguments))
                    return ctor;
            }
            throw new ArgumentException("Constructor not found");
        }

        bool CompareArguments(ParameterInfo[] parameters, Type[] types) {
            if(parameters.Length != types.Length)
                return false;

            for(int i = 0; i < parameters.Length; ++i)
                if(parameters[i].ParameterType != types[i])
                    return false;
            return true;
        }
    }
}