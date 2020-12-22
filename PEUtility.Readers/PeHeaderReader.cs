using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using PEUtility.Exceptions;
using PEUtility.Headers;

namespace PEUtility.Readers
{
    public class PeHeaderReader
    {
        private readonly String _filePath;
        public Boolean Is32BitPeHeader { get; set; }
        public DosHeader DosHeader { get; set; }
        public PeHeader32 PeHeader32 { get; set; }
        public PeHeader64 PeHeader64 { get; set; }
        public SectionHeader[] SectionHeaders { get; set; }

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

            var signature = br.ReadUInt32();
            if (signature != 17744) throw new InvalidPeSignatureException();

            var machineType = br.ReadUInt16();
            Is32BitPeHeader = machineType == 0x10b;

            fs.Seek(DosHeader.e_ifanew, SeekOrigin.Begin);

            if (Is32BitPeHeader)
            {
                PeHeader32 = PeBlockToStructConverter.ConvertTo<PeHeader32>(br);
            }
            else
            {
                PeHeader64 = PeBlockToStructConverter.ConvertTo<PeHeader64>(br);
            }
            
            SectionHeaders = new SectionHeader[Is32BitPeHeader 
                ? PeHeader32.FileHeader.NumberOfSections 
                : PeHeader64.FileHeader.NumberOfSections];
            
            for (var i = 0; i < SectionHeaders.Length; i++)
            {
                SectionHeaders[i] = PeBlockToStructConverter.ConvertTo<SectionHeader>(br);
                Console.WriteLine(SectionHeaders[i].Name);
            }

            br.Dispose();
            br.Close();
        }
    }
}