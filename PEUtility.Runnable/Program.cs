using System;
using PEUtility.Readers;

namespace PEUtility.Runnable
{
    class Program
    {
        static void Main(string[] args)
        {
            PeRelocationsReader peRelocationsReader = new PeRelocationsReader("C:\\Users\\annchous\\Downloads\\test.ekze");
            peRelocationsReader.Read();

            //PeHeaderReader peHeaderReader = new PeHeaderReader("C:\\Users\\annchous\\Downloads\\test.ekze");
            //peHeaderReader.Read();
            //Console.WriteLine(peHeaderReader.SectionHeaders[^2].SizeOfRawData);
        }
    }
}
