using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiDevicePathProtocol
{
    public byte Type;
    public byte SubType;
    public fixed byte Length[2];
}