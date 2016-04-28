using System.Collections.Generic;
using System.Linq;
using GoorooMania.Japi.Serialization.Data.Path;
using NUnit.Framework;

namespace GoorooMania.Japi.Tests {

    [TestFixture]
    public class DataPathParserTests {
        readonly DataPathParser parser = new DataPathParser();

        IEnumerable<string> Valid
        {
            get
            {
                yield return "a";
                yield return "a/b/c";
                yield return "a[8]";
                yield return "a[8]/b/c";
                yield return "abc/d[88]/f";
                yield return "[0]";
                yield return "[0]/cbb/e";
                yield return "a[8][2][3]";
                yield return "a[0][2]/[9]";
                yield return "a[0][0]/b[2]";
            }
        }

        [Test]
        public void ParseValid([ValueSource("Valid")] string path) {
            IPathElement[] elements = parser.ParsePath(path).ToArray();
            Assert.Greater(elements.Length, 0);
        }
    }
}