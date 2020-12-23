using System;

namespace PEUtility.Sections
{
    public class ExportFunction
    {
        public String Name { get; set; }
        public UInt32 Address { get; set; }
        public UInt16 Ordinal { get; set; }

        public ExportFunction(UInt32 address, UInt16 ordinal, String name = default(String))
        {
            Name = name;
            Address = address;
            Ordinal = ordinal;
        }
    }
}