using System;
using PEUtility.Readers;

namespace PEUtility.Runnable
{
    class Program
    {
        static void Main(string[] args)
        {
            PeImportTableReader peImportTableReader = new PeImportTableReader("C:\\Windows\\System32\\kernel32.dll");
            peImportTableReader.Read();

            foreach (var importFunction in peImportTableReader.ImportFunctions)
            {
                Console.WriteLine(importFunction.DllName);

                foreach (var importFunctionInfo in importFunction.Functions)
                {
                    Console.Write("HINT: " + importFunctionInfo.Hint);
                    if (importFunctionInfo.Name != null)
                    {
                        Console.Write(" NAME " + importFunctionInfo.Name);
                    }
                    else
                    {
                        Console.Write(" ORDINAL: " + (importFunctionInfo.Ordinal32 ?? importFunctionInfo.Ordinal64));
                    }
                    Console.Write("\n");
                }
            }
        }
    }
}
