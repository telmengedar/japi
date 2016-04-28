using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GoorooMania.Japi.Json.Serialization;

namespace GoorooMania.Japi.Json.Expressions {

    /// <summary>
    /// serialization handler for expressions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpressionSerializer : JSonSerializationHandler<Expression> {
        readonly Dictionary<ExpressionType, ISpecificExpressionSerializer> serializers = new Dictionary<ExpressionType, ISpecificExpressionSerializer>();

        /// <summary>
        /// creates a new expression serializer
        /// </summary>
        public ExpressionSerializer() {
            AddSerializer(new BinaryExpressionSerializer());
            AddSerializer(new BlockExpressionSerializer());
            AddSerializer(new ConditionalExpressionSerializer());
            AddSerializer(new ConstantExpressionSerializer());
            AddSerializer(new DebugInfoExpressionSerializer());
            AddSerializer(new DefaultExpressionSerializer());
            AddSerializer(new IndexExpressionSerializer());
            AddSerializer(new LambdaExpressionSerializer());
            AddSerializer(new InvocationExpressionSerializer());
            AddSerializer(new UnaryExpressionSerializer());
            AddSerializer(new LabelExpressionSerializer());
            AddSerializer(new ListInitExpressionSerializer());
            AddSerializer(new LoopExpressionSerializer());
            AddSerializer(new MemberExpressionSerializer());
            AddSerializer(new GotoExpressionSerializer());
            AddSerializer(new MemberInitExpressionSerializer());
            AddSerializer(new MethodCallExpressionSerializer());
            AddSerializer(new NewArrayExpressionSerializer());
            AddSerializer(new NewExpressionSerializer());
            AddSerializer(new ParameterExpressionSerializer());
            AddSerializer(new RuntimeVariablesExpressionSerializer());
            AddSerializer(new SwitchExpressionSerializer());
            AddSerializer(new TryExpressionSerializer());
            AddSerializer(new TypeBinaryExpressionSerializer());
            AddSerializer(new DynamicExpressionSerializer());
        }

        void AddSerializer(ISpecificExpressionSerializer serializer) {
            foreach(ExpressionType type in serializer.Supported)
                serializers[type] = serializer;
        }

        /// <summary>
        /// serializes a value to json
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public sealed override JsonNode SerializeValue(Expression value) {
            JsonObject json = new JsonObject
            {
                ["type"] = JsonSerializer.Write(value.NodeType),
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