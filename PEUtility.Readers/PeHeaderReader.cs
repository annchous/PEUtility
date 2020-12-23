using System;
using System.IO;
using PEUtility.Exceptions;
using PEUtility.Headers;
using PEUtility.Tools;

namespace PEUtility.Readers
{
    public class PeHeaderReader : IReadable
    {
        private readonly String _filePath;
        public DosHeader DosHeader { get; set; }
        public PeHeader32 PeHeader32 { get; set; }
        public PeHeader64 PeHeader64 { get; set; }
        public Boolean Is32BitPeHeader { get; set; }
        public SectionHeader[] SectionHeaders { get; set; }

        public PeHeaderReader(String filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Reads all Portable Executable file headers:
        /// DosHeader and PeHeader, which includes Signature,
        /// FileHeader and OptionalHeader depending on bit type of file.
        /// Checks signature to be equal "PE". In case of inequality
        /// throws an exception.
        /// <returns>Void, just fills properties in class.</returns>
        /// </summary>
        public IReadable Read()
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
            }

            br.Dispose();
            br.Close();

            return this;
        }

        /// <summary>
        /// Converts TimeDateStamp from FileHeader structure
        /// given in seconds to UTC DateTime format.
        /// </summary>
        /// <param name="timeDateStamp">Value in seconds from FileHeader structure</param>
        /// <returns></returns>
        public DateTime GetTimeDateStamp(UInt32 timeDateStamp)
        {
            var result = new DateTime(1970, 1, 1, 0, 0, 0);
            return result.AddSeconds(timeDateStamp);
        }
    }
}