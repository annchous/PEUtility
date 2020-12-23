using System;
using System.Runtime.InteropServices;
using PEUtility.Tools;

namespace PEUtility.Sections
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RelocationSection
    {
        public RelocationType Type;
        public UInt16 Offset;
    }
}
