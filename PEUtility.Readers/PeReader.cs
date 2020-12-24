using System;

namespace PEUtility.Readers
{
    public class PeReader
    {
        protected readonly String _filePath;

        public PeReader(String filePath)
        {
            _filePath = filePath;
        }
    }
}