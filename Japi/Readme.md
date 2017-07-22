# JAPI

Java Interaction Library. This Library contains classes used to interact with java data. This includes deserialization of Java Object Streams and Json data manipulation.

## Java Object Streams

Initially this project was created to be able to read Java Object Streams in C#.
[Java Object Streams](https://docs.oracle.com/javase/7/docs/platform/serialization/spec/protocol.html) are a somehow well documented format which is used by Java for binary serialization of objects. Some Java applications are using this format to interact with each other.

### How to read a Java Object Stream

The following code example reads a java object stream and deserializes it into an XmlDocument.

```
Stream javastream; // this is the source stream providing the object stream data

using(javastream) {
    XmlObjectReader reader = new XmlObjectReader(javastream);
    while(reader.ContainsData) {
        XmlDocument document = reader.Read();
		// do something with the data
		// when the stream contains multiple serialized objects there might be more than one document
		// containing the deserialized data
    }
}
```

## Json

This library contains a Json parser and serializer to be able to read and write json data. If you are using some different library like [Newtonsoft JSON.NET](https://github.com/JamesNK/Newtonsoft.Json) there is no need to try out this project since it surprise me if my code did any better.

### How to deserialize JSON

```
T data=JSON.Read<T>(stream);
```

This reads an object of type <T> from the stream. The parser loops over every **public writable** property and tries to find a key in the json structure and deserializes the corresponding value. The key in json is expected to be the propertyname in all **lowercase**.
If the key should be something different this information is to be annotated at the property using the [JsonKey](Japi/Json/JsonKeyAttribute.cs).

### Example

The following class:

```
public class TestObject {
	public int IntegerProperty { get; set; }
	public string StringProperty { get; set; }
	public double[] DoubleArray { get; set; }
}
```

Can be deserialized using the following json structure:

```
{
	"integerproperty" : 42,
	"stringproperty" : "teststring",
	"doublearray" : [4.9,3.2,903.2]
}
```

by using the following line in code:

```
TestObject data=JSON.Read<TestObject>(stream);
```

### How to serialize JSON

```
Stream outstream; // stream to write json to
TestObject data=new TestObject(...); // object to be serialized

using(outstream)
{
	JSON.Write(data, outstream)
}
```

The serialization logic basically is the same as noted in the deserialization section. The resulting json will contain all properties of the object. The json keys are the property names in all **lowercase**.