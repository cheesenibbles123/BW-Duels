using System;
using System.Collections.Generic;
using Harmony;

namespace Duels
{
	[HarmonyPatch(typeof(CaptainCommandWheel), "setCommand")]
	static class AbandonPatch
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020F0 File Offset: 0x000002F0
		private static bool Prefix(int íóóïíðóòêêè, int óîèòèðîðçîì, int íäêìòïçìóòä, int óêóêíðåîóêí, ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			return !DuelsGameMode.Instance.Loaded;
		}
	}

	[HarmonyPatch(typeof(ShipMovement), "setSails")]
	static class SailPatch
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
		private static bool Prefix(int ss, ïçîìäîóäìïæ.åéðñðçîîïêç info)
		{
			return !DuelsGameMode.Instance.Loaded;
		}
	}

	[HarmonyPatch(typeof(GameModeHandler), "win")]
	static class WinPatch
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020AC File Offset: 0x000002AC
		private static void Postfix(string ëëäíêðäóæîó, int íïïìîóðíçëæ, ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			DuelsGameMode.Instance.Loaded = false;
			DuelsGameMode.Instance.started = false;
		}
	}

	[HarmonyPatch(typeof(Chat), "sendChat")]
	static class ChatPatch
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000022A8 File Offset: 0x000004A8
		private static bool Prefix(int chatType, int senderTeam, string sender, string text, ïçîìäîóäìïæ.åéðñðçîîïêç info)
		{
			PlayerInfo playerBySocket = GameMode.getPlayerBySocket(info.éäñåíéíìééä);
			text = text.ToLower();

			if (text == "!xyz")
			{
				DuelsGameMode.Instance.wno.òäóæåòîððòä("broadcastChat", info.éäñåíéíìééä, new object[]
							{
								1,
								1,
								"game",
								string.Concat(new object[]
								{
									"X: ",
									playerBySocket.transform.position.x,
									" Y: ",
									playerBySocket.transform.position.y,
									" Z: ",
									playerBySocket.transform.position.z
								})
							});
				DuelsGameMode.Instance.wno.òäóæåòîððòä("broadcastChat", info.éäñåíéíìééä, new object[]
				{
								1,
								1,
								"game",
								string.Concat(new object[]
								{
									"rX: ",
									playerBySocket.transform.rotation.eulerAngles.x,
									" rY: ",
									playerBySocket.transform.rotation.eulerAngles.y,
									" rZ: ",
									playerBySocket.transform.rotation.eulerAngles.z
								})
				});
				return false;
			}else if (text == "!force")
            {
				ModeHandler.vote(sender,info.éäñåíéíìééä, true);
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(WinConditions), "ðñëåîçåêêðæ")]
	static class WinConditionPatch
	{
		private static bool Prefix(ref bool __result)
		{
			return !DuelsGameMode.Instance.started;
		}
	}
}
