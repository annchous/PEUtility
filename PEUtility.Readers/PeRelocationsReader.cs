using PEUtility.Directories;
using PEUtility.Headers;
using PEUtility.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using PEUtility.Units;

namespace PEUtility.Readers
{
    public class PeRelocationsReader : PeReader, IReadable
    {
        public ImageDataDirectory BaseRelocationTable { get; set; }
        public List<RelocationBlock> RelocationBlocks { get; set; }

        public PeRelocationsReader(String filePath) : base(filePath)
        {
            RelocationBlocks = new List<RelocationBlock>();
        }

        public IReadable Read()
        {
            var peHeaderReader = (PeHeaderReader)new PeHeaderReader(_filePath).Read();

            BaseRelocationTable = peHeaderReader.Is32BitPeHeader
                ? peHeaderReader.PeHeader32.OptionalHeader.BaseRelocationTable
                : peHeaderReader.PeHeader64.OptionalHeader.BaseRelocationTable;

            var address = RvaToRawFormatConverter.RvaToOffset(BaseRelocationTable.VirtualAddress,
                peHeaderReader.SectionHeaders,
                peHeaderReader.Is32BitPeHeader
                    ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                    : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

            using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);
            fs.Seek(address, SeekOrigin.Begin);

            Int32 currentSize;
            for (var i = 0; i < BaseRelocationTable.Size; i += currentSize)
            {
                currentSize = 0;
                var relocationBlock = new RelocationBlock
                {
                    PageRva = br.ReadUInt32(), 
                    BlockSize = br.ReadUInt32()
                };

                currentSize += Marshal.SizeOf(relocationBlock.PageRva);
                currentSize += Marshal.SizeOf(relocationBlock.BlockSize);

                for (var j = 0; j < (relocationBlock.BlockSize - 8) / 2; j++)
                {
                    var relocationSectionInfo = br.ReadUInt16();
                    var relocationSection = new RelocationSection
                    {
                        Type = (RelocationType)((relocationSectionInfo & 0xf000) >> 12),
                        Offset = (UInt16)(relocationSectionInfo & 0x0fff)
                    };
                    relocationBlock.RelocationSections.Add(relocationSection);
                    currentSize += Marshal.SizeOf(relocationSection.Offset);
                }

                RelocationBlocks.Add(relocationBlock);
            }

            br.Dispose();
            br.Close();

            return this;
        }
    }
}