using System;
using PEUtility.Readers;

namespace PEUtility.Runnable
{
    class Program
    {
        static void Main(string[] args)
        {
            PeImportTableReader peImportTableReader = new PeImportTableReader("C:\\Users\\annchous\\Downloads\\test.ekze");
            peImportTableReader.Read();

            foreach (var importFunction in peImportTableReader.ImportFunctions)
            {
                Console.WriteLine(importFunction.DllName);

                foreach (var importFunctionInfo in importFunction.Functions)
                {
                    Console.WriteLine(importFunctionInfo.Hint);
                }
            }

            //PeHeaderReader peHeaderReader = new PeHeaderReader("C:\\Users\\annchous\\Downloads\\test.ekze");
            //peHeaderReader.Read();
            //Console.WriteLine(peHeaderReader.SectionHeaders[^2].SizeOfRawData);
        }
    }
}
