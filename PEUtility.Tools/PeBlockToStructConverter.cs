using System.IO;
using System.Runtime.InteropServices;

namespace PEUtility.Tools
{
    public static class PeBlockToStructConverter
    {
        public static T ConvertTo<T>(BinaryReader br)
        {
            var bytes = br.ReadBytes(Marshal.SizeOf(typeof(T)));
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return result;
        }
    }
}