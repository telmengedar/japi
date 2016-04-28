using System;
using System.Linq.Expressions;

namespace GoorooMania.Japi.Json.Serialization.Handler {
    public class MemberBindingSerializer : IJSonSerializationHandler {
        public JsonNode Serialize(object value) {
            throw new NotImplementedException("use specific serializers");
        }

        public object Deserialize(JsonNode json) {
            switch(json.SelectValue<string>("type")) {
                case "assigment":
                    return JsonSerializer.Read<MemberAssignment>(json);
                case "member":
                    return JsonSerializer.Read<MemberMemberBinding>(json);
                case "list":
                    return JsonSerializer.Read<MemberListBinding>(json);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}