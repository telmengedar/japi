using System.Collections.Generic;
using System.Linq.Expressions;

namespace NightlyCode.Japi.Json.Expressions {

    /// <summary>
    /// interface for a serializer of specific expression types
    /// </summary>
    public interface ISpecificExpressionSerializer {

        /// <summary>
        /// serializes an expression to json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="expression"></param>
        void Serialize(JsonObject json, Expression expression);

        /// <summary>
        /// deserializes an expression from json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        Expression Deserialize(JsonObject json);

        /// <summary>
        /// get supported expression types
        /// </summary>
        IEnumerable<ExpressionType> Supported { get; } 
    }
}