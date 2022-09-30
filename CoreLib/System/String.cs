using Internal.Runtime.CompilerHelpers;
using Internal.Runtime.CompilerServices;
using System.Runtime.CompilerServices;

namespace System
{
	public unsafe sealed class String
	{
		public extern unsafe String(char* Pointer, int Index, int Length);
		public extern unsafe String(IntPtr Pointer);
		public extern unsafe String(char[] Buffer);
		public extern unsafe String(char* Pointer);

		/*
         * CONSTRUCTORS
         *
         * Defining a new constructor for string-like types (like String) requires changes both
         * to the managed code below and to the native VM code. See the comment at the top of
         * src/vm/ecall.cpp for instructions on how to add new overloads.
         */
		public static unsafe string Ctor(char* Pointer, int Index, int Length)
		{
			EETypePtr et = EETypePtr.EETypePtrOf<string>();

			char* start = Pointer + Index;
			object data = StartupCodeHelpers.RhpNewArray(et._value, Length);
			string s = Unsafe.As<object, string>(ref data);

			MemoryOperations.Copy((byte*)s.FirstChar, (byte*)start, (ulong)Length * sizeof(char));
			s.FirstChar[Length] = '\0';

			return s;
		}
		public static unsafe string Ctor(IntPtr Pointer)
		{
			return Ctor((char*)Pointer);
		}
		public static unsafe string Ctor(char[] Buffer)
		{
			fixed (char* _buf = Buffer)
			{
				return Ctor(_buf, 0, Buffer.Length);
			}
		}
		public static unsafe string Ctor(char* Pointer)
		{
			int i = 0;

			while (Pointer[i++] != '\0') { }

			return Ctor(Pointer, 0, i - 1);
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

		public static string operator +(string S1, string S2)
		{
			char* Buffer = stackalloc char[S1.Length + S2.Length];
			for (int I = 0; I < S1.Length; I++)
			{
				Buffer[I] = S1[I];
			}
			for (int I = 0; I < S2.Length; I++)
			{
				Buffer[S1.Length + I] = S2[I];
			}
			return Ctor(Buffer, 0, S1.Length + S2.Length);
		}
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
			MemoryOperations.Copy(Array, FirstChar, (ulong)(Length * sizeof(char)));
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

		public static string Concat(string S1, string S2)
		{
			return S1 + S2;
		}
		public bool StartsWith(char C)
		{
			return FirstChar[0] == C;
		}
		public bool EndsWith(char C)
		{
			return FirstChar[Length] == C;
		}
		public string ToUpper()
		{
			char[] T = new char[Length];
			for (long I = 0; I < Length; I++)
			{
				T[I] = this[I] switch
				{
					'a' => 'A',
					'b' => 'B',
					'c' => 'C',
					'd' => 'D',
					'e' => 'E',
					'f' => 'F',
					'g' => 'G',
					'h' => 'H',
					'i' => 'I',
					'j' => 'J',
					'k' => 'K',
					'l' => 'L',
					'm' => 'M',
					'n' => 'N',
					'o' => 'O',
					'p' => 'P',
					'q' => 'Q',
					'r' => 'R',
					's' => 'S',
					't' => 'T',
					'u' => 'U',
					'v' => 'V',
					'w' => 'W',
					'x' => 'X',
					'y' => 'Y',
					'z' => 'Z',
					_ => this[I],
				};
			}
			return Ctor(T);
		}
		public string ToLower()
		{
			char[] T = new char[Length];
			for (long I = 0; I < Length; I++)
			{
				T[I] = this[I] switch
				{
					'A' => 'a',
					'B' => 'b',
					'C' => 'c',
					'D' => 'd',
					'E' => 'e',
					'F' => 'f',
					'G' => 'g',
					'H' => 'h',
					'I' => 'i',
					'J' => 'j',
					'K' => 'k',
					'L' => 'l',
					'M' => 'm',
					'N' => 'n',
					'O' => 'o',
					'P' => 'p',
					'Q' => 'q',
					'R' => 'r',
					'S' => 's',
					'T' => 't',
					'U' => 'u',
					'V' => 'v',
					'W' => 'w',
					'X' => 'x',
					'Y' => 'y',
					'Z' => 'z',
					_ => this[I],
				};
			}
			return Ctor(T);
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