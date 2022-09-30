using System.Runtime;
using Internal.EFI;

namespace System
{
	public static unsafe class MemoryOperations
    {
        [RuntimeExport("memset")]
        public static void Fill(void* Destionation, byte Value, ulong Size)
        {
            EFI.GST->BootServices->SetMem(Destionation, Size, Value);
            //for (byte* p = ptr; p < ptr + count; p++) *p = c;
        }

        [RuntimeExport("memcpy")]
        public static void Copy(void* Destination, void* Source, ulong Size)
        {
            EFI.GST->BootServices->CopyMem(Destination, Source, Size);
            //for (ulong i = 0; i < count; i++) dst[i] = src[i];
        }
    }
}