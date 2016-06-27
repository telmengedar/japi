using System;
using System.Reflection;
using System.Linq;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class TypeSerializer : IJSonSerializationHandler {

        public JsonNode Serialize(object value) {
            Type type = (Type)value;
#if WINDOWS_UWP
            return new JsonObject
            {
                ["assembly"] = new JsonValue(type.GetTypeInfo().Assembly.GetName().Name),
                ["namespace"] = new JsonValue(type.Namespace),
                ["name"] = new JsonValue(type.Name)
            };
#else
            return new JsonObject {
                ["assembly"] = new JsonValue(type.Assembly.GetName().Name),
                ["namespace"] = new JsonValue(type.Namespace),
                ["name"] = new JsonValue(type.Name)
            };
#endif
        }

        public object Deserialize(JsonNode json) {
            string assemblyname = json.SelectValue<string>("assembly");

#if !WINDOWS_UWP
            // the following fixes exceptions where system.core or other system dlls are not found
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == assemblyname);
            if(assembly != null)
                return assembly.GetType(json.SelectValue<string>("namespace") + "." + json.SelectValue<string>("name"));
#endif

#if WINDOWS_UWP
            return Type.GetType(json.SelectValue<string>("namespace") + "." + json.SelectValue<string>("name"));
#else
            return Type.GetType(json.SelectValue<string>("namespace") + "." + json.SelectValue<string>("name") + ", " + assemblyname);
#endif
        }
    }
}