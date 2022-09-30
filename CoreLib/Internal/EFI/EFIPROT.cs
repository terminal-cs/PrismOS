using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct EfiGraphicsOutputProtocol
{
    public readonly delegate* unmanaged<EfiGraphicsOutputProtocol*, uint, ulong*, EfiGraphicsOutputModeInformation**, EfiStatus> QueryMode;
    public readonly delegate* unmanaged<EfiGraphicsOutputProtocol*, uint, EfiStatus> SetMode;
    public readonly delegate* unmanaged<EfiGraphicsOutputProtocol*, EfiGraphicsOutputBltPixel*, EfiGraphicsOutputBltOpreation, ulong, ulong, ulong, ulong, ulong, ulong, ulong, EfiStatus> Blt;

    public readonly EfiGraphicsOutputProtocolMode* Mode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiLoadedImageProtocol
{
    public uint Revision;
    public EfiHandle ParentHandle;
    public EfiSystemTable* SystemTable;
    public EfiHandle DeviceHandle;
    public EfiDevicePathProtocol* FilePath;
    public void* Reserved;
    public uint LoadOptionsSize;
    public void* LoadOptions;
    public void* ImageBase;
    public ulong ImageSize;
    public EfiMemoryType ImageCodeType;
    public EfiMemoryType ImageDataType;
    public readonly delegate* unmanaged<EfiHandle, EfiStatus> Unload;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiSimplePointerProtocol
{
    public readonly delegate* unmanaged<EfiSimplePointerProtocol*, bool, EfiStatus> Reset;
    public readonly delegate* unmanaged<EfiSimplePointerProtocol*, EfiSimplePointerState*, EfiStatus> GetState;
    public readonly EfiEvent WaitForInput;
    public readonly EfiSimplePointerMode* Mode;

}

[StructLayout(LayoutKind.Sequential)]
public struct EfiSimplePointerMode{

    public ulong ResolutionX;
    public ulong ResolutionY;
    public ulong ResolutionZ;
    public bool LeftButton;
    public bool RightButton;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiSimplePointerState{

    public int RelativeMovementX;
    public int RelativeMovementY;
    public int RelativeMovementZ;
    public bool LeftButton;
    public bool RightButton;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiSimpleFileSystemProtocol
{
    public ulong Revision;
    public delegate* unmanaged<EfiSimpleFileSystemProtocol*, EfiFileHandle**, EfiStatus> OpenVolume;
}

public enum EfiFileMode : ulong
{
    Read = 0x0000000000000001,
    Write = 0x0000000000000002,
    Create = 0x8000000000000000
}

public enum EfiFileAttribute : ulong
{
    ReadOnly = 0x0000000000000001,
    Hidden = 0x0000000000000002,
    System = 0x0000000000000004,
    Reservied = 0x0000000000000008,
    Directory = 0x0000000000000010,
    Archive = 0x0000000000000020,
    ValidAttr = 0x0000000000000037
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiFileHandle
{
    public ulong Revision;
    public delegate* unmanaged<EfiFileHandle*, EfiFileHandle**, char*, EfiFileMode, EfiFileAttribute, EfiStatus> Open;
    public delegate* unmanaged<EfiFileHandle*, EfiStatus> Close;
    public delegate* unmanaged<EfiFileHandle*, EfiStatus> Delete;
    public delegate* unmanaged<EfiFileHandle*, ulong*, void*, EfiStatus> Read;
    public delegate* unmanaged<EfiFileHandle*, ulong*, void*, EfiStatus> Write;
    public delegate* unmanaged<EfiFileHandle*, ulong*, EfiStatus> GetPosition;
    public delegate* unmanaged<EfiFileHandle*, ulong, EfiStatus> SetPosition;
    public delegate* unmanaged<EfiFileHandle*, EfiGuid*, ulong*, void*, EfiStatus> GetInfo;
    public delegate* unmanaged<EfiFileHandle*, EfiGuid*, ulong, void*, EfiStatus> SetInfo;
    public delegate* unmanaged<EfiFileHandle*, EfiStatus> Flush;
    public delegate* unmanaged<EfiFileHandle*, EfiFileHandle**, char*, EfiFileMode, EfiFileAttribute, EfiFileIoToken*, EfiStatus> OpenEx;
    public delegate* unmanaged<EfiFileHandle*, EfiFileIoToken*, EfiStatus> ReadEx;
    public delegate* unmanaged<EfiFileHandle*, EfiFileIoToken*, EfiStatus> WriteEx;
    public delegate* unmanaged<EfiFileHandle*, EfiFileIoToken*, EfiStatus> FlushEx;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiFileIoToken
{
    public EfiEvent Event;
    public EfiStatus Status;
    public ulong BufferSize;
    public void* Buffer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiFileInfo
{
    public ulong Size;
    public ulong FileSize;
    public ulong PhysicalSize;
    public EfiTime CreateTime;
    public EfiTime LastAccessTime;
    public EfiTime ModificationTime;
    public ulong Attribute;
    public fixed char FileName[128];
}