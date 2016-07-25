using System;
using System.Linq;
using System.Reflection;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="ConstructorInfo"/>s
    /// </summary>
    public class ConstructorInfoSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="ConstructorInfoSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public ConstructorInfoSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            ConstructorInfo ctor = (ConstructorInfo)value;
            return new JsonObject {
                ["host"] = serializer.Write(ctor.DeclaringType),
                ["parameters"] = new JsonArray(ctor.GetParameters().Select(p => serializer.Write(p.ParameterType)))
            };
        }

        public object Deserialize(JsonNode json) {
            Type host = serializer.Read<Type>(json["host"]);
            Type[] arguments = json["parameters"].Select(serializer.Read<Type>).ToArray();

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