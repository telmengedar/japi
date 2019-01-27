
using NightlyCode.Japi.Json;
using NUnit.Framework;

namespace NightlyCode.Japi.Tests {
    [TestFixture, Parallelizable]
    public class JsonNodeTests {

        [Test, Parallelizable]
        public void SelectValue() {
            string data = "{\"t\":null,\"s\":null,\"op\":10,\"d\":{\"heartbeat_interval\":41250,\"_trace\":[\"gateway-prd-main-6lhw\"]}}";
            JsonNode json = JSON.ReadNodeFromString(data);
            Assert.AreEqual(41250, json.SelectSingle<int>("d/heartbeat_interval"));
        }
    }
}