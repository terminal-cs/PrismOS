using System;

namespace PrismOS
{
    public unsafe class Program
    {
        // QEMU corrupts BOOTX64.efi, which is probably why it doesn't work on real hardware, please notice!
        // If you want to running this on real hardware, please press ctrl+b to rebuild this repo and copy
        // files from Disk folder to your usb drive, of course you have to format your usb drive to fat32 and
        // use GUID partition table first
        public static void Main()
        {
            Console.WriteLine("Hello, World!");
			while (true) { }
        }
    }
}