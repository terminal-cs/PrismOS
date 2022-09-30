using Internal.EFI;

namespace System
{
	public static unsafe class Console
	{
		public static void Clear()
		{
			EFI.GST->ConOut->ClearScreen(EFI.GST->ConOut);
		}

		public static void WriteLine(object O)
		{
			Write(O);
			Write('\n');
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
		public static string ReadLine(bool Output = false)
		{
			while (true)
			{
				EfiInputKey Key = ReadKey();
				char[] Buffer = new char[0];

				switch ((byte)Key.UnicodeChar)
				{
					case 13:
						Write("Entering newline");
						return string.Ctor(Buffer);
					default:
						if (Output)
						{
							Write(Key.UnicodeChar);
						}

						fixed (char* P1 = Buffer, P2 = Buffer = new char[Buffer.Length + 1])
						{
							MemoryOperations.Copy(P2, P1, (ulong)Buffer.Length - 1);
						}
						Buffer[Buffer.Length] = Key.UnicodeChar;
						break;
				}
			}
		}
		public static char ReadChar()
		{
			return ReadKey().UnicodeChar;
		}

		public static void Write(object O)
		{
			Write(O.ToString());
		}
		public static void Write(string s)
		{
			fixed (char* ptr = s)
			{
				EFI.GST->ConOut->OutputString(EFI.GST->ConOut, ptr);
			}
		}
		public static void Write(char c)
		{
			char* chr = stackalloc char[2];
			chr[0] = c;
			chr[1] = '\0';
			EFI.GST->ConOut->OutputString(EFI.GST->ConOut, chr);
		}
	}
}