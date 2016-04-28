using System.Collections.Generic;

namespace GoorooMania.Japi.Json
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

        protected override JsonNode GetNode(string key) {
            throw new JsonException("named access only valid for Json Objects");
        }

        protected override JsonNode GetNode(int index) {
            throw new JsonException("Indexed access only valid for arrays");
        }

        public override object Value => value;

        public override IEnumerator<JsonNode> GetEnumerator() {
            throw new JsonException("Enumerator only available for arrays");
        }
    }
}