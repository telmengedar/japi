using System;
using System.Collections.Generic;
using NightlyCode.Japi.Json;
using NUnit.Framework;

namespace NightlyCode.Japi.Tests {

    [TestFixture]
    public class JsonExtensionTests {

        [Test]
        public void FormatJson([ValueSource(typeof(Resources), nameof(Resources.JSon))]ResourceData<string> resource) {
            JsonNode json = JSON.ReadNodeFromString(resource.Data);
            Console.WriteLine(json.Format());
        }
    }
}