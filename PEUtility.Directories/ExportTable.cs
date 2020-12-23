using System;
using System.Runtime.InteropServices;

namespace PEUtility.Directories
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ExportTable
    {
        [FieldOffset(0)]
        public UInt32 Characteristics;
        [FieldOffset(4)]
        public UInt32 TimeDateStamp;
        [FieldOffset(8)]
        public UInt16 MajorVersion;
        [FieldOffset(10)]
        public UInt16 MinorVersion;
        [FieldOffset(12)]
        public UInt32 Name;
        [FieldOffset(16)]
        public UInt32 Base;
        [FieldOffset(20)]
        public UInt32 NumberOfFunctions;
        [FieldOffset(24)]
        public UInt32 NumberOfNames;
        [FieldOffset(28)]
        public UInt32 AddressOfFunctions;
        [FieldOffset(32)]
        public UInt32 AddressOfNames;
        [FieldOffset(36)]
        public UInt32 AddressOfNameOrdinals;
    }
}