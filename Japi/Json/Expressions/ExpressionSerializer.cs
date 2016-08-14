using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NightlyCode.Japi.Json.Serialization;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// serialization handler for expressions
    /// </summary>
    public class ExpressionSerializer : JSonSerializationHandler<Expression> {
        readonly IJsonSerializer serializer;
        readonly Dictionary<ExpressionType, ISpecificExpressionSerializer> serializers = new Dictionary<ExpressionType, ISpecificExpressionSerializer>();

        /// <summary>
        /// creates a new expression serializer
        /// </summary>
        public ExpressionSerializer(IJsonSerializer serializer) {
            this.serializer = serializer;
#if !UNITY
            AddSerializer(new BlockExpressionSerializer(serializer));
            AddSerializer(new DebugInfoExpressionSerializer(serializer));
            AddSerializer(new DefaultExpressionSerializer(serializer));
            AddSerializer(new LabelExpressionSerializer(serializer));
            AddSerializer(new LoopExpressionSerializer(serializer));
            AddSerializer(new GotoExpressionSerializer(serializer));
            AddSerializer(new RuntimeVariablesExpressionSerializer(serializer));
            AddSerializer(new SwitchExpressionSerializer(serializer));
            AddSerializer(new TryExpressionSerializer(serializer));
            AddSerializer(new DynamicExpressionSerializer(serializer));
            AddSerializer(new IndexExpressionSerializer(serializer));
#endif
            AddSerializer(new BinaryExpressionSerializer(serializer));
            AddSerializer(new ConditionalExpressionSerializer(serializer));
            AddSerializer(new ConstantExpressionSerializer(serializer));
            AddSerializer(new LambdaExpressionSerializer(serializer));
            AddSerializer(new InvocationExpressionSerializer(serializer));
            AddSerializer(new UnaryExpressionSerializer(serializer));
            AddSerializer(new ListInitExpressionSerializer(serializer));
            AddSerializer(new MemberExpressionSerializer(serializer));
            AddSerializer(new MemberInitExpressionSerializer(serializer));
            AddSerializer(new MethodCallExpressionSerializer(serializer));
            AddSerializer(new NewArrayExpressionSerializer(serializer));
            AddSerializer(new NewExpressionSerializer(serializer));
            AddSerializer(new ParameterExpressionSerializer(serializer));
            AddSerializer(new TypeBinaryExpressionSerializer(serializer));
        }

        void AddSerializer(ISpecificExpressionSerializer specificserializer) {
            foreach(ExpressionType type in specificserializer.Supported)
                serializers[type] = specificserializer;
        }

        /// <summary>
        /// serializes a value to json
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public sealed override JsonNode SerializeValue(Expression value) {
            JsonObject json = new JsonObject
            {
                ["type"] = serializer.Write(value.NodeType),
            };

            serializers[value.NodeType].Serialize(json, value);

            return json;
        }

        /// <summary>
        /// deserializes a json structure to a value
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public sealed override Expression DeserializeValue(JsonNode json) {
            if(!(json is JsonObject))
                throw new InvalidOperationException();

            
            JsonObject @object = (JsonObject)json;
            return serializers[json.SelectValue<ExpressionType>("type")].Deserialize(@object);
        }

    }
}