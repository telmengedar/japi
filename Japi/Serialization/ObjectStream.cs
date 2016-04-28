using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GoorooMania.Japi.Serialization.Data;

namespace GoorooMania.Japi.Serialization {

    /// <summary>
    /// stream for java serialized data
    /// </summary>
    public class ObjectStream {
        readonly JavaSerializationStream stream;
        const ushort MAGIC = 0xaced;
        const ushort VERSION = 5;

        readonly List<IJavaData> references = new List<IJavaData>(); 

        /// <summary>
        /// creates a new object stream
        /// </summary>
        /// <param name="stream">source stream containing java serialized data</param>
        public ObjectStream(Stream stream) {
            this.stream = new JavaSerializationStream(stream);
            ReadHeader();
        }

        ClassDescriptor CreateObjectDescriptor() {
            ClassDescriptor descriptor = new ClassDescriptor();
            CreateReference(descriptor);
            return descriptor;
        }

        T GetReference<T>(int handle)
            where T : IJavaData
        {
            return (T)references[handle&0xFFFF];
        }

        T GetReferenceValue<T>(int handle) {
            return (T)GetReference<JavaValue>(handle).Value;
        }

        void CreateReference(IJavaData data) {
            references.Add(data);
        }

        void ReadHeader() {
            ushort magic = stream.ReadUShort();
            if(magic != MAGIC)
                throw new StreamCorruptedException($"MAGIC not correct ({magic.ToString("X4")})");
            short version = stream.ReadShort();
            if(version != VERSION)
                throw new StreamCorruptedException($"Stream version not correct ({version})");
        }

        /// <summary>
        /// reads data from the object stream
        /// </summary>
        /// <returns></returns>
        public IJavaData ReadObject() {
            Tag tag = stream.ReadTag();
            switch(tag) {
            case Tag.NULL:
                return new JavaValue(null);
            case Tag.REFERENCE:
                return GetReference<IJavaData>(stream.ReadInt());
            case Tag.CLASSDESC:
            case Tag.PROXYCLASSDESC:
                ReadClassDesc();
                return null;
            case Tag.CLASS:
                return ReadClass();
            case Tag.OBJECT:
                return ReadInstance();
            case Tag.STRING:
            case Tag.LONGSTRING:
                return ReadString(tag);
            case Tag.ARRAY:
                return ReadArray();
            case Tag.BLOCKDATA:
                return new JavaValue(ReadBlock(stream.ReadByte()).ToArray());
            case Tag.BLOCKDATALONG:
                return new JavaValue(ReadBlock(stream.ReadInt()).ToArray());
            case Tag.EXCEPTION:
                return ReadException();
            case Tag.ENUM:
                return ReadEnum();
            default:
                throw new StreamCorruptedException("Invalid Tag" + tag.ToString("X2"));
            }
        }

        JavaException ReadException() {
            references.Clear();
            JavaException exception = new JavaException(ReadObject());
            references.Clear();
            return exception;
        }

        JavaValue ReadClass() {
            ReadClassDesc();
            JavaValue value = new JavaValue("TODO: class instance");
            CreateReference(value);
            return value;
        }

        JavaArray ReadArray() {
            ClassDescriptor descriptor = ReadClassDesc();
            JavaArray array = new JavaArray();
            CreateReference(array);

            Func<IJavaData> reader;
            switch (descriptor.Name.Substring(1))
            {
                case DataType.Integer:
                    reader = () => new JavaValue(stream.ReadInt());
                    break;
                case DataType.Byte:
                    reader = () => new JavaValue(stream.ReadByte());
                    break;
                case DataType.Long:
                    reader = () => new JavaValue(stream.ReadLong());
                    break;
                case DataType.Float:
                    reader = () => new JavaValue(stream.ReadFloat());
                    break;
                case DataType.Double:
                    reader = () => new JavaValue(stream.ReadDouble());
                    break;
                case DataType.Short:
                    reader = () => new JavaValue(stream.ReadShort());
                    break;
                case DataType.Char:
                    reader = () => new JavaValue(stream.ReadChar());
                    break;
                case DataType.Boolean:
                    reader = () => new JavaValue(stream.ReadBool());
                    break;
                default:
                    reader = ReadObject;
                    break;
            }

            int length = stream.ReadInt();
            while(length-- > 0)
                array.Items.Add(reader());
            return array;
        }


        JavaObject ReadInstance() {
            ClassDescriptor descriptor = ReadClassDesc();
            JavaObject @object = new JavaObject(descriptor.Name);
            CreateReference(@object);
            ReadInstanceData(descriptor, @object);
            return @object;
        }

        void ReadInstanceData(ClassDescriptor descriptor, JavaObject @object) {
            if(descriptor.Base != null)
                ReadInstanceData(descriptor.Base, @object);

            if(descriptor.Flags.HasFlag(ClassFlags.SERIALIZABLE)) {
                if(descriptor.Flags.HasFlag(ClassFlags.EXTERNALIZABLE))
                    throw new StreamCorruptedException("Descriptor can't be serializable and externizable");
                @object.Fields.AddRange(ReadFieldData(descriptor));
                if(descriptor.Flags.HasFlag(ClassFlags.WRITE_METHOD))
                    @object.Custom.AddRange(ReadCustomData());
            }
            else if (descriptor.Flags.HasFlag(ClassFlags.EXTERNALIZABLE))
            {
                //if (descriptor.Flags.HasFlag(ClassFlags.BLOCK_DATA))
                //    throw new NotSupportedException("External non block data not supported");
                @object.Custom.AddRange(ReadCustomData());
            }
        }

        IEnumerable<JavaField> ReadFieldData(ClassDescriptor descriptor) {
            foreach(FieldDescriptor field in descriptor.Fields) {
                switch(field.Type) {
                case DataType.Boolean:
                    yield return new JavaField(field.Name, new JavaValue(stream.ReadBool()));
                    break;
                case DataType.Char:
                    yield return new JavaField(field.Name, new JavaValue(stream.ReadChar()));
                    break;
                case DataType.Byte:
                    yield return new JavaField(field.Name, new JavaValue(stream.ReadByte()));
                    break;
                case DataType.Float:
                    yield return new JavaField(field.Name, new JavaValue(stream.ReadFloat()));
                    break;
                case DataType.Double:
                    yield return new JavaField(field.Name, new JavaValue(stream.ReadDouble()));
                    break;
                case DataType.Short:
                    yield return new JavaField(field.Name, new JavaValue(stream.ReadShort()));
                    break;
                case DataType.Integer:
                    yield return new JavaField(field.Name, new JavaValue(stream.ReadInt()));
                    break;
                case DataType.Long:
                    yield return new JavaField(field.Name, new JavaValue(stream.ReadLong()));
                    break;
                default:
                    yield return new JavaField(field.Name, ReadObject());
                    break;
                }
            }
        }

        JavaValue ReadEnum() {
            ReadClassDesc();
            JavaValue data = new JavaValue(stream.ReadString());
            CreateReference(data);
            return data;
        }

        ClassDescriptor ReadClassDesc() {
            switch(stream.ReadTag()) {
            case Tag.NULL:
                // no more classes
                return null;
            case Tag.REFERENCE:
                return GetReference<ClassDescriptor>(stream.ReadInt());
            case Tag.PROXYCLASSDESC:
                return ReadProxyDesc();
            case Tag.CLASSDESC:
                return ReadNonProxyDesc();
            default:
                throw new StreamCorruptedException("Invalid Tag");
            }
        }

        ClassDescriptor ReadNonProxyDesc() {
            ClassDescriptor descriptor = ReadClassDescriptor();
            descriptor.Custom = ReadCustomData().ToArray();
            descriptor.Base = ReadClassDesc();
            return descriptor;
        }

        ClassDescriptor ReadClassDescriptor() {
            ClassDescriptor descriptor = CreateObjectDescriptor();
            descriptor.Name = stream.ReadString();
            descriptor.SID = stream.ReadLong();
            descriptor.Flags = (ClassFlags)stream.ReadByte();
            if((descriptor.Flags & (ClassFlags.SERIALIZABLE | ClassFlags.EXTERNALIZABLE)) == (ClassFlags.SERIALIZABLE | ClassFlags.EXTERNALIZABLE))
                throw new StreamCorruptedException("Class can't be serializable and externalizable");

            bool enumeration = (descriptor.Flags & ClassFlags.ENUM) != 0;
            if(enumeration && descriptor.SID != 0)
                throw new StreamCorruptedException("Enum with non zero version id is invalid");

            descriptor.Fields = ReadFields().ToArray();
            return descriptor;
        }

        IEnumerable<FieldDescriptor> ReadFields() {
            int fields = stream.ReadShort();
            while(fields-- > 0) {
                FieldDescriptor field = new FieldDescriptor();
                string type = ((char)stream.ReadByte()).ToString(CultureInfo.InvariantCulture);
                field.Name = stream.ReadString();
                field.Type = type == "L" || type == "[" ? ReadSignature() : type;
                yield return field;
            }
        }

        string ReadSignature() {
            switch(stream.ReadTag()) {
            case Tag.REFERENCE:
                return GetReferenceValue<string>(stream.ReadInt());
            case Tag.STRING:
                return (string)ReadString(Tag.STRING).Value;
            case Tag.LONGSTRING:
                return (string)ReadString(Tag.LONGSTRING).Value;
            default:
                throw new StreamCorruptedException("Invalid Type code " + (Tag)stream.PeekByte());
            }
        }

        JavaValue ReadString(Tag tag) {
            string value;
            switch(tag) {
            case Tag.STRING:
                value = stream.ReadString();
                break;
            case Tag.LONGSTRING:
                value = stream.ReadLongString();
                break;
            default:
                throw new StreamCorruptedException("Invalid Type code.");
            }
            JavaValue data = new JavaValue(value);
            CreateReference(data);
            return data;
        }

        ClassDescriptor ReadProxyDesc() {
            ClassDescriptor descriptor = CreateObjectDescriptor();
            descriptor.Name = "Proxy";
            descriptor.Interfaces = ReadInterfaces().ToArray();
            descriptor.Custom = ReadCustomData().ToArray();
            descriptor.Base = ReadClassDesc();
            return descriptor;
        }

        IEnumerable<string> ReadInterfaces() {
            int interfacecount = stream.ReadInt();
            while(interfacecount-- > 0)
                yield return stream.ReadString();            
        } 

        IEnumerable<IJavaData> ReadCustomData() {
            bool block = true;
            while(block) {
                switch(stream.PeekTag()) {
                case Tag.RESET:
                    stream.ReadTag();
                    references.Clear();
                    continue;
                case Tag.ENDBLOCKDATA:
                    stream.ReadTag();
                    block = false;
                    continue;
                }

                yield return ReadObject();
            }
        }

        IEnumerable<byte> ReadBlock(int size) {
            while(size-- > 0)
                yield return stream.ReadByte();
        }
    }
}