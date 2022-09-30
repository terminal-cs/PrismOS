using System.Runtime.InteropServices;

namespace Internal.EFI
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct EfiTcpConfigData
    {
        public byte TypeOfService;
        public byte TimeToLive;
        public EfiTcp4AccessPoint AccessPoint;
        public EfiTcp4Option* ControlOption;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EfiTcp4AccessPoint
    {
        public bool UseDefaultAddress;
        public EfiIPv4Address StationAddress;
        public EfiIPv4Address SubnetMask;
        public ushort StationPort;
        public EfiIPv4Address RemoteAddress;
        public ushort RemotePort;
        public bool ActiveFlag;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EfiTcp4Option
    {
        public uint ReceiveBufferSize;
        public uint SendBufferSize;
        public uint MaxSynBackLog;
        public uint ConnectionTimeout;
        public uint DataRetries;
        public uint FinTimeout;
        public uint TimeWaitTimeout;
        public uint KeepAliveProbes;
        public uint KeepAliveTime;
        public uint KeepAliveInterval;
        public bool EnableNagle;
        public bool EnableTimeStamp;
        public bool EnableWindowScaling;
        public bool EnableSelectiveAck;
        public bool EnablePAthMtuDiscovery;
    }

    public enum EfiTcp4ConnectionState
    {
        Tcp4StateClosed = 0,
        Tcp4StateListen = 1,
        Tcp4StateSynSent = 2,
        Tcp4StateSynReceived = 3,
        Tcp4StateEstablished = 4,
        Tcp4StateFinWait1 = 5,
        Tcp4StateFinWait2 = 6,
        Tcp4StateClosing = 7,
        Tcp4StateTimeWait = 8,
        Tcp4StateCloseWait = 9,
        Tcp4StateLastAck = 10
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct EfiTcp4Protocol
    {
        public delegate* unmanaged<EfiTcp4Protocol*, EfiTcp4ConnectionState*, EfiTcpConfigData*, EfiIp4ModeData*, EfiManagedNetworkConfigData*, EfiSimpleNetworkMode*, EfiStatus> GetModeData;
        public delegate* unmanaged<EfiTcp4Protocol*, EfiTcpConfigData*, EfiStatus> Configure;
        public delegate* unmanaged<EfiTcp4Protocol*, bool, EfiIPv4Address*, EfiIPv4Address*, EfiIPv4Address*, EfiStatus> Routes;
        public delegate* unmanaged<EfiTcp4Protocol*, EfiTcp4ConnectionToken*, EfiStatus> Connect;
        public delegate* unmanaged<EfiTcp4Protocol*, EfiTcp4ListenToken*, EfiStatus> Accept;
        public delegate* unmanaged<EfiTcp4Protocol*, EfiTcp4IoToken*, EfiStatus> Transmit;
        public delegate* unmanaged<EfiTcp4Protocol*, EfiTcp4IoToken*, EfiStatus> Receive;
        public delegate* unmanaged<EfiTcp4Protocol*, EfiTcp4CloseToken*, EfiStatus> Close;
        public delegate* unmanaged<EfiTcp4Protocol*, EfiTcp4CompletionToken*, EfiStatus> Cancel;
        public delegate* unmanaged<EfiTcp4Protocol*, EfiStatus> Poll;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct EfiIp4ModeData
    {
        public bool IsStarted;
        public uint MaxPacketSize;
        public EfiIp4ConfigData ConfigData;
        public bool IsConfigured;
        public uint GroupCount;
        public EfiIPv4Address* GroupTable;
        public uint RouteCount;
        public EfiIp4RouteTable* RouteTable;
        public uint IcmpTypeCount;
        public EfiIp4IcmpType* IcmpTypeList;
    }
}