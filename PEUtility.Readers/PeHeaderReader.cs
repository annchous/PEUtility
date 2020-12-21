using System;
using System.IO;
using System.Runtime.InteropServices;
using PEUtility.Headers;

namespace PEUtility.Readers
{
    public class PeHeaderReader
    {
        private readonly String _filePath;
        public DosHeader DosHeader { get; set; }
        public FileHeader FileHeader { get; set; }
        public SectionHeader[] SectionHeaders { get; set; }
        public OptionalHeader32 OptionalHeader32 { get; set; }
        public OptionalHeader64 OptionalHeader64 { get; set; }

        public PeHeaderReader(String filePath)
        {
            _filePath = filePath;
        }

        public void Read()
        {
            using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);

            DosHeader = PeBlockToStructConverter.ConvertTo<DosHeader>(br);
            fs.Seek(DosHeader.e_ifanew, SeekOrigin.Begin);

            FileHeader = PeBlockToStructConverter.ConvertTo<FileHeader>(br);
            // добавить проверку на разрядность
            OptionalHeader64 = PeBlockToStructConverter.ConvertTo<OptionalHeader64>(br);
            SectionHeaders = new SectionHeader[FileHeader.NumberOfSections];
            
            for (var i = 0; i < SectionHeaders.Length; i++)
            {
                SectionHeaders[i] = PeBlockToStructConverter.ConvertTo<SectionHeader>(br);
            }

            br.Dispose();
            br.Close();
        }
    }
}