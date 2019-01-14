using NightlyCode.Japi.Extensions;
using NightlyCode.Japi.Json;
using NUnit.Framework;

namespace NightlyCode.Japi.Tests {

    [TestFixture]
    public class JsonTests {

        [Test]
        public void SerializeAndDeserialize() {
            TestObject test = TestObject.TestData;
            string jsondata = JSON.WriteString(test);
            TestObject othertest = JSON.Read<TestObject>(jsondata);
            Assert.AreEqual(test, othertest);
        }

        /*[Test]
        public void ExpressionTest() {
            Expression<Func<TestObject, bool>> expression = o => o.Data == "Dufte" && o.Integer != 3 || o.Float > 2.9f;
            string jsondata = JSON.WriteString(expression);
            Expression<Func<TestObject, bool>> othertest = JSON.Read<Expression<Func<TestObject, bool>>>(jsondata);
            Assert.AreEqual(expression.ToString(), othertest.ToString());
        }

        [Test]
        public void EnumerationExtensionTest() {
            int[] array = {1, 2, 3};
            Expression<Func<TestObject, bool>> expression = o => array.Contains(o.Integer);
            string json = JSON.WriteString(expression);
            Expression<Func<TestObject, bool>> other = JSON.Read<Expression<Func<TestObject, bool>>>(json);
        }*/

        [Test]
        public void ReadJson([ValueSource(typeof(Resources), nameof(Resources.JSon))]ResourceData<string> data) {
            string json = data.Data;
            JsonNode structure=JSON.ReadNodeFromString(json);
        }

        [Test]
        public void ReadJsonFromBytes([ValueSource(typeof(Resources), nameof(Resources.JsonBytes))]ResourceData<byte[]> data)
        {
            JSON.Writer.Read(data.Data);
        }

        [Test]
        public void JsonConstructorTest() {
            string data=JSON.Writer.WriteString(new JsonObject {
                ["test"] = "test",
                ["int"] = 42
            });

            JsonNode deserialized = JSON.Writer.Read(data);
            Assert.AreEqual("test", deserialized.SelectValue<string>("test"));
            Assert.AreEqual(42, deserialized.SelectValue<int>("int"));
        }

        [Test]
        public void DeserializeScientificNotation() {
            JsonNode json=JSON.Writer.Read("1.234567E-06");
        }

        [Test]
        public void SerializeVariant() {
            VariantClass variant = new VariantClass {
                Data = new IData[] {
                    new Data1(),
                    new Data2(),
                    new Data1(),
                    new Data1(),
                    new Data2()
                }
            };

            string data=JSON.WriteString(variant);
            VariantClass v2 = JSON.Read<VariantClass>(data);
        }
    }
}