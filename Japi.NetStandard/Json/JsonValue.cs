using System.Collections.Generic;

namespace NightlyCode.Japi.Json
{

    /// <summary>
    /// value in json format
    /// </summary>
    public class JsonValue : JsonNode {

        /// <summary>
        /// creates a new json entry
        /// </summary>
        /// <param name="value"></param>
        public JsonValue(object value) {
            Value = value;
        }

        /// <inheritdoc />
        public override object this[string key] {
            get => throw new JsonException("named access only valid for Json Objects");
            set => throw new JsonException("named access only valid for Json Objects");
        }

        /// <inheritdoc />
        public override object this[int index] {
            get => throw new JsonException("Indexed access only valid for arrays");
            set => throw new JsonException("Indexed access only valid for arrays");
        }

        /// <inheritdoc />
        public override object Value { get; }

        /// <inheritdoc />
        public override IEnumerator<JsonNode> GetEnumerator() {
            throw new JsonException("Enumerator only available for arrays");
        }

        /// <inheritdoc />
        public override string ToString() {
            return JSON.Writer.WriteString(this);
        }
    }
}