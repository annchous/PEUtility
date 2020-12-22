using System;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using PEUtility.Readers;

namespace PEUtility.Runnable
{
    class Program
    {
        static void Main(string[] args)
        {
            PeHeaderReader peHeaderReader = new PeHeaderReader("C:\\Users\\annchous\\Downloads\\test.ekze");
            peHeaderReader.Read();
            Console.WriteLine(peHeaderReader.GetTimeDateStamp(peHeaderReader.PeHeader64.FileHeader.TimeDateStamp));
        }
    }
}
