using System;
using System.Runtime.InteropServices;

namespace PEUtility.Units
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ThunkData32
    {
        [FieldOffset(0)] 
        public UInt32 ForwarderString;
        [FieldOffset(4)] 
        public UInt32 Function;
        [FieldOffset(8)] 
        public UInt32 Ordinal;
        [FieldOffset(12)] 
        public UInt32 AddressOfData;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ThunkData64
    {
        [FieldOffset(0)]
        public UInt64 ForwarderString;
        [FieldOffset(8)]
        public UInt64 Function;
        [FieldOffset(16)]
        public UInt64 Ordinal;
        [FieldOffset(24)]
        public UInt64 AddressOfData;
    }
}