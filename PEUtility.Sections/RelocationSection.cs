using System;
using System.Runtime.InteropServices;
using PEUtility.Tools;

namespace PEUtility.Sections
{
    public struct RelocationSection
    {
        public RelocationType Type;
        public UInt32 Offset;
    }
}
