using System;
using System.Collections.Generic;

namespace PEUtility.Units
{
    public class ImportFunction
    {
        public String DllName { get; set; }
        public DateTime TimeDateStamp { get; set; }
        public List<ImportFunctionInfo> Functions { get; }

        public ImportFunction()
        {
            Functions = new List<ImportFunctionInfo>();
        }
    }
}