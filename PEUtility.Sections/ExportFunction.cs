#nullable enable
using System;

namespace PEUtility.Units
{
    public class ExportFunction
    {
        public String? Name { get; set; }
        public UInt32 Address { get; set; }
        public UInt16 Ordinal { get; set; }
        public String? RedirectionName { get; set; }
    }
}