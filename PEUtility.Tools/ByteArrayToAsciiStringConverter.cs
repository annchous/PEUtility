using System;
using System.IO;
using System.Text;

namespace PEUtility.Tools
{
    public static class ByteArrayToAsciiStringConverter
    {
        public static String ConvertToString(Stream stream, UInt32 stringOffset)
        {
            var stringLength = GetStringLength(stream, stringOffset);
            var byteArray = new Byte[stringLength];

            stream.Seek(stringOffset, SeekOrigin.Begin);
            stream.Read(byteArray);

            return Encoding.ASCII.GetString(byteArray);
        }

        private static Int32 GetStringLength(Stream stream, UInt32 offset)
        {
            var result = 0;
            stream.Seek(offset, SeekOrigin.Begin);

            while (stream.ReadByte() != 0x00)
            {
                result++;
            }

            return result;
        }
    }
}