using System;
using System.Linq;
using System.Linq.Expressions;
using GoorooMania.Japi.Json;
using NUnit.Framework;

namespace GoorooMania.Japi.Tests {

    [TestFixture]
    public class JsonTests {

        [Test]
        public void SerializeAndDeserialize() {
            TestObject test = TestObject.TestData;
            JsonNode json = JsonSerializer.Write(test);
            string jsondata = JsonWriter.WriteString(json);
            JsonNode otherjson = JsonWriter.Read(jsondata);
            TestObject othertest = JsonSerializer.Read<TestObject>(otherjson);
            Assert.AreEqual(test, othertest);
        }

        [Test]
        public void ExpressionTest() {
            Expression<Func<TestObject, bool>> expression = o => o.Data == "Dufte" && o.Integer != 3 || o.Float > 2.9f;
            JsonNode json = JsonSerializer.Write(expression);
            string jsondata = JsonWriter.WriteString(json);
            JsonNode otherjson = JsonWriter.Read(jsondata);
            Expression<Func<TestObject, bool>> othertest = JsonSerializer.Read<Expression<Func<TestObject, bool>>>(otherjson);
            Assert.AreEqual(expression.ToString(), othertest.ToString());
        }

        [Test]
        public void EnumerationExtensionTest() {
            int[] array = {1, 2, 3};
            Expression<Func<TestObject, bool>> expression = o => array.Contains(o.Integer);
            string json = JSON.WriteString(expression);
            Expression<Func<TestObject, bool>> other = JSON.Read<Expression<Func<TestObject, bool>>>(json);
            Func<TestObject, bool> compiled = other.Compile();
            Assert.That(compiled(new TestObject() {
                Integer = 1
            }));
            Assert.That(compiled(new TestObject()
            {
                Integer = 2
            }));
            Assert.That(compiled(new TestObject()
            {
                Integer = 3
            }));
            Assert.That(!compiled(new TestObject()
            {
                Integer = 4
            }));
            //Assert.AreEqual(expression.ToString(), other.ToString());
        }

        [Test]
        public void ReadJson([ValueSource(typeof(Resources), nameof(Resources.JSon))]ResourceData<string> data) {
            string json = data.Data;
            JsonWriter.Read(json);
            Assert.Pass();
        }
    }
}