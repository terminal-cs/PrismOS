using Internal.EFI;

namespace System
{
    public static unsafe class Console
    {
        public static void Clear()
        {
            EFI.GST->ConOut->ClearScreen(EFI.GST->ConOut);
        }

        public static void Write(char c)
        {
            char* chr = stackalloc char[2];
            chr[0] = c;
            chr[1] = '\0';
            EFI.GST->ConOut->OutputString(EFI.GST->ConOut, chr);
        }

        public static void Write(string s)
        {
            fixed(char* ptr = s)
            {
                EFI.GST->ConOut->OutputString(EFI.GST->ConOut, ptr);
            }
        }

        public static void WriteLine(string s)
        {
            Write(s);
            WriteLine();
        }

        public static void WriteLine()
        {
            char* chr = stackalloc char[3];
            chr[0] = '\n';
            chr[1] = '\r';
            chr[2] = '\0';
            EFI.GST->ConOut->OutputString(EFI.GST->ConOut, chr);
        }

        public static EfiInputKey ReadKey()
        {
            EfiInputKey key;
            ulong keyEvent = 0;
            EFI.GBS->WaitForEvent(1, &EFI.GST->ConIn->WaitForKey, &keyEvent);
            EFI.GST->ConIn->ReadKeyStroke(EFI.GST->ConIn, &key);
            EFI.GST->ConIn->Reset(EFI.GST->ConIn, false);
            return key;
        }
    }
}
