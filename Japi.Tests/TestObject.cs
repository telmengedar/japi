using System;
using System.Linq;

namespace GoorooMania.Japi.Tests {

    public class TestObject {
        public int Integer { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public string Data { get; set; }
        public TestObject Child { get; set; }
        public TestObject[] ObjectArray { get; set; }
        public int[] IntArray { get; set; }

        bool AreEqual(Array lhs, Array rhs) {
            if(lhs == null) return rhs == null;
            if(rhs == null) return false;
            return lhs.Cast<object>().SequenceEqual(rhs.Cast<object>());
        }

        protected bool Equals(TestObject other) {
            return Integer == other.Integer && Float.Equals(other.Float) && Double.Equals(other.Double) && string.Equals(Data, other.Data) && Equals(Child, other.Child) && AreEqual(ObjectArray, other.ObjectArray) && AreEqual(IntArray, other.IntArray);
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj)) return false;
            if(ReferenceEquals(this, obj)) return true;
            if(obj.GetType() != GetType()) return false;
            return Equals((TestObject)obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = Integer;
                hashCode = (hashCode * 397) ^ Float.GetHashCode();
                hashCode = (hashCode * 397) ^ Double.GetHashCode();
                hashCode = (hashCode * 397) ^ (Data?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Child?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (ObjectArray?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (IntArray?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public static TestObject TestData => new TestObject {
            Integer = 9,
            Float = 2.4f,
            Double = 7.2,
            Data = "Test",
            IntArray = new[] {1, 2, 3, 4, 5},
            Child = new TestObject {
                Integer = 1,
                Float = 1.2f,
                Double = 2.2,
                Data = "Hello World",
            },
            ObjectArray = new[] {
                new TestObject {
                    Integer = 9,
                    Float = 9.2f,
                    Double = 3.2,
                    Data = "Zent"
                },
                new TestObject {
                    Integer = 6,
                    Float = 7.1f,
                    Double = 10.2,
                    Data = "Und"
                }
            }
        };
    }
}