using System;
using System.Runtime.InteropServices;

namespace PEUtility.Directories
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ImportDescriptor
    {
        [FieldOffset(0)] 
        public UInt32 OriginalFirstThunk;
        [FieldOffset(4)]
        public UInt32 TimeDateStamp;
        [FieldOffset(8)] 
        public UInt32 ForwarderChain;
        [FieldOffset(12)]
        public UInt32 Name;
        [FieldOffset(16)]
        public UInt32 FirstThunk;
    }
}