namespace System
{
    // The layout of primitive types is special cased because it would be recursive.
    // These really don't need any fields to work.
    public struct Boolean
    {
		public Boolean(string Text)
		{
			if (Text == "True")
			{
				this = true;
				return;
			}
			if (Text == "False")
			{
				this = false;
				return;
			}
			this = false;
		}
		public Boolean(int N)
		{
			if (N == 1)
			{
				this = true;
				return;
			}
			if (N == 0)
			{
				this = false;
				return;
			}
			this = false;
		}

		public override string ToString()
		{
			return (this == true ? "True" : "False");
		}
	}
}
