namespace System.Text.Encoding
{
	public static class UTF8
    {
        public static byte[] GetBytes(string Text)
		{
            byte[] Buffer = new byte[Text.Length];
            for (ulong I = 0; I < (ulong)Text.Length; I++)
			{
                Buffer[I] = (byte)Text[(int)I];
			}
            return Buffer;
		}
        public static string GetString(byte[] Bytes)
        {
            char[] Buffer = new char[Bytes.Length];
            for (int I = 0; I < Bytes.Length; I++)
            {
                Buffer[I] = (char)Bytes[I];
            }
            return string.Ctor(Buffer);
        }
    }
}