using static Internal.EFI.EFI;
using System.Text.Encoding;
using Internal.EFI;

namespace System.IO
{
    public static unsafe class File
    {
        public static byte[] ReadAllBytes(string Path)
        {
            EfiLoadedImageProtocol* loadedimage = null;
            EfiSimpleFileSystemProtocol* simplefilesystem = null;
            GBS->HandleProtocol(gImageHandle, EfiLoadedImageProtocolGuid, (void**)&loadedimage);
            GBS->HandleProtocol(loadedimage->DeviceHandle, EfiSimpleFileSystemProtocolGuid, (void**)&simplefilesystem);
            EfiFileHandle* vol = null;
            simplefilesystem->OpenVolume(simplefilesystem, &vol);
            EfiFileHandle* file = null;
            fixed (char* ptr = Path)
                vol->Open(vol, &file, ptr, EfiFileMode.Read, 0);
            EfiFileInfo info = new EfiFileInfo();
            ulong fileinfosize = (ulong)sizeof(EfiFileInfo);
            file->GetInfo(file, EfiFileInfoGuid, &fileinfosize, &info);
            byte[] buffer = new byte[info.FileSize];
            fixed (byte* pbuf = buffer)
                file->Read(file, &info.FileSize, pbuf);
            file->Close(file);
            vol->Close(vol);
            return buffer;
        }
        public static string ReadAllText(string Path)
        {
            return UTF8.GetString(ReadAllBytes(Path));
        }

        public static void WriteAllBytes(string Path, byte[] Buffer)
        {
            Delete(Path);
            EfiLoadedImageProtocol* loadedimage = null;
            EfiSimpleFileSystemProtocol* simplefilesystem = null;
            GBS->HandleProtocol(gImageHandle, EfiLoadedImageProtocolGuid, (void**)&loadedimage);
            GBS->HandleProtocol(loadedimage->DeviceHandle, EfiSimpleFileSystemProtocolGuid, (void**)&simplefilesystem);
            EfiFileHandle* vol = null;
            simplefilesystem->OpenVolume(simplefilesystem, &vol);
            EfiFileHandle* file = null;
            fixed (char* ptr = Path)
                vol->Open(vol, &file, ptr, EfiFileMode.Read | EfiFileMode.Write | EfiFileMode.Create, 0);
            ulong size = (ulong)Buffer.Length;
            fixed (byte* pbuf = Buffer)
                file->Write(file, &size, pbuf);
            file->Flush(file);
            file->Close(file);
            vol->Flush(vol);
            vol->Close(vol);
        }
        public static void WriteAllText(string Path, string Contents)
		{
            WriteAllBytes(Path, UTF8.GetBytes(Contents));
		}

        public static void Delete(string path)
        {
            EfiLoadedImageProtocol* loadedimage = null;
            EfiSimpleFileSystemProtocol* simplefilesystem = null;
            GBS->HandleProtocol(gImageHandle, EfiLoadedImageProtocolGuid, (void**)&loadedimage);
            GBS->HandleProtocol(loadedimage->DeviceHandle, EfiSimpleFileSystemProtocolGuid, (void**)&simplefilesystem);
            EfiFileHandle* vol = null;
            simplefilesystem->OpenVolume(simplefilesystem, &vol);
            EfiFileHandle* file = null;
            fixed (char* ptr = path)
                vol->Open(vol, &file, ptr, EfiFileMode.Read | EfiFileMode.Write | EfiFileMode.Create, 0);
            file->Delete(file);
            vol->Flush(vol);
            vol->Close(vol);
        }
    }
}
