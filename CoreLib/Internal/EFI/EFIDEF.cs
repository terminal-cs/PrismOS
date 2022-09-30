using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiGuid
{
    public static implicit operator EfiGuid*(EfiGuid guid) => &guid;

    public uint Data1;
    public ushort Data2;
    public ushort Data3;
    public fixed byte Data4[8];

    public EfiGuid(uint d1, ushort d2, ushort d3, byte d4_0, byte d4_1, byte d4_2, byte d4_3, byte d4_4, byte d4_5, byte d4_6, byte d4_7)
    {
        Data1 = d1;
        Data2 = d2;
        Data3 = d3;
        Data4[0] = d4_0;
        Data4[1] = d4_1;
        Data4[2] = d4_2;
        Data4[3] = d4_3;
        Data4[4] = d4_4;
        Data4[5] = d4_5;
        Data4[6] = d4_6;
        Data4[7] = d4_7;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiTime
{
    public ushort Year; // 1998 - 20XX
    public byte Month; // 1 - 12
    public byte Day; // 1 - 31
    public byte Hour; // 0 - 23
    public byte Minute; // 0 - 59
    public byte Second; // 0 - 59
    public byte Pad1;
    public uint Nanosecond; // 0 - 999,999,999
    public short TimeZone; // -1440 to 1440 or 2047
    public byte Daylight;
    public byte Pad2;
}


[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiIPv4Address
{
    public fixed byte Addr[4];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiIPv6Address
{
    public fixed byte Addr[16];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiMacAddress
{
    public fixed byte Addr[32];
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiManagedNetworkConfigData
{
    public uint ReceivedQueueTimeoutValue;
    public uint TransmitQueueTimeoutValue;
    public ushort ProtocolTypeFilter;
    public bool EnableUnicastReceive;
    public bool EnableMulticastReceive;
    public bool EnableBroadcastReceive;
    public bool EnablePromiscuousReceive;
    public bool FlushQueuesOnReset;
    public bool EnableReceiveTimestamps;
    public bool DisableBackgroundPolling;
}


public enum EfiAllocateType
{
    AllocateAnyPages,
    AllocateMaxAddress,
    AllocateAddress,
    MaxAllocateType
}


public enum EfiMemoryType : uint
{
    EfiReservedMemoryType,
    EfiLoaderCode,
    EfiLoaderData,
    EfiBootServicesCode,
    EfiBootServicesData,
    EfiRuntimeServicesCode,
    EfiRuntimeServicesData,
    EfiConventionalMemory,
    EfiUnusableMemory,
    EfiACPIReclaimMemory,
    EfiACPIMemoryNVS,
    EfiMemoryMappedIO,
    EfiMemoryMappedIOPortSpace,
    EfiPalCode,
    EfiMaxMemoryType
}

public enum EfiGraphicsPixelFormat
{
    RedGreenBlueReserved8BitPerColor,
    BlueGreenRedReserved8BitPerColor,
    BitMask,
    BltOnly
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiMemoryDescriptor
{
    public EfiMemoryType Type; // Field size is 32 bits followed by 32 bit pad
    public uint Pad;
    public EfiPhysicalAddress PhysicalStart; // Field size is 64 bits
    public EfiVirtualAddress VirtualStart; // Field size is 64 bits
    public ulong NumberOfPages; // Field size is 64 bits
    public ulong Attribute; // Field size is 64 bits
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiGraphicsOutputBltPixel
{
    public byte Blue;
    public byte Green;
    public byte Red;
    public byte Reserved;
}

public enum EfiGraphicsOutputBltOpreation
{
    EfiBltVideoFill,
    EfiBltVideoToBltBuffer,
    EfiBltBufferToVideo,
    EfiBltVideoToVideo,
    EfiGraphicsOutputBltOperationMax
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiGraphicsOutputProtocolMode
{
    public uint MaxMode;
    public uint Mode;
    public EfiGraphicsOutputModeInformation* Info;
    public ulong SizeOfInfo;
    public ulong FrameBufferBase;
    public ulong FrameBufferSize;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiGraphicsOutputModeInformation
{
    public uint Version;
    public uint HorizontalResolution;
    public uint VerticalResolution;
    public EfiGraphicsPixelFormat PixelFormat;
    public EfiGraphicsOutputBltPixel PixelInformation;
    public uint PixelsPerScanLine;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiServiceBindingProtocol
{
    public delegate* unmanaged<EfiServiceBindingProtocol*, EfiHandle*, EfiStatus> CreateChild;
    public delegate* unmanaged<EfiServiceBindingProtocol*, EfiHandle, EfiStatus> DestroyChild;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiSimpleNetworkMode
{
    public uint State;
    public uint HwAddressSize;
    public uint MediaHeaderSize;
    public uint MaxPacketSize;
    public uint NvRamSize;
    public uint NvRamAccessSize;
    public uint ReceiveFilterMask;
    public uint ReceiveFilterSetting;
    public uint MaxMCastFilterCount;
    public uint MCastFilterCount;
    public EfiMacAddress MCastFilter0;
    public EfiMacAddress MCastFilter1;
    public EfiMacAddress MCastFilter2;
    public EfiMacAddress MCastFilter3;
    public EfiMacAddress MCastFilter4;
    public EfiMacAddress MCastFilter5;
    public EfiMacAddress MCastFilter6;
    public EfiMacAddress MCastFilter7;
    public EfiMacAddress MCastFilter8;
    public EfiMacAddress MCastFilter9;
    public EfiMacAddress MCastFilter10;
    public EfiMacAddress MCastFilter11;
    public EfiMacAddress MCastFilter12;
    public EfiMacAddress MCastFilter13;
    public EfiMacAddress MCastFilter14;
    public EfiMacAddress MCastFilter15;
    public EfiMacAddress CurrentAddress;
    public EfiMacAddress BroadcastAddress;
    public EfiMacAddress PermanentAddress;
    public byte IfType;
    public bool MacAddressChangeable;
    public bool MultipleTxSupported;
    public bool MediaPresentSupported;
    public bool MediaPresent;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiIp4ConfigData
{
    public byte DefaultProtocol;
    public bool AcceptAnyProtocol;
    public bool AcceptIcmpErrors;
    public bool AcceptBroadcast;
    public bool AcceptPromiscuous;
    public bool UseDefaultAddress;
    public EfiIPv4Address StationAddress;
    public EfiIPv4Address SubnetMask;
    public byte TypeOfService;
    public byte TimeToLive;
    public bool DoNotFragment;
    public bool RawData;
    public uint ReceiveTimeout;
    public uint TransmitTimeout;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiIp4RouteTable
{
    public EfiIPv4Address SubnetAddress;
    public EfiIPv4Address SubnetMask;
    public EfiIPv4Address GatewayAddress;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiIp4IcmpType
{
    public byte Type;
    public byte Code;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiTcp4ConnectionToken
{
    ///
    /// The Status in the CompletionToken will be set to one of
    /// the following values if the active open succeeds or an unexpected
    /// error happens:
    /// EFI_SUCCESS:              The active open succeeds and the instance's
    ///                           state is Tcp4StateEstablished.
    /// EFI_CONNECTION_RESET:     The connect fails because the connection is reset
    ///                           either by instance itself or the communication peer.
    /// EFI_CONNECTION_REFUSED:   The connect fails because this connection is initiated with
    ///                           an active open and the connection is refused.
    /// EFI_ABORTED:              The active open is aborted.
    /// EFI_TIMEOUT:              The connection establishment timer expires and
    ///                           no more specific information is available.
    /// EFI_NETWORK_UNREACHABLE:  The active open fails because
    ///                           an ICMP network unreachable error is received.
    /// EFI_HOST_UNREACHABLE:     The active open fails because an
    ///                           ICMP host unreachable error is received.
    /// EFI_PROTOCOL_UNREACHABLE: The active open fails
    ///                           because an ICMP protocol unreachable error is received.
    /// EFI_PORT_UNREACHABLE:     The connection establishment
    ///                           timer times out and an ICMP port unreachable error is received.
    /// EFI_ICMP_ERROR:           The connection establishment timer timeout and some other ICMP
    ///                           error is received.
    /// EFI_DEVICE_ERROR:         An unexpected system or network error occurred.
    /// EFI_NO_MEDIA:             There was a media error.
    ///
    public EfiTcp4CompletionToken CompletionToken;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiTcp4CompletionToken
{
    public EfiEvent Event;
    public EfiStatus Status;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiTcp4ListenToken
{
    public EfiTcp4CompletionToken CompletionToken;
    public EfiHandle NewChildHandle;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiTcp4CloseToken
{
    public EfiTcp4CompletionToken CompletionToken;
    public bool AbortOnClose;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiTcp4FragmentData
{
    public uint FragmentLength;
    public void* FragmentBuffer;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiTcp4ReceiveData
{
    public bool UrgentFlag;
    public uint DataLength;
    public uint FragmentCount;
    public EfiTcp4FragmentData FragmentTable;
}

[StructLayout(LayoutKind.Sequential)]
public struct EfiTcp4TransmitData
{
    public bool Push;
    public bool Urgent;
    public uint DataLength;
    public uint FragmentCount;
    public EfiTcp4FragmentData FragmentTable;
}


[StructLayout(LayoutKind.Explicit)]
public unsafe struct EfiTcp4IoToken
{
    ///
    /// When transmission finishes or meets any unexpected error it will
    /// be set to one of the following values:
    /// EFI_SUCCESS:              The receiving or transmission operation
    ///                           completes successfully.
    /// EFI_CONNECTION_FIN:       The receiving operation fails because the communication peer
    ///                           has closed the connection and there is no more data in the
    ///                           receive buffer of the instance.
    /// EFI_CONNECTION_RESET:     The receiving or transmission operation fails
    ///                           because this connection is reset either by instance
    ///                           itself or the communication peer.
    /// EFI_ABORTED:              The receiving or transmission is aborted.
    /// EFI_TIMEOUT:              The transmission timer expires and no more
    ///                           specific information is available.
    /// EFI_NETWORK_UNREACHABLE:  The transmission fails
    ///                           because an ICMP network unreachable error is received.
    /// EFI_HOST_UNREACHABLE:     The transmission fails because an
    ///                           ICMP host unreachable error is received.
    /// EFI_PROTOCOL_UNREACHABLE: The transmission fails
    ///                           because an ICMP protocol unreachable error is received.
    /// EFI_PORT_UNREACHABLE:     The transmission fails and an
    ///                           ICMP port unreachable error is received.
    /// EFI_ICMP_ERROR:           The transmission fails and some other
    ///                           ICMP error is received.
    /// EFI_DEVICE_ERROR:         An unexpected system or network error occurs.
    /// EFI_NO_MEDIA:             There was a media error.
    ///
    [FieldOffset(0)]
    public EfiTcp4CompletionToken CompletionToken;
    [FieldOffset(16)]
    public EfiTcp4ReceiveData* RxData;
    [FieldOffset(16)]
    public EfiTcp4TransmitData* TxData;
}