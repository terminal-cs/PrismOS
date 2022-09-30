using Internal.Runtime.CompilerHelpers;
using System.Runtime.InteropServices;
using System.Runtime;
using System;

namespace Internal.EFI
{
	public static unsafe class EFI
	{
		static IntPtr _ST;
		static IntPtr _BS;
		static IntPtr _RT;

		public static EfiRuntimeServices* GRT => (EfiRuntimeServices*)_RT;
		public static EfiBootServices* GBS => (EfiBootServices*)_BS;
		public static EfiSystemTable* GST => (EfiSystemTable*)_ST;

		public static EfiHandle gImageHandle { get; set; }

		public static void InitializeLib(EfiHandle imageHandle, EfiSystemTable* systemTable)
		{
			_RT = (IntPtr)systemTable->RuntimeServices;
			_BS = (IntPtr)systemTable->BootServices;
			_ST = (IntPtr)systemTable;
			gImageHandle = imageHandle;

			InitializeGuid();

			EfiLoadedImageProtocol* loadedimage = null;
			GBS->HandleProtocol(gImageHandle, EfiLoadedImageProtocolGuid, (void**)&loadedimage);
			long ImageBase = (long)loadedimage->ImageBase;
			DOSHeader* doshdr = (DOSHeader*)ImageBase;
			NtHeaders64* nthdr = (NtHeaders64*)(ImageBase + doshdr->e_lfanew);
			SectionHeader* sections = ((SectionHeader*)(ImageBase + doshdr->e_lfanew + sizeof(NtHeaders64)));
			IntPtr moduleSec = IntPtr.Zero;
			for (int i = 0; i < nthdr->FileHeader.NumberOfSections; i++)
			{
				if (*(ulong*)sections[i].Name == 0x73656C75646F6D2E) moduleSec = (IntPtr)(ImageBase + sections[i].VirtualAddress);
			}
			StartupCodeHelpers.InitializeModules(moduleSec);
		}

		public static EfiGuid EfiGraphicsOutputProtocolGuid { get; set; }
		public static EfiGuid EfiTcp4ServiceBindingProtocolGuid { get; set; }
		public static EfiGuid EfiTcp4ProtocolGuid { get; set; }
		public static EfiGuid EfiLoadedImageProtocolGuid { get; set; }
		public static EfiGuid EfiSimpleFileSystemProtocolGuid { get; set; }
		public static EfiGuid EfiFileInfoGuid { get; set; }

		[RuntimeExport("EfiMain")]
		public static EfiStatus EfiMain(EfiHandle imageHandle, EfiSystemTable* systemTable)
		{
			InitializeLib(imageHandle, systemTable);

			//Disable watchdog timer
			GBS->SetWatchdogTimer(0, 0, 0, null);
			Console.Clear();
			PrismOS.Program.Main();

			return EfiStatus.EfiSuccess;
		}

		public static void InitializeGuid()
		{
			EfiGraphicsOutputProtocolGuid = new EfiGuid(0x9042a9de, 0x23dc, 0x4a38, 0x96, 0xfb, 0x7a, 0xde, 0xd0, 0x80, 0x51, 0x6a);
			EfiTcp4ServiceBindingProtocolGuid = new EfiGuid(0x00720665, 0x67eb, 0x4a99, 0xba, 0xf7, 0xd3, 0xc3, 0x3a, 0x1c, 0x7c, 0xc9);
			EfiTcp4ProtocolGuid = new EfiGuid(0x65530bc7, 0xa359, 0x410f, 0xb0, 0x10, 0x5a, 0xad, 0xc7, 0xec, 0x2b, 0x62);
			EfiLoadedImageProtocolGuid = new EfiGuid(0x5B1B31A1, 0x9562, 0x11d2, 0x8E, 0x3F, 0x00, 0xA0, 0xC9, 0x69, 0x72, 0x3B);
			EfiSimpleFileSystemProtocolGuid = new EfiGuid(0x964e5b22, 0x6459, 0x11d2, 0x8e, 0x39, 0x0, 0xa0, 0xc9, 0x69, 0x72, 0x3b);
			EfiFileInfoGuid = new EfiGuid(0x9576e92, 0x6d3f, 0x11d2, 0x8e, 0x39, 0x0, 0xa0, 0xc9, 0x69, 0x72, 0x3b);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DataDirectory
	{
		public uint VirtualAddress;
		public uint Size;
	}

	public enum DllCharacteristicsType : ushort
	{
		RES_0 = 0x0001,
		RES_1 = 0x0002,
		RES_2 = 0x0004,
		RES_3 = 0x0008,
		DynamicBase = 0x0040,
		ForceIntegrity = 0x0080,
		NxCompat = 0x0100,
		NoIsolation = 0x0200,
		NoSEH = 0x0400,
		NoBind = 0x0800,
		RES_4 = 0x1000,
		WDMDriver = 0x2000,
		TerminalServerName = 0x8000
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DOSHeader
	{
		public ushort e_magic;              // Magic number
		public ushort e_cblp;               // Bytes on last page of file
		public ushort e_cp;                 // Pages in file
		public ushort e_crlc;               // Relocations
		public ushort e_cparhdr;            // Size of header in paragraphs
		public ushort e_minalloc;           // Minimum extra paragraphs needed
		public ushort e_maxalloc;           // Maximum extra paragraphs needed
		public ushort e_ss;                 // Initial (relative) SS value
		public ushort e_sp;                 // Initial SP value
		public ushort e_csum;               // Checksum
		public ushort e_ip;                 // Initial IP value
		public ushort e_cs;                 // Initial (relative) CS value
		public ushort e_lfarlc;             // File address of relocation table
		public ushort e_ovno;               // Overlay number
		public fixed ushort e_res1[4];      // Reserved words
		public ushort e_oemid;              // OEM identifier (for e_oeminfo)
		public ushort e_oeminfo;            // OEM information; e_oemid specific
		public fixed ushort e_res2[10];     // Reserved words
		public int e_lfanew;                // File address of new exe header
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct FileHeader
	{
		public ushort Machine;
		public ushort NumberOfSections;
		public uint TimeDateStamp;
		public uint PointerToSymbolTable;
		public uint NumberOfSymbols;
		public ushort SizeOfOptionalHeader;
		public ushort Characteristics;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct NtHeaders64
	{
		public uint Signature;
		public FileHeader FileHeader;
		public OptionalHeaders64 OptionalHeader;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct OptionalHeaders64
	{
		public ushort Magic;
		public byte MajorLinkerVersion;
		public byte MinorLinkerVersion;
		public uint SizeOfCode;
		public uint SizeOfInitializedData;
		public uint SizeOfUninitializedData;
		public uint AddressOfEntryPoint;
		public uint BaseOfCode;
		public ulong ImageBase;
		public uint SectionAlignment;
		public uint FileAlignment;
		public ushort MajorOperatingSystemVersion;
		public ushort MinorOperatingSystemVersion;
		public ushort MajorImageVersion;
		public ushort MinorImageVersion;
		public ushort MajorSubsystemVersion;
		public ushort MinorSubsystemVersion;
		public uint Win32VersionValue;
		public uint SizeOfImage;
		public uint SizeOfHeaders;
		public uint CheckSum;
		public SubSystemType Subsystem;
		public DllCharacteristicsType DllCharacteristics;
		public ulong SizeOfStackReserve;
		public ulong SizeOfStackCommit;
		public ulong SizeOfHeapReserve;
		public ulong SizeOfHeapCommit;
		public uint LoaderFlags;
		public uint NumberOfRvaAndSizes;
		public DataDirectory ExportTable;
		public DataDirectory ImportTable;
		public DataDirectory ResourceTable;
		public DataDirectory ExceptionTable;
		public DataDirectory CertificateTable;
		public DataDirectory BaseRelocationTable;
		public DataDirectory Debug;
		public DataDirectory Architecture;
		public DataDirectory GlobalPtr;
		public DataDirectory TLSTable;
		public DataDirectory LoadConfigTable;
		public DataDirectory BoundImport;
		public DataDirectory IAT;
		public DataDirectory DelayImportDescriptor;
		public DataDirectory CLRRuntimeHeader;
		public DataDirectory Reserved;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SectionHeader
	{
		public fixed byte Name[8];
		public uint PhysicalAddress_VirtualSize;
		public uint VirtualAddress;
		public uint SizeOfRawData;
		public uint PointerToRawData;
		public uint PointerToRelocations;
		public uint PointerToLineNumbers;
		public ushort NumberOfRelocations;
		public ushort NumberOfLineNumbers;
		public uint Characteristics;
	}

	public enum SubSystemType : ushort
	{
		Unknown = 0,
		Native = 1,
		WindowsGUI = 2,
		WindowsCUI = 3,
		PosixCUI = 7,
		WindowsCEGui = 9,
		EfiApplication = 10,
		EfiBootServiceDriver = 11,
		EfiRuntimeDriver = 12,
		EfiRom = 13,
		Xbox = 14
	}
}