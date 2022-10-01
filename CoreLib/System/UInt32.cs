namespace System
{
    public struct UInt32
    {
        public const uint MaxValue = 4294967295;
        public const uint MinValue = 0;

        public override string ToString()
        {
            return ((long)this).ToString();
        }
    }
}
