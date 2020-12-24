using System;
using System.Runtime.InteropServices;

namespace PEUtility.Directories
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ImportTable
    {
        [FieldOffset(0)] 
        public UInt32 Characteristics;
        [FieldOffset(4)]
        public UInt32 TimeDateStamp;
        [FieldOffset(8)] 
        public UInt32 ForwarderChain;
        [FieldOffset(12)]
        public UInt32 Name;
        [FieldOffset(16)]
        public UInt32 ThunkTable;
    }
}