using System;
using System.Linq.Expressions;
using NightlyCode.Japi.Extensions;

namespace NightlyCode.Japi.Json.Serialization.Handler {

    /// <summary>
    /// serializes <see cref="MemberBinding"/>s
    /// </summary>
    /// <remarks>
    /// actually just deserializes them
    /// </remarks>
    public class MemberBindingSerializer : IJSonSerializationHandler {
        readonly IJsonSerializer serializer;

        /// <summary>
        /// creates a new <see cref="MemberBindingSerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public MemberBindingSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
        }

        public JsonNode Serialize(object value) {
            throw new NotImplementedException("use specific serializers");
        }

        public object Deserialize(JsonNode json) {
            switch(json.SelectValue<string>("type")) {
                case "assigment":
                    return serializer.Read<MemberAssignment>(json);
                case "member":
                    return serializer.Read<MemberMemberBinding>(json);
                case "list":
                    return serializer.Read<MemberListBinding>(json);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}