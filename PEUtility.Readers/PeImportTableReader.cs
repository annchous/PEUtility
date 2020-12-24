using System;
using System.Collections.Generic;
using PEUtility.Directories;
using PEUtility.Headers;
using PEUtility.Sections;

namespace PEUtility.Readers
{
    public class PeImportTableReader : PeReader, IReadable
    {
        public ImportTable ImportTable { get; set; }
        public List<ImportFunction> ImportFunctions { get; set; }
        public ImageDataDirectory ImportTableDirectory { get; set; }

        public PeImportTableReader(String filePath) : base(filePath)
        {
            ImportFunctions = new List<ImportFunction>();
        }

        public IReadable Read()
        {
            var peHeaderReader = (PeHeaderReader)new PeHeaderReader(_filePath).Read();

            ImportTableDirectory = peHeaderReader.Is32BitPeHeader
                ? peHeaderReader.PeHeader32.OptionalHeader.ImportTable
                : peHeaderReader.PeHeader64.OptionalHeader.ImportTable;



            return this;
        }
    }
}