using System.IO;
using System.Xml;
using NightlyCode.Japi.Serialization.Readers;
using NUnit.Framework;

namespace NightlyCode.Japi.Tests {

    [TestFixture]
    public class ObjectStreamTests {

        [Test]
        public void DeserializeRawData([ValueSource(typeof(Resources), nameof(Resources.ObjectStreams))] ResourceData<Stream> data) {
            using(data.Data) {
                XmlObjectReader reader = new XmlObjectReader(data.Data);
                while(reader.ContainsData) {
                    XmlDocument document = reader.Read();
                    Assert.NotNull(document, "No document read");
                }
                Assert.AreEqual(data.Data.Position, data.Data.Length, "Bytes left in stream");
            }
        }
    }
}