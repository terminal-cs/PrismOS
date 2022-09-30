using Internal.EFI;

namespace PrismOS.Core
{
	public static unsafe class Power
	{
		/// <summary>
		/// Shutdown the system.
		/// </summary>
		public static void Shutdown()
		{
			EFI.GST->RuntimeServices->ResetSystem(EfiResetType.EfiResetShutdown, 0, null);
		}
		/// <summary>
		/// Restart the system.
		/// </summary>
		public static void Restart()
		{
			EFI.GST->RuntimeServices->ResetSystem(EfiResetType.EfiResetCold, 0, null);
		}
	}
}