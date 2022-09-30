namespace System
{
    public struct Byte
    {
		public const byte MaxValue = 255;
		public const byte MinValue = 0;

		public override string ToString()
		{
			return ((long)this).ToString();
		}
	}
}
