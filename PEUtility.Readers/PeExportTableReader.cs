using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PEUtility.Directories;
using PEUtility.Headers;
using PEUtility.Tools;

namespace PEUtility.Readers
{
    public class PeExportTableReader : IReadable
    {
        private readonly String _filePath;
        public ExportTable ExportTable { get; set; }
        public ImageDataDirectory ExportTableDirectory { get; set; }

        public List<String> Names { get; set; }
        public List<UInt32> Ordinals { get; set; }
        public List<UInt32> Functions { get; set; }

        public PeExportTableReader(String filePath)
        {
            _filePath = filePath;
            Names = new List<String>();
            Ordinals = new List<UInt32>();
            Functions = new List<UInt32>();
        }

        public IReadable Read()
        {
            var peHeaderReader = (PeHeaderReader)new PeHeaderReader(_filePath).Read();

            ExportTableDirectory = peHeaderReader.Is32BitPeHeader
                ? peHeaderReader.PeHeader32.OptionalHeader.ExportTable
                : peHeaderReader.PeHeader64.OptionalHeader.ExportTable;

            var address = RvaToRawFormatConverter.RvaToOffset(ExportTableDirectory.VirtualAddress,
                peHeaderReader.SectionHeaders,
                peHeaderReader.Is32BitPeHeader
                    ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                    : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

            using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);
            fs.Seek(address, SeekOrigin.Begin);

            ExportTable = PeBlockToStructConverter.ConvertTo<ExportTable>(br);

            var ordinalsTableAddress = RvaToRawFormatConverter.RvaToOffset(ExportTable.AddressOfNameOrdinals, 
                peHeaderReader.SectionHeaders,
                peHeaderReader.Is32BitPeHeader
                    ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                    : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

            var functionsTableAddress = RvaToRawFormatConverter.RvaToOffset(ExportTable.AddressOfFunctions,
                peHeaderReader.SectionHeaders,
                peHeaderReader.Is32BitPeHeader
                    ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                    : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

            var namesTableAddress = RvaToRawFormatConverter.RvaToOffset(ExportTable.AddressOfNames,
                peHeaderReader.SectionHeaders,
                peHeaderReader.Is32BitPeHeader
                    ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                    : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

            fs.Seek(functionsTableAddress, SeekOrigin.Begin);

            for (UInt32 i = 0; i < ExportTable.NumberOfFunctions; i++)
            {
                Functions.Add(br.ReadUInt32());
            }

            fs.Seek(ordinalsTableAddress, SeekOrigin.Begin);

            for (UInt32 i = 0; i < ExportTable.NumberOfNames; i++)
            {
                Ordinals.Add(br.ReadUInt32());
            }

            return this;
        }
    }
}