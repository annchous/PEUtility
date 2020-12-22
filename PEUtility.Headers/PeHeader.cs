using System;
using System.Runtime.InteropServices;

namespace PEUtility.Headers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PeHeader32
    {
        public UInt32 Signature;
        public FileHeader FileHeader;
        public OptionalHeader32 OptionalHeader;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PeHeader64
    {
        public UInt32 Signature;
        public FileHeader FileHeader;
        public OptionalHeader64 OptionalHeader;
    }
}