using System;
using PEUtility.Headers;

namespace PEUtility.Tools
{
    public static class RvaToRawFormatConverter
    {
        public static UInt32 RvaToOffset32(UInt32 rva, SectionHeader[] sectionHeaders, UInt32 sectionAlignment)
        {
            var sectionIndex = DefineSection(rva, sectionHeaders, sectionAlignment);
            if (sectionIndex is not null)
                return rva - sectionHeaders[sectionIndex.Value].VirtualAddress +
                       sectionHeaders[sectionIndex.Value].PointerToRawData;
            return 0;
        }

        public static UInt32 RvaToOffset64(UInt64 rva, SectionHeader[] sectionHeaders, UInt32 sectionAlignment)
        {
            var sectionIndex = DefineSection(rva, sectionHeaders, sectionAlignment);
            if (sectionIndex is not null)
                return (UInt32)(rva - sectionHeaders[sectionIndex.Value].VirtualAddress +
                       sectionHeaders[sectionIndex.Value].PointerToRawData);
            return 0;
        }

        private static UInt32? DefineSection(UInt32 rva, SectionHeader[] sectionHeaders, UInt32 sectionAlignment)
        {
            for (UInt32 i = 0; i < sectionHeaders.Length; i++)
            {
                var start = sectionHeaders[i].VirtualAddress;
                var end = start + AlignUp(sectionHeaders[i].VirtualSize, sectionAlignment);

                if (rva >= start && rva < end) return i;
            }

            return null;
        }

        private static UInt64? DefineSection(UInt64 rva, SectionHeader[] sectionHeaders, UInt32 sectionAlignment)
        {
            for (UInt32 i = 0; i < sectionHeaders.Length; i++)
            {
                var start = sectionHeaders[i].VirtualAddress;
                var end = start + AlignUp(sectionHeaders[i].VirtualSize, sectionAlignment);

                if (rva >= start && rva < end) return i;
            }

            return null;
        }

        private static UInt32 AlignDown(UInt32 x, UInt32 alignment) => (UInt32)(x & ~(alignment - 1));
        private static UInt32 AlignUp(UInt32 x, UInt32 alignment) => (UInt32)((x & (alignment - 1)) > 0 ? AlignDown(x, alignment) + 2 : x);
    }
}