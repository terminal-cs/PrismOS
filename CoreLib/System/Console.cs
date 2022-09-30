namespace System
{
    public static unsafe class Console
    {
        public static void Clear()
        {
            GST->ConOut->ClearScreen(GST->ConOut);
        }

        public static void Write(char c)
        {
            char* chr = stackalloc char[2];
            chr[0] = c;
            chr[1] = '\0';
            GST->ConOut->OutputString(GST->ConOut, chr);
        }

        public static void Write(string s)
        {
            fixed(char* ptr = s)
            {
                GST->ConOut->OutputString(GST->ConOut, ptr);
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
            GST->ConOut->OutputString(GST->ConOut, chr);
        }

        public static EfiInputKey ReadKey()
        {
            EfiInputKey key;
            ulong keyEvent = 0;
            GBS->WaitForEvent(1, &GST->ConIn->WaitForKey, &keyEvent);
            GST->ConIn->ReadKeyStroke(GST->ConIn, &key);
            GST->ConIn->Reset(GST->ConIn, false);
            return key;
        }
    }
}
