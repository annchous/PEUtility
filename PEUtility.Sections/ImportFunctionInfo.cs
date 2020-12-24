#nullable enable
using System;

namespace PEUtility.Units
{
    public class ImportFunctionInfo
    {
        public UInt16 Hint { get; set; }
        public String? Name { get; set; }
        public UInt32? Ordinal32 { get; set; }
        public UInt64? Ordinal64 { get; set; }
    }
}