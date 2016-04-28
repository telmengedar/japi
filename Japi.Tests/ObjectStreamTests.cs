using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using GoorooMania.Japi.Serialization.Readers;
using NUnit.Framework;

namespace GoorooMania.Japi.Tests {

    [TestFixture]
    public class ObjectStreamTests {

        byte[] GetBytes(Stream data) {
            List<byte> bytes = new List<byte>();
            data.ReadByte();
            data.ReadByte();
            data.ReadByte();
            while(data.Position < data.Length) {
                char upper = (char)(byte)data.ReadByte();
                char lower = (char)(byte)data.ReadByte();
                bytes.Add(byte.Parse(upper.ToString() + lower, NumberStyles.HexNumber));
            }
            return bytes.ToArray();
        }

        [Test]
        public void DeserializeEncodedData([ValueSource(typeof(Resources), nameof(Resources.Encoded))]ResourceData<Stream> data) {
            byte[] bytes;
            using(data.Data)
                bytes = GetBytes(data.Data);

            using(MemoryStream ms = new MemoryStream(bytes)) {
                XmlObjectReader reader = new XmlObjectReader(ms);
                XmlDocument document = reader.Read();
                Assert.NotNull(document, "No data read");
                Assert.AreEqual(ms.Position, ms.Length, "Bytes left in stream");
            }
        }

        [Test]
        public void DeserializeRawData([ValueSource(typeof(Resources), nameof(Resources.Raw))] ResourceData<Stream> data) {
            using(data.Data) {
                XmlObjectReader reader = new XmlObjectReader(data.Data);
                while(reader.ContainsData) {
                    XmlDocument document = reader.Read();
                    Assert.NotNull(document, "No document read");
                }
                Assert.AreEqual(data.Data.Position, data.Data.Length, "Bytes left in stream");
            }
        }

        [Test]
        public void ReadBase64Orders([ValueSource(typeof(Resources), nameof(Resources.Base64))] ResourceData<byte[]> data) {
            using(MemoryStream ms = new MemoryStream(data.Data)) {
                XmlObjectReader reader = new XmlObjectReader(ms);
                XmlDocument document = reader.Read();
                Assert.NotNull(document);
            }
        }
    }
}