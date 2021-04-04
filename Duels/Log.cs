using BWModLoader;

namespace Duels
{
	internal static class Log
	{
		// Token: 0x04000001 RID: 1
		public static readonly ModLogger logger = new ModLogger("[DuelsGameMode]", ModLoader.LogPath + "\\DuelsGameMode.txt");

		public static void log(string message)
        {
			logger.Log(message);
        }
	}
}
