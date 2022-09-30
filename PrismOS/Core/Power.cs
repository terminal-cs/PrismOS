namespace PrismOS.Core
{
	public static unsafe class Power
	{
		/// <summary>
		/// Shutdown the system.
		/// </summary>
		public static void Shutdown()
		{
			GST->RuntimeServices->ResetSystem(EfiResetType.EfiResetShutdown, 0, null);
		}
		/// <summary>
		/// Restart the system.
		/// </summary>
		public static void Restart()
		{
			GST->RuntimeServices->ResetSystem(EfiResetType.EfiResetCold, 0, null);
		}
	}
}