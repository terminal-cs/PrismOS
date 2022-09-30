using System.Runtime.InteropServices;
using static Internal.EFI.EFI;
using Internal.EFI;

namespace System.Net.Sockets
{
    public unsafe class Socket
    {
        EfiTcp4Protocol* tcp;
        EfiTcp4ReceiveData receiveData;
        EfiTcp4TransmitData transmitData;
        EfiTcp4IoToken receiveToken;
        EfiTcp4IoToken transmitToken;
        EfiTcpConfigData configuration;
        bool receiveDone;
        bool transmitDone;

        public Socket(SocketType socketType, ProtocolType protocolType)
        {
            if (socketType != SocketType.Stream || protocolType != ProtocolType.Tcp)
            {
                Console.WriteLine("Unsupported socket type!");
                for (; ; );
            }
        }

        public void Connect(EfiIPv4Address address, ushort port)
        {
            EfiStatus sts = EfiStatus.EfiSuccess;

            configuration = new EfiTcpConfigData();
            configuration.TimeToLive = 188;
            configuration.AccessPoint.UseDefaultAddress = true;

            configuration.AccessPoint.ActiveFlag = true;
            configuration.AccessPoint.RemotePort = port;
            configuration.AccessPoint.RemoteAddress = address;

            ulong numdevices;
            EfiHandle* devices;

            sts = EFI.GBS->LocateHandleBuffer(
                EfiLocateSearchType.ByProtocol,
                EfiTcp4ServiceBindingProtocolGuid,
                null,
                &numdevices,
                &devices
                );

            if(sts == EfiStatus.EfiNotFound)
            {
                Console.WriteLine("Your UEFI firmware does not support TCP!");
                Console.WriteLine("Make sure you have \"Network Stack\" enabled on your BIOS!");
                for (; ; );
            }

            EfiHandle dev = devices[0];

            EfiServiceBindingProtocol* bs;

            GBS->OpenProtocol(
                dev,
                EfiTcp4ServiceBindingProtocolGuid,
                (void**)&bs,
                gImageHandle,
                default,
                EfiProtocolAttributes.EFI_OPEN_PROTOCOL_GET_PROTOCOL
                );

            EfiHandle tcphandle;

            bs->CreateChild(bs, &tcphandle);

            fixed (EfiTcp4Protocol** pcli = &tcp)
                GBS->OpenProtocol(
                    tcphandle,
                    EfiTcp4ProtocolGuid,
                    (void**)pcli,
                    gImageHandle,
                    default,
                    EfiProtocolAttributes.EFI_OPEN_PROTOCOL_GET_PROTOCOL
                    );

            receiveData = new EfiTcp4ReceiveData();
            transmitData = new EfiTcp4TransmitData();
            receiveToken = new EfiTcp4IoToken();
            transmitToken = new EfiTcp4IoToken();

            receiveData.FragmentCount = 1;

            fixed (EfiTcp4ReceiveData* prx = &receiveData)
                receiveToken.RxData = prx;

            transmitData.FragmentCount = 1;
            transmitData.Push = true;
            fixed (EfiTcp4TransmitData* ptx = &transmitData)
                transmitToken.TxData = ptx;

            fixed (EfiTcpConfigData* pcfg = &configuration)
                sts = tcp->Configure(tcp, pcfg);

            EfiIp4ModeData mode = new EfiIp4ModeData();

            if (sts == EfiStatus.EfiNoMapping)
            {
                Console.WriteLine("Trying to get an IP from DHCP server...");

                do
                {
                    tcp->GetModeData(tcp, null, null, &mode, null, null);
                } while (!mode.IsConfigured);
                fixed (EfiTcpConfigData* pcfg = &configuration)
                    tcp->Configure(tcp, pcfg);

                Console.Write("Your IP is: ");
                Console.Write(Convert.ToString(mode.ConfigData.StationAddress.Addr[0], 10));
                Console.Write('.');
                Console.Write(Convert.ToString(mode.ConfigData.StationAddress.Addr[1], 10));
                Console.Write('.');
                Console.Write(Convert.ToString(mode.ConfigData.StationAddress.Addr[2], 10));
                Console.Write('.');
                Console.Write(Convert.ToString(mode.ConfigData.StationAddress.Addr[3], 10));
                Console.WriteLine();
            }

            EfiTcp4ConnectionToken conn;

            fixed (bool* preceiveDone = &receiveDone)
            fixed (EfiTcp4IoToken* preceiveToken = &receiveToken)
                GBS->CreateEvent(
                    EfiEventType.EvtNotifySignal,
                    EfiTpl.TplNotify,
                    &OnEvent,
                    preceiveDone,
                    &preceiveToken->CompletionToken.Event
                    );

            fixed (bool* ptransmitDone = &transmitDone)
            fixed (EfiTcp4IoToken* ptransmitToken = &transmitToken)
                GBS->CreateEvent(
                    EfiEventType.EvtNotifySignal,
                    EfiTpl.TplNotify,
                    &OnEvent,
                    ptransmitDone,
                    &ptransmitToken->CompletionToken.Event
                    );

            bool isConnected = false;

            GBS->CreateEvent(
                EfiEventType.EvtNotifySignal,
                EfiTpl.TplNotify,
                &OnEvent,
                &isConnected,
                &conn.CompletionToken.Event
                );

            tcp->Connect(tcp, &conn);

            while (!isConnected) ;

            GBS->CloseEvent(conn.CompletionToken.Event);
        }

        public void Send(byte[] buffer)
        {
            transmitDone = false;

            transmitData.DataLength = (uint)buffer.Length;
            transmitData.FragmentCount = 1;
            transmitData.FragmentTable.FragmentLength = (uint)buffer.Length;
            fixed (byte* pbuf = buffer)
                transmitData.FragmentTable.FragmentBuffer = pbuf;

            fixed (EfiTcp4IoToken* ptransmitToken = &transmitToken)
                tcp->Transmit(tcp, ptransmitToken);

            while (!transmitDone) ;
        }

        public int Receive(byte[] buffer)
        {
            receiveDone = false;

            receiveData.DataLength = (uint)buffer.Length;
            receiveData.FragmentTable.FragmentLength = (uint)buffer.Length;
            fixed (byte* pbuf = buffer)
                receiveData.FragmentTable.FragmentBuffer = pbuf;

            fixed (EfiTcp4IoToken* preceiveToken = &receiveToken)
                tcp->Receive(tcp, preceiveToken);

            while (!receiveDone) tcp->Poll(tcp);

            return (int)receiveData.FragmentTable.FragmentLength;
        }

        [UnmanagedCallersOnly]
        public static void OnEvent(EfiEvent e, void* ctx)
        {
            if (ctx != null)
            {
                *(bool*)ctx = true;
            }
        }

        internal void Close()
        {
            bool isClosed = false;
            EfiTcp4CloseToken closeToken;
            GBS->CreateEvent(
                EfiEventType.EvtNotifySignal,
                EfiTpl.TplNotify,
                &OnEvent,
                &isClosed,
                &closeToken.CompletionToken.Event
                );

            tcp->Close(tcp, &closeToken);

            while (!isClosed) ;

            GBS->CloseEvent(receiveToken.CompletionToken.Event);
            GBS->CloseEvent(transmitToken.CompletionToken.Event);
            GBS->CloseEvent(closeToken.CompletionToken.Event);
        }
    }
}
