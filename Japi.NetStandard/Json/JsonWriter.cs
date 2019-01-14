using System;
using System.Globalization;
using System.IO;
using System.Text;
using NightlyCode.Japi.Extern;

namespace NightlyCode.Japi.Json
{
    /// <summary>
    /// writes and reads <see cref="JsonNode"/>s
    /// </summary>
    internal class JsonWriter : IJsonWriter {

        /// <summary>
        /// reads json data
        /// </summary>
        /// <param name="data">string which contains a json structure</param>
        /// <returns><see cref="JsonNode"/> containing the read json data</returns>
        public JsonNode Read(string data) { 
            using(StringReader reader = new StringReader(data)) {
                return Read(reader);
            }
        }

        /// <summary>
        /// reads json data
        /// </summary>
        /// <param name="data">data which contains a json structure</param>
        /// <returns><see cref="JsonNode"/> containing the read json data</returns>
        public JsonNode Read(byte[] data) {
            using(MemoryStream ms = new MemoryStream(data))
                return Read(ms);
        }

#if FRAMEWORK35
        /// <summary>
        /// reads json data
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public JsonNode Read(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true, 4096))
                return Read(reader);
        }
#else
        /// <summary>
        /// reads json data
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="leaveopen">whether to leave stream open after reading</param>
        /// <returns></returns>
        public JsonNode Read(Stream stream, bool leaveopen=false) {
            using(StreamReader reader = new StreamReader(stream, Encoding.UTF8, true, 4096, leaveopen))
                return Read(reader);
        }
#endif

        JsonNode Read(TextReader reader) {
            SkipWhiteSpaces(reader);
            return ReadValue(reader);
        }

        char PeekCharacter(TextReader sr) {
            int read = sr.Peek();
            if(read == -1)
                throw new JsonException("unexpected stream end");
            return (char)read;
        }

        char ReadCharacter(TextReader sr) {
            int read = sr.Read();
            if(read == -1)
                throw new JsonException("unexpected stream end");
            return (char)read;
        }

        JsonObject ReadDictionary(TextReader reader) {
            JsonObject json = new JsonObject();
            do {
                SkipWhiteSpaces(reader);
                char character = PeekCharacter(reader);
                if(character == '}') {
                    ReadCharacter(reader);
                    return json;
                }

                string key = ReadKey(reader);
                SkipWhiteSpaces(reader);
                character = ReadCharacter(reader);
                if(character != ':')
                    throw new InvalidOperationException($"unexpected character '{character}'");

                SkipWhiteSpaces(reader);
                JsonNode value = ReadValue(reader);
                json[key] = value;
                SkipWhiteSpaces(reader);

                character = ReadCharacter(reader);
                if(character == '}')
                    return json;

                if(character != ',')
                    throw new InvalidOperationException($"unexpected character '{character}'");
            } while(true);
        }

        JsonNode ReadValue(TextReader reader) {
            char character = PeekCharacter(reader);
            switch(character) {
                case '{':
                    reader.Read();
                    return ReadDictionary(reader);
                case '[':
                    reader.Read();
                    return ReadArray(reader);
                case '\'':
                case '\"':
                    reader.Read();
                    return new JsonValue(ReadString(reader, character));
                default:
                    return new JsonValue(ReadValueType(reader));
            }
        }

        JsonArray ReadArray(TextReader reader) {
            JsonArray json = new JsonArray();

            SkipWhiteSpaces(reader);
            char character = PeekCharacter(reader);
            if(character == ']') {
                ReadCharacter(reader);
                return json;
            }

            do {
                SkipWhiteSpaces(reader);
                json.Add(ReadValue(reader));

                SkipWhiteSpaces(reader);
                character = ReadCharacter(reader);
                if(character == ']')
                    return json;
                if(character == ',')
                    continue;

                throw new InvalidOperationException("invalid array specification");
            } while(true);
        }

        string ReadString(TextReader reader, char delimiter) {
            StringBuilder buffer = new StringBuilder();
            do {
                char character = ReadCharacter(reader);

                if(character == delimiter)
                    return buffer.ToString();

                if(character == '\\') {
                    character = ReadCharacter(reader);
                    switch(character) {
                    case '\\':
                        buffer.Append('\\');
                        break;
                    case '\'':
                        buffer.Append('\'');
                        break;
                    case '\"':
                        buffer.Append('\"');
                        break;
                    case 'u':
                        buffer.Append(ReadUnicodeCharacter(reader));
                        break;
                    case 'r':
                        buffer.Append('\r');
                        break;
                    case 'n':
                        buffer.Append('\n');
                        break;
                    default:
                        buffer.Append(character);
                        break;
                    }
                }
                else
                    buffer.Append(character);
            } while(true);
        }

        char ReadUnicodeCharacter(TextReader reader) {
            char[] characters = new char[4];
            reader.Read(characters, 0, 4);
            return (char)int.Parse(new string(characters), NumberStyles.HexNumber);
        }

        object ReadValueType(TextReader reader) {
            StringBuilder buffer = new StringBuilder();

            do {
                char character = PeekCharacter(reader);
                if(char.IsWhiteSpace(character) || character == ',' || character == '}' || character == ']')
                    break;

                reader.Read();
                buffer.Append(character);
            }
            while(reader.Peek() != -1);

            string value = buffer.ToString().ToLower();
            switch(value) {
                case "null":
                    return null;
                case "true":
                    return true;
                case "false":
                    return false;
            }

            // strangely this produces doubles if the expression is simplified ... even for integers
            // makes absolutely no sense ...
            if(value.Contains("."))
                return double.Parse(value, CultureInfo.InvariantCulture);

            // there are no differences between int and long except long allows for a larger range of numbers
            // if someone pushes something like ticks in these values parsing it with int could crash
            try {
                return long.Parse(value);
            }
            catch(Exception e) {

                throw new JsonException($"Unable to parse '{value}'", e);
            }
            
        }

        string ReadKey(TextReader reader) {
            StringBuilder key = new StringBuilder();

            char delimiter = PeekCharacter(reader);
            if(delimiter == '\"' || delimiter == '\'') {
                reader.Read();
                return ReadString(reader, delimiter);
            }

            do {
                char character = PeekCharacter(reader);
                if(character == ':' || char.IsWhiteSpace(character))
                    return key.ToString();

                reader.Read();
                key.Append(character);
            } while(true);
        }

        void SkipWhiteSpaces(TextReader reader) {
            do {
                int read = reader.Peek();
                if(read == -1)
                    throw new InvalidOperationException("unexpected stream end");

                char character = (char)read;
                if(char.IsWhiteSpace(character))
                    reader.Read();
                else
                    return;
            }
            while(true);
        }

        /// <summary>
        /// writes a json node to string
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public string WriteString(JsonNode node) {
            using (MemoryStream ms = new MemoryStream())
            {
                Write(node, ms);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

#if FRAMEWORK35
        /// <summary>
        /// writes a json node to a stream
        /// </summary>
        /// <param name="node"></param>
        /// <param name="target"></param>
        public void Write(JsonNode node, Stream target)
        {
            using (TextWriter writer = new StreamWriter(target, new UTF8Encoding(false), 4096))
                Write(node, writer);
        }
#else
        /// <summary>
        /// writes a json node to a stream
        /// </summary>
        /// <param name="node"></param>
        /// <param name="target"></param>
        /// <param name="leaveopen">whether to leave the stream open after data has been written</param>
        public void Write(JsonNode node, Stream target, bool leaveopen=false) {
            using(TextWriter writer = new StreamWriter(target, new UTF8Encoding(false),4096, leaveopen))
                Write(node, writer);
        }
#endif


        void Write(JsonNode node, TextWriter writer) {
            if(node is JsonArray)
                WriteArray((JsonArray)node, writer);
            else if(node is JsonObject)
                WriteObject((JsonObject)node, writer);
            else if(node is JsonValue)
                WriteValue((JsonValue)node, writer);
        }

        /// <summary>
        /// serializes the object to string
        /// </summary>
        /// <returns></returns>
        void WriteObject(JsonObject json, TextWriter writer) {
            writer.Write("{");
            bool flag = false;
            foreach(string key in json.Keys) {
                if(flag)
                    writer.Write(",");
                else flag = true;

                Serialize(key, json.GetNode(key), writer);
            }
            writer.Write("}");
        }

        void WriteArray(JsonArray array, TextWriter writer) {
            writer.Write("[");
            bool flag = false;
            if(array.Count > 0) {
                foreach(JsonNode item in array) {
                    if(flag)
                        writer.Write(",");
                    else flag = true;
                    Write(item, writer);
                }
            }
            writer.Write("]");
        }

        void Serialize(string key, JsonNode value, TextWriter writer) {
            writer.Write("\"");
            writer.Write(key);
            writer.Write("\"");
            writer.Write(":");
            Write(value, writer);
        }

        void WriteValue(JsonValue jsonvalue, TextWriter writer) {
            object value = jsonvalue.Value;
            if(value is Version)
                value = value.ToString();
            else if(value is Enum)
                value = Converter.Convert(value, Enum.GetUnderlyingType(value.GetType()));
            else if (value is DateTime)
                value = ((DateTime)value).Ticks;
            else if(value is TimeSpan)
                value = ((TimeSpan)value).Ticks;
            else if(value is byte[])
                value = Convert.ToBase64String((byte[])value);
            else if(value is IntPtr)
                value = IntPtr.Size > 4 ? ((IntPtr)value).ToInt64() : ((IntPtr)value).ToInt32();
            else if(value is UIntPtr)
                value = UIntPtr.Size > 4 ? ((UIntPtr)value).ToUInt64() : ((UIntPtr)value).ToUInt32();

            if (value == null)
                writer.Write("null");
            else if(value is string) {
                writer.Write("\"");
                writer.Write(Escape((string)value));
                writer.Write("\"");
            }
            else if(value is bool)
                writer.Write((bool)value ? "true" : "false");
            else if (value is byte)
                writer.Write(((byte)value).ToString(CultureInfo.InvariantCulture));
            else if(value is short)
                writer.Write(((short)value).ToString(CultureInfo.InvariantCulture));
            else if(value is ushort)
                writer.Write(((ushort)value).ToString(CultureInfo.InvariantCulture));
            else if(value is int)
                writer.Write(((int)value).ToString(CultureInfo.InvariantCulture));
            else if (value is uint)
                writer.Write(((uint)value).ToString(CultureInfo.InvariantCulture));
            else if (value is long)
                writer.Write(((long)value).ToString(CultureInfo.InvariantCulture));
            else if (value is ulong)
                writer.Write(((ulong)value).ToString(CultureInfo.InvariantCulture));
            else if (value is float)
                writer.Write(((float)value).ToString(CultureInfo.InvariantCulture));
            else if(value is double)
                writer.Write(((double)value).ToString(CultureInfo.InvariantCulture));
            else if(value is decimal)
                writer.Write(((decimal)value).ToString(CultureInfo.InvariantCulture));
            else
                throw new InvalidOperationException("Type not supported");
        }

        string Escape(string data) {
            StringBuilder result = new StringBuilder();
            foreach(char character in data) {
                if(character < 0x20 || character == '\\' || character == '\"')
                    result.Append("\\u" + ((int)character).ToString("x4"));
                else
                    result.Append(character);
            }
            return result.ToString();
        }
    }
}