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

            var address = RvaToRawFormatConverter.RvaToOffset32(ImportTableDirectory.VirtualAddress,
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

            for (var i = 0; i < ImportTable.Count; i++)
            {
                var importFunction = new ImportFunction();

                // Getting DLL name
                var dllNameAddress = RvaToRawFormatConverter.RvaToOffset32(ImportTable[i].Name,
                    peHeaderReader.SectionHeaders,
                    peHeaderReader.Is32BitPeHeader
                        ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                        : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

                var currentPosition = fs.Position;
                importFunction.DllName = ByteArrayToAsciiStringConverter.ConvertToString(fs, dllNameAddress);
                fs.Seek(currentPosition, SeekOrigin.Begin);
                importFunction.TimeDateStamp = TimeDateStampToDateTimeConverter.ConvertTimeDateStamp(ImportTable[i].TimeDateStamp);

                var iat = RvaToRawFormatConverter.RvaToOffset32(ImportTable[i].FirstThunk,
                    peHeaderReader.SectionHeaders,
                    peHeaderReader.Is32BitPeHeader
                        ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                        : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

                var ilt = RvaToRawFormatConverter.RvaToOffset32(ImportTable[i].OriginalFirstThunk,
                    peHeaderReader.SectionHeaders,
                    peHeaderReader.Is32BitPeHeader
                        ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                        : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

                if (peHeaderReader.Is32BitPeHeader)
                {
                    fs.Seek(ilt, SeekOrigin.Begin);
                    var thunkData32 = PeBlockToStructConverter.ConvertTo<ThunkData32>(br);
                    fs.Seek(currentPosition, SeekOrigin.Begin);

                    while (thunkData32.Function != 0)
                    {
                        var currentPosition2 = fs.Position;
                        if ((thunkData32.AddressOfData & (UInt32) 1 << 31) == 0)
                        {
                            var functionAddress = RvaToRawFormatConverter.RvaToOffset32(thunkData32.Function,
                                peHeaderReader.SectionHeaders,
                                peHeaderReader.Is32BitPeHeader
                                    ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                                    : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

                            fs.Seek(functionAddress, SeekOrigin.Begin);

                            importFunction.Functions.Add(new ImportFunctionInfo()
                            {
                                Hint = br.ReadUInt16(),
                                Name = ByteArrayToAsciiStringConverter.ConvertToString(fs, 0)
                            });

                            fs.Seek(currentPosition2, SeekOrigin.Begin);
                        }
                        else
                        {
                            importFunction.Functions.Add(new ImportFunctionInfo()
                            {
                                Name = null, 
                                Ordinal64 = null,
                                Ordinal32 = (thunkData32.Ordinal & ((UInt32)1 << 31) - 1)
                            });
                        }

                        thunkData32 = PeBlockToStructConverter.ConvertTo<ThunkData32>(br);
                    }
                }
                else
                {
                    fs.Seek(ilt, SeekOrigin.Begin);
                    var thunkData64 = PeBlockToStructConverter.ConvertTo<ThunkData64>(br);
                    fs.Seek(currentPosition, SeekOrigin.Begin);

                    while (thunkData64.Function != 0)
                    {
                        var currentPosition2 = fs.Position;
                        if ((thunkData64.AddressOfData & (UInt64)1 << 63) == 0)
                        {
                            var functionAddress = RvaToRawFormatConverter.RvaToOffset64(thunkData64.Function,
                                peHeaderReader.SectionHeaders,
                                peHeaderReader.Is32BitPeHeader
                                    ? peHeaderReader.PeHeader32.OptionalHeader.SectionAlignment
                                    : peHeaderReader.PeHeader64.OptionalHeader.SectionAlignment);

                            fs.Seek(functionAddress, SeekOrigin.Begin);

                            importFunction.Functions.Add(new ImportFunctionInfo()
                            {
                                Hint = br.ReadUInt16(),
                                Name = ByteArrayToAsciiStringConverter.ConvertToString(fs, 0)
                            });

                            fs.Seek(currentPosition2, SeekOrigin.Begin);
                        }
                        else
                        {
                            importFunction.Functions.Add(new ImportFunctionInfo()
                            {
                                Name = null,
                                Ordinal32 = null,
                                Ordinal64 = (thunkData64.Ordinal & ((UInt64)1 << 63) - 1)
                            });
                        }

                        thunkData64 = PeBlockToStructConverter.ConvertTo<ThunkData64>(br);
                    }
                }

                ImportFunctions.Add(importFunction);
            }

            return this;
        }
    }
}