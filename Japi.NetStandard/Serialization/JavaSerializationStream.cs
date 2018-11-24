using System;
using System.IO;
using System.Text;

namespace NightlyCode.Japi.Serialization {

    /// <summary>
    /// stream for reading binary data
    /// </summary>
    public class JavaSerializationStream : Stream {
        readonly Stream basestream;

        readonly byte[] peekbuffer = new byte[1];
        bool peeked;

        /// <summary>
        /// creates a new binary stream
        /// </summary>
        /// <param name="basestream"></param>
        public JavaSerializationStream(Stream basestream) {
            this.basestream = basestream;
        }

        public override void Flush() {
            if(peeked)
                peeked = false;
        }

        public override long Seek(long offset, SeekOrigin origin) {
            throw new NotImplementedException();
        }

        public override void SetLength(long value) {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count) {
            if(count == 0)
                return 0;

            if(peeked) {
                buffer[offset++] = peekbuffer[0];
                --count;
                peeked = false;
            }

            if(count == 0)
                return 1;

            return basestream.Read(buffer, offset, count);
        }

        public Tag ReadTag()
        {
            return (Tag)ReadByte();
        }

        public Tag PeekTag()
        {
            return (Tag)PeekByte();
        }

        public byte PeekByte() {
            if(peeked) return peekbuffer[0];

            basestream.Read(peekbuffer, 0, 1);
            peeked = true;
            return peekbuffer[0];
        }

        /// <summary>
        /// reads a byte from the stream
        /// </summary>
        /// <returns></returns>
        public new byte ReadByte() {
            if(peeked) {
                peeked = false;
            }
            else {
                Read(peekbuffer, 0, 1);
            }
            return peekbuffer[0];
        }

        public bool ReadBool() {
            return ReadByte() != 0;
        }

        /// <summary>
        /// reads a short from the stream
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            short result = (short)(ReadByte() << 8);
            result |= ReadByte();
            return result;
        }

        /// <summary>
        /// reads an unsigned short from the stream
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort()
        {
            ushort result = (ushort)(ReadByte() << 8);
            result |= ReadByte();
            return result;
        }

        /// <summary>
        /// reads an integer from the stream
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            int result = ReadByte() << 24;
            result |= ReadByte() << 16;
            result |= ReadByte() << 8;
            result |= ReadByte();
            return result;
        }

        /// <summary>
        /// reads a long integer from the stream
        /// </summary>
        /// <returns></returns>
        public long ReadLong()
        {
            int result = ReadByte() << 56;
            result |= ReadByte() << 48;
            result |= ReadByte() << 40;
            result |= ReadByte() << 32;
            result |= ReadByte() << 24;
            result |= ReadByte() << 16;
            result |= ReadByte() << 8;
            result |= ReadByte();
            return result;
        }

        public float ReadChar()
        {
            byte[] data = new byte[2];
            Read(data, 0, 2);
            return BitConverter.ToChar(data, 0);
        }

        public float ReadFloat() {
            byte[] data = new byte[4];
            Read(data, 0, 4);
            Array.Reverse(data);
            return BitConverter.ToSingle(data, 0);
        }

        public double ReadDouble()
        {
            byte[] data = new byte[8];
            Read(data, 0, 8);
            Array.Reverse(data);
            return BitConverter.ToDouble(data, 0);
        }

        public override void Write(byte[] buffer, int offset, int count) {
            throw new NotImplementedException();
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => basestream.Length;
        public override long Position
        {
            get { return peeked ? basestream.Position - 1 : basestream.Position; }
            set { throw new NotImplementedException(); }
        }

        public string ReadString() {
            return ReadString(ReadUShort());
        }

        public string ReadLongString() {
            return ReadString((int)ReadLong());
        }

        public string ReadString(int length) {
            byte[] data = new byte[length];
            Read(data, 0, data.Length);
            return Encoding.UTF8.GetString(data);
        }
    }
}