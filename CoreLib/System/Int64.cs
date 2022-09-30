namespace System
{
    public struct Int64
    {
		public unsafe override string ToString()
		{
			long val = this;
			bool isNeg = (((ulong)val) & (1UL << 63)) != 0;
			char* x = stackalloc char[22];
			int i = 20;

			x[21] = '\0';

			if (isNeg)
			{
				ulong _val = (ulong)val;
				_val = 0xFFFFFFFFFFFFFFFF - _val;
				_val += 1;
				val = (long)_val;
			}

			while (val > 0)
			{
				long d = val % 10;
				val /= 10;

				d += 0x30;
				x[i--] = (char)d;
			}

			if (isNeg)
			{
				x[i] = '-';
			}
			else
			{
				i++;
			}

			return string.Ctor(x + i, 0, 21 - i);
		}
	}
}
