using System;
using System.Runtime.InteropServices;

namespace PEUtility.Headers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageDataDirectory
    {
        public UInt32 VirtualAddress;
        public UInt32 Size;
    }
}