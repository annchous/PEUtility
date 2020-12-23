using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PEUtility.Directories;
using PEUtility.Headers;
using PEUtility.Sections;
using PEUtility.Tools;

namespace PEUtility.Readers
{
    public class PeExportTableReader : IReadable
    {
        private readonly String _filePath;
        public ExportTable ExportTable { get; set; }
        public ImageDataDirectory ExportTableDirectory { get; set; }
        public List<ExportFunction> ExportFunctions { get; set; }

        public PeExportTableReader(String filePath)
        {
            _filePath = filePath;
            ExportFunctions = new List<ExportFunction>();
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
                var ordinal = ExportTable.Base + i;
                var exportFunctionAddress = br.ReadUInt32();
                ExportFunctions.Add(new ExportFunction(exportFunctionAddress, (UInt16)ordinal));
            }

            fs.Seek(namesTableAddress, SeekOrigin.Begin);

            for (UInt32 i = 0; i < ExportTable.NumberOfNames; i++)
            {
                var nameRva = br.ReadUInt32();
                var nameAddress = RvaToRawFormatConverter.RvaToOffset(nameRva, 
                    peHeaderReader.SectionHeaders,
                    peHeaderReader.Is32BitPeHeader
                        ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                        : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

                var currentPosition = fs.Position;
                String name = ByteArrayToAsciiStringConverter.ConvertToString(fs, nameAddress);
                fs.Seek(currentPosition, SeekOrigin.Begin);

                
            }

            return this;
        }

        private Boolean RedirectionRva(UInt32 address) => address >= ExportTableDirectory.VirtualAddress &&
                                                          address < ExportTableDirectory.VirtualAddress +
                                                          ExportTableDirectory.Size;
    }
}