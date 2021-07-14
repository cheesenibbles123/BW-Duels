using Harmony;
using UnityEngine;

namespace BoardingBloodbath
{
	[HarmonyPatch(typeof(CaptainCommandWheel), "setCommand")]
	static class AbandonPatch
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020F0 File Offset: 0x000002F0
		private static bool Prefix(int íóóïíðóòêêè, int óîèòèðîðçîì, int íäêìòïçìóòä, int óêóêíðåîóêí, ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			return !MainGameMode.Instance.Loaded;
		}
	}

	[HarmonyPatch(typeof(ShipMovement), "setSails")]
	static class SailPatch
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
		private static bool Prefix(int îîçêèîëìîæê, ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			return !MainGameMode.Instance.Loaded;
		}
	}

	[HarmonyPatch(typeof(GameModeHandler), "win")]
	static class WinPatch
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020AC File Offset: 0x000002AC
		private static void Postfix(string ëëäíêðäóæîó, int íïïìîóðíçëæ, ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			MainGameMode.Instance.Loaded = false;
			MainGameMode.Instance.started = false;
		}
	}

	[HarmonyPatch(typeof(GameModeHandler), "êèîðñíóóïëé")]
	internal static class GamemodePatch
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		private static void Postfix(ref int __result)
		{
			Log.logger.Log("Did gm " + __result.ToString());
			if (ModeHandler.voteSucceded)
			{
				__result = 1;
			}
		}
	}

	[HarmonyPatch(typeof(Chat), "sendChat")]
	static class ChatPatch
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000022A8 File Offset: 0x000004A8
		private static bool Prefix(int chatType, int senderTeam, string sender, string text, ïçîìäîóäìïæ.åéðñðçîîïêç info)
		{
			Log.log("Chat patch called");
			PlayerInfo playerBySocket = GameMode.getPlayerBySocket(info.éäñåíéíìééä);
			text = text.ToLower();
			Log.log(text);

			ulong steamID = GameMode.getPlayerBySocket(info.éäñåíéíìééä).steamPlayer.ðñéèéåóëìêé.m_SteamID;
			bool isAdmin = éæñêääóîîèò.ðïîñðçòäêëæ(steamID);
			if (isAdmin)
			{
				if (text == "!xyz")
				{
					Log.log("xyz command used");
					MainGameMode.Instance.wno.òäóæåòîððòä("broadcastChat", info.éäñåíéíìééä, new object[]
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
					MainGameMode.Instance.wno.òäóæåòîððòä("broadcastChat", info.éäñåíéíìééä, new object[]
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
				}
				else if (text == "!force")
				{
					Log.log("force command used");
					ModeHandler.vote(sender, info.éäñåíéíìééä, true);
					return false;
				}
				else if (text == "!reload")
                {
					Log.log("Reloading config");
					MainGameMode.Instance.loadSettings();
					return false;
				}
				return true;
			}
			return true;
		}
	}
	/*
	[HarmonyPatch(typeof(WinConditions), "ðñëåîçåêêðæ")]
	static class WinConditionPatch
	{
		private static bool Prefix(ref bool __result)
		{
			return !MainGameMode.Instance.started;
		}
	}
	*/

	[HarmonyPatch(typeof(Cannonball), "ðñëåîçåêêðæ")]
	static class cannonballPatch
    {
		private static bool Prefix()
        {
			return !MainGameMode.Instance.started;
        }
    }

	[HarmonyPatch(typeof(SwivelUse), "onThisObjectUsed")]
	static class swivelPatch
	{
		private static bool Prefix()
		{
			return !MainGameMode.Instance.started;
		}
	}

	[HarmonyPatch(typeof(BotPlayer), "Init")]
	static class botPatch
	{
		private static bool Prefix(BotPlayer __instance)
		{
			return !MainGameMode.Instance.started;
		}
	}

}
