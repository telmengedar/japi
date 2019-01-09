using System.Collections.Generic;

namespace NightlyCode.Japi.Json
{

    /// <summary>
    /// value in json format
    /// </summary>
    public class JsonValue : JsonNode {
        readonly object value;

        /// <summary>
        /// creates a new json entry
        /// </summary>
        /// <param name="value"></param>
        public JsonValue(object value) {
            this.value = value;
        }

        /// <inheritdoc />
        protected override JsonNode GetNode(string key) {
            throw new JsonException("named access only valid for Json Objects");
        }

        /// <inheritdoc />
        protected override JsonNode GetNode(int index) {
            throw new JsonException("Indexed access only valid for arrays");
        }

        /// <inheritdoc />
        public override object Value => value;

        /// <inheritdoc />
        public override IEnumerator<JsonNode> GetEnumerator() {
            throw new JsonException("Enumerator only available for arrays");
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Value}";
        }
    }
}