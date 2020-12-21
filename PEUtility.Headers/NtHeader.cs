using System;

namespace PEUtility.Headers
{
    public struct NtHeader32
    {
        public UInt32 Signature;
        public FileHeader FileHeader;
        public OptionalHeader32 OptionalHeader;
    }

    public struct NtHeader64
    {
        public UInt32 Signature;
        public FileHeader FileHeader;
        public OptionalHeader64 OptionalHeader;
    }
}