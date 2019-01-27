using System.Security.Policy;
using NightlyCode.Japi.Extensions;
using NightlyCode.Japi.Json;
using NUnit.Framework;

namespace NightlyCode.Japi.Tests {

    [TestFixture, Parallelizable]
    public class SelectTests {

        [Test, Parallelizable]
        public void SelectValue() {
            JsonObject json = JSON.ReadNodeFromString("{\"created_folder\":\"cloudflow://PP_FILE_STORE/Incoming/test/\",\"messages\":[{\"severity\":\"warning\",\"type\":\"URL does not end on slash but references folder\",\"description\":\"URL cloudflow://PP_FILE_STORE/Incoming does not end on slash but references folder\"}],\"error_code\":\"URL does not end on slash but references folder\",\"error\":\"URL cloudflow://PP_FILE_STORE/Incoming does not end on slash but references folder\"}") as JsonObject;

            Assert.AreEqual("cloudflow://PP_FILE_STORE/Incoming/test/", json.SelectValue<string>("created_folder"));
        }

        [Test, Parallelizable]
        public void SelectNonExistingPath()
        {
            JsonObject json = JSON.ReadNodeFromString("{}") as JsonObject;

            Assert.AreEqual(null, json.SelectSingle<string>("created_folder"));
        }
    }
}