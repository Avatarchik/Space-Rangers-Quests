using System;
using System.IO;
using System.Linq;

namespace SpaceRangersQuests.Model.Utils
{
    public static partial class StreamExtensions
    {
        /// <summary>
        /// Порядок байтов
        /// </summary>
        public enum Endian
        {
            /// <summary>
            /// 0x01020304 => 0x04030201
            /// </summary>
            LittleEndian,
            /// <summary>
            /// 0x01020304 => 0x01020304
            /// </summary>
            BigEndian
        }

        /// <summary>
        /// Прочитать из потока 2-х байтовое целое.
        /// </summary>
        /// <param name="stream">входной поток</param>
        /// <returns>2-х байтовое целое без знака</returns>
        public static Int16 ReadInt16(this Stream stream, Endian endian = Endian.LittleEndian)
        {
            var b0 = stream.ReadByte();
            var b1 = stream.ReadByte();
            return endian == Endian.LittleEndian
                ? (Int16)((b0 << 0) + (b1 << 8))
                : (Int16)((b1 << 0) + (b0 << 8));
        }

        /// <summary>
        /// Прочитать из потока 2-х байтовое целое без знака.
        /// </summary>
        /// <param name="stream">входной поток</param>
        /// <returns>2-х байтовое целое без знака</returns>
        public static UInt16 ReadUInt16(this Stream stream, Endian endian = Endian.LittleEndian)
        {
            var b0 = stream.ReadByte();
            var b1 = stream.ReadByte();
            return endian == Endian.LittleEndian
                ? (UInt16)((b0 << 0) + (b1 << 8))
                : (UInt16)((b1 << 0) + (b0 << 8));
        }

        /// <summary>
        /// Прочитать из потока число с плавающей точкой двойной точности.
        /// </summary>
        /// <param name="stream">входной поток</param>
        /// <returns>Число с плавающей точкой двойной точности</returns>
        public static double ReadDouble(this Stream stream, Endian endian = Endian.LittleEndian)
        {
            var buffer = new byte[8];
            stream.Read(buffer, 0, buffer.Length);
            if (endian == Endian.BigEndian)
                buffer = buffer.Reverse().ToArray();
            return BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        /// Прочитать из потока 4-х байтовое целое.
        /// </summary>
        /// <param name="stream">входной поток</param>
        /// <param name="endian">Порядок байтов</param>
        /// <returns>4-х байтовое целое</returns>
        public static Int32 ReadInt32(this Stream stream, Endian endian = Endian.LittleEndian)
        {
            var value0 = stream.ReadByte();
            var value1 = stream.ReadByte();
            var value2 = stream.ReadByte();
            var value3 = stream.ReadByte();
            return endian == Endian.LittleEndian
                ? (value3 << 24)
                  + (value2 << 16)
                  + (value1 << 8)
                  + (value0 << 0)
                : (value0 << 24)
                  + (value1 << 16)
                  + (value2 << 8)
                  + (value3 << 0);
        }

        /// <summary>
        /// Прочитать из потока 4-х байтовое целое без знака.
        /// </summary>
        /// <param name="stream">входной поток</param>
        /// <param name="endian">Порядок байтов</param>
        /// <returns>4-х байтовое целое без знака</returns>
        public static UInt32 ReadUInt32(this Stream stream, Endian endian = Endian.LittleEndian)
        {
            var value0 = stream.ReadByte();
            var value1 = stream.ReadByte();
            var value2 = stream.ReadByte();
            var value3 = stream.ReadByte();
            return endian == Endian.LittleEndian
                ? (UInt32)((value3 << 24)
                           + (value2 << 16)
                           + (value1 << 8)
                           + (value0 << 0))
                : (UInt32)((value0 << 24)
                           + (value1 << 16)
                           + (value2 << 8)
                           + (value3 << 0));
        }
    }
}
