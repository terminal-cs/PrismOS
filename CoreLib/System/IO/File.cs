namespace System.IO
{
    public static unsafe class File
    {
        public static byte[] ReadAllBytes(string path)
        {
            EfiLoadedImageProtocol* loadedimage = null;
            EfiSimpleFileSystemProtocol* simplefilesystem = null;
            GBS->HandleProtocol(gImageHandle, EfiLoadedImageProtocolGuid, (void**)&loadedimage);
            GBS->HandleProtocol(loadedimage->DeviceHandle, EfiSimpleFileSystemProtocolGuid, (void**)&simplefilesystem);
            EfiFileHandle* vol = null;
            simplefilesystem->OpenVolume(simplefilesystem, &vol);
            EfiFileHandle* file = null;
            fixed (char* ptr = path)
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

        public static void WriteAllBytes(string path, byte[] buffer)
        {
            File.Delete(path);
            EfiLoadedImageProtocol* loadedimage = null;
            EfiSimpleFileSystemProtocol* simplefilesystem = null;
            GBS->HandleProtocol(gImageHandle, EfiLoadedImageProtocolGuid, (void**)&loadedimage);
            GBS->HandleProtocol(loadedimage->DeviceHandle, EfiSimpleFileSystemProtocolGuid, (void**)&simplefilesystem);
            EfiFileHandle* vol = null;
            simplefilesystem->OpenVolume(simplefilesystem, &vol);
            EfiFileHandle* file = null;
            fixed (char* ptr = path)
                vol->Open(vol, &file, ptr, EfiFileMode.Read | EfiFileMode.Write | EfiFileMode.Create, 0);
            ulong size = (ulong)buffer.Length;
            fixed (byte* pbuf = buffer)
                file->Write(file, &size, pbuf);
            file->Flush(file);
            file->Close(file);
            vol->Flush(vol);
            vol->Close(vol);
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
