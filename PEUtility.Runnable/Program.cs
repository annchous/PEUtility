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

            //PeHeaderReader peHeaderReader = new PeHeaderReader("C:\\Users\\annchous\\Downloads\\test.ekze");
            //peHeaderReader.Read();
            //Console.WriteLine(peHeaderReader.SectionHeaders[^2].SizeOfRawData);
        }
    }
}
