using System;
using System.Collections.Generic;
using PEUtility.Sections;

namespace PEUtility.Directories
{
    public class RelocationBlock
    {
        public UInt32 PageRva { get; set; }
        public UInt32 BlockSize { get; set; }
        public List<RelocationSection> RelocationSections { get; }

        public RelocationBlock()
        {
            RelocationSections = new List<RelocationSection>();
        }
    }
}
