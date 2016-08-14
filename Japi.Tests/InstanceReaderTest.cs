using NUnit.Framework;

namespace NightlyCode.Japi.Tests
{

    [TestFixture]
    public class InstanceReaderTest {

        /*[Test]
        public void ReadOrder([ValueSource(typeof(Resources), nameof(Resources.Raw))] ResourceData<Stream> resource) {
            using (resource.Data)
            {
                InstanceReader<Order> reader=new InstanceReader<Order>();
                while (reader.ContainsData)
                {
                    XmlDocument document = reader.Read();
                    Assert.NotNull(document, "No document read");
                }
            }

        }*/
    }
}