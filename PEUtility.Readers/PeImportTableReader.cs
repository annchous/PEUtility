using System;
using System.Collections.Generic;
using System.IO;
using PEUtility.Directories;
using PEUtility.Headers;
using PEUtility.Tools;
using PEUtility.Units;

namespace PEUtility.Readers
{
    public class PeImportTableReader : PeReader, IReadable
    {
        public List<ImportDescriptor> ImportTable { get; set; }
        public List<ImportFunction> ImportFunctions { get; set; }
        public ImageDataDirectory ImportTableDirectory { get; set; }

        public PeImportTableReader(String filePath) : base(filePath)
        {
            ImportTable = new List<ImportDescriptor>();
            ImportFunctions = new List<ImportFunction>();
        }

        public IReadable Read()
        {
            var peHeaderReader = (PeHeaderReader)new PeHeaderReader(_filePath).Read();

            ImportTableDirectory = peHeaderReader.Is32BitPeHeader
                ? peHeaderReader.PeHeader32.OptionalHeader.ImportTable
                : peHeaderReader.PeHeader64.OptionalHeader.ImportTable;

            var address = RvaToRawFormatConverter.RvaToOffset(ImportTableDirectory.VirtualAddress,
                peHeaderReader.SectionHeaders,
                peHeaderReader.Is32BitPeHeader
                    ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                    : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

            using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);
            fs.Seek(address, SeekOrigin.Begin);

            while (true)
            {
                var importDescriptor = PeBlockToStructConverter.ConvertTo<ImportDescriptor>(br);

                if (importDescriptor.OriginalFirstThunk == 0
                    && importDescriptor.ForwarderChain == 0
                    && importDescriptor.Name == 0
                    && importDescriptor.FirstThunk == 0)
                {
                    break;
                }

                ImportTable.Add(importDescriptor);
            }

            var sizeOfThunk = (UInt32)(peHeaderReader.Is32BitPeHeader ? 0x4 : 0x8);
            var ordinal = (UInt32)(peHeaderReader.Is32BitPeHeader ? 0x80000000 : 0x8000000000000000);

            for (var i = 0; i < ImportTable.Count; i++)
            {
                var importFunction = new ImportFunction();

                // Getting DLL name
                var dllNameAddress = RvaToRawFormatConverter.RvaToOffset(ImportTable[i].Name,
                    peHeaderReader.SectionHeaders,
                    peHeaderReader.Is32BitPeHeader
                        ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                        : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

                var currentPosition = fs.Position;
                importFunction.DllName = ByteArrayToAsciiStringConverter.ConvertToString(fs, dllNameAddress);
                fs.Seek(currentPosition, SeekOrigin.Begin);

                Console.WriteLine(importFunction.DllName);
            }

            return this;
        }
    }
}