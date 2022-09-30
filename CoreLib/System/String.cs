using Internal.Runtime.CompilerHelpers;
using Internal.Runtime.CompilerServices;
using System.Runtime.CompilerServices;

namespace System
{
	public unsafe sealed class String
	{
		/*
         * CONSTRUCTORS
         *
         * Defining a new constructor for string-like types (like String) requires changes both
         * to the managed code below and to the native VM code. See the comment at the top of
         * src/vm/ecall.cpp for instructions on how to add new overloads.
         */
		public static unsafe string Ctor(char* ptr)
		{
			int i = 0;

			while (ptr[i++] != '\0')
			{ }

			return Ctor(ptr, 0, i - 1);
		}
		public static unsafe string Ctor(IntPtr ptr)
		{
			return Ctor((char*)ptr);
		}
		public static unsafe string Ctor(char[] buf)
		{
			fixed (char* _buf = buf)
			{
				return Ctor(_buf, 0, buf.Length);
			}
		}
		public static unsafe string Ctor(char* ptr, int index, int length)
		{
			EETypePtr et = EETypePtr.EETypePtrOf<string>();

			char* start = ptr + index;
			object data = StartupCodeHelpers.RhpNewArray(et._value, length);
			string s = Unsafe.As<object, string>(ref data);

			MemoryOperations.Copy((byte*)s.FirstChar, (byte*)start, (ulong)length * sizeof(char));
			s.FirstChar[length] = '\0';

			return s;
		}

		public unsafe char this[long Index]
		{
			[Intrinsic]
			get => Unsafe.Add(ref FirstChar[0], Index);
			set
			{
				FirstChar[Index] = value;
			}
		}
		public unsafe char this[int Index]
		{
			get
			{
				return this[(long)Index];
			}
			set
			{
				this[(long)Index] = value;
			}
		}

		#region Operators

		public static bool operator ==(string S1, string S2)
		{
			if (S1.Length != S2.Length)
			{
				return false;
			}

			for (long I = 0; I < S1.Length; I++)
			{
				if (S1[I] != S2[I])
				{
					return false;
				}
			}
			return true;
		}
		public static bool operator !=(string S1, string S2)
		{
			return !(S1 == S2);
		}

		#endregion

		#region Methods

		private static ulong GetLength(char* Pointer)
		{
			ulong Index = 0;
			for (; Pointer[Index++] != '\0'; Index++) { }
			return Index;
		}
		public unsafe char[] ToCharArray()
		{
			char[] Array = new char[Length];
			fixed (char* PTR2 = Array)
			{
				MemoryOperations.Copy(PTR2, FirstChar, (ulong)Length * sizeof(char));
			}
			return Array;
		}

		public static bool IsNullOrWhitespace(string Text)
		{
			for (long I = 0; I < Text.Length; I++)
			{
				if (Text[I] != ' ' || Text[I] != 0)
				{
					return false;
				}
			}
			return true;
		}
		public static bool IsNullOrEmpty(string Text)
		{
			if (Text == null || Text.Length == 0)
			{
				return true;
			}
			return false;
		}


		public static string ToUpper(string Text)
		{
			for (long I = 0; I < Text.Length; I++)
			{
				switch (Text[I])
				{
					case 'a':
						Text[I] = 'A';
						break;
					case 'b':
						Text[I] = 'B';
						break;
					case 'c':
						Text[I] = 'C';
						break;
					case 'd':
						Text[I] = 'D';
						break;
					case 'e':
						Text[I] = 'E';
						break;
					case 'f':
						Text[I] = 'F';
						break;
					case 'g':
						Text[I] = 'G';
						break;
					case 'h':
						Text[I] = 'H';
						break;
					case 'i':
						Text[I] = 'I';
						break;
					case 'j':
						Text[I] = 'J';
						break;
					case 'k':
						Text[I] = 'K';
						break;
					case 'l':
						Text[I] = 'L';
						break;
					case 'm':
						Text[I] = 'M';
						break;
					case 'n':
						Text[I] = 'N';
						break;
					case 'o':
						Text[I] = 'O';
						break;
					case 'p':
						Text[I] = 'P';
						break;
					case 'q':
						Text[I] = 'Q';
						break;
					case 'r':
						Text[I] = 'R';
						break;
					case 's':
						Text[I] = 'S';
						break;
					case 't':
						Text[I] = 'T';
						break;
					case 'u':
						Text[I] = 'U';
						break;
					case 'v':
						Text[I] = 'V';
						break;
					case 'w':
						Text[I] = 'W';
						break;
					case 'x':
						Text[I] = 'X';
						break;
					case 'y':
						Text[I] = 'Y';
						break;
					case 'z':
						Text[I] = 'Z';
						break;
				}
			}
			return Text;
		}
		public static string ToLower(string Text)
		{
			for (long I = 0; I < Text.Length; I++)
			{
				switch(Text[I])
				{
					case 'A':
						Text[I] = 'a';
						break;
					case 'B':
						Text[I] = 'b';
						break;
					case 'C':
						Text[I] = 'c';
						break;
					case 'D':
						Text[I] = 'd';
						break;
					case 'E':
						Text[I] = 'e';
						break;
					case 'F':
						Text[I] = 'f';
						break;
					case 'G':
						Text[I] = 'g';
						break;
					case 'H':
						Text[I] = 'h';
						break;
					case 'I':
						Text[I] = 'i';
						break;
					case 'J':
						Text[I] = 'j';
						break;
					case 'K':
						Text[I] = 'k';
						break;
					case 'L':
						Text[I] = 'l';
						break;
					case 'M':
						Text[I] = 'm';
						break;
					case 'N':
						Text[I] = 'n';
						break;
					case 'O':
						Text[I] = 'o';
						break;
					case 'P':
						Text[I] = 'p';
						break;
					case 'Q':
						Text[I] = 'q';
						break;
					case 'R':
						Text[I] = 'r';
						break;
					case 'S':
						Text[I] = 's';
						break;
					case 'T':
						Text[I] = 't';
						break;
					case 'U':
						Text[I] = 'u';
						break;
					case 'V':
						Text[I] = 'v';
						break;
					case 'W':
						Text[I] = 'w';
						break;
					case 'X':
						Text[I] = 'x';
						break;
					case 'Y':
						Text[I] = 'y';
						break;
					case 'Z':
						Text[I] = 'z';
						break;
				}
			}
			return Text;
		}
		public bool StartsWith(char C)
		{
			return FirstChar[0] == C;
		}
		public bool EndsWith(char C)
		{
			return FirstChar[Length] == C;
		}

		#endregion

		#region Fields

		public static readonly string Empty = Ctor((char*)0, 0, 0);
		private char* FirstChar;
		public int Length
		{
			[Intrinsic]
			get => _Length;
			set => _Length = value;
		}
		private int _Length;

		#endregion
	}
}