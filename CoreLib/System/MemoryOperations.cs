using System.Runtime;
using Internal.EFI;

namespace System
{
	public static unsafe class MemoryOperations
    {
        public static void Copy(byte[] Destionation, byte[] Source, ulong Size)
        {
            fixed (byte* P1 = Destionation, P2 = Source)
            {
                Copy(P1, P2, Size);
            }
        }
        public static void Copy(byte[] Destionation, byte* Source, ulong Size)
		{
            fixed (byte* P = Destionation)
			{
                Copy(P, Source, Size);
			}
		}

        public static void Copy(char[] Destination, char[] Source, ulong Size)
		{
            fixed (char* P1 = Destination, P2 = Source)
			{
                Copy(P1, P2, Size);
			}
		}
        public static void Copy(char[] Destination, char* Source, ulong Size)
		{
            fixed (char* P = Destination)
			{
                Copy(P, Source, Size);
			}
		}

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

        public static void* Malloc(ulong Size)
		{
            fixed (byte* PTR = new byte[Size])
			{
                return PTR;
			}
        }
    }
}