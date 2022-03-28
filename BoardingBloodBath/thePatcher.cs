using Harmony;
using UnityEngine;
using System.Collections.Generic;

namespace BoardingBloodbath
{
	
	[HarmonyPatch(typeof(CaptainCommandWheel), "setCommand")]
	static class AbandonPatch
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020F0 File Offset: 0x000002F0
		private static bool Prefix(int íóóïíðóòêêè, int óîèòèðîðçîì, int íäêìòïçìóòä, int óêóêíðåîóêí, ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			return !BoardingBloodbathGameMode.Instance.Loaded;
		}
	}

	[HarmonyPatch(typeof(ShipMovement), "setSails")]
	static class SailPatch
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
		private static bool Prefix(int îîçêèîëìîæê, ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			return !BoardingBloodbathGameMode.Instance.Loaded;
		}
	}

	[HarmonyPatch(typeof(GameModeHandler), "win")]
	static class WinPatch
	{
		private static void Postfix(string ëëäíêðäóæîó, int íïïìîóðíçëæ, ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			BoardingBloodbathGameMode.Instance.Loaded = false;
			BoardingBloodbathGameMode.Instance.started = false;
		}
	}

	[HarmonyPatch(typeof(GameModeHandler), "êèîðñíóóïëé")]
	internal static class GamemodePatch
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		private static void Postfix(ref int __result)
		{
			Log.logger.Log("Did gm " + __result.ToString());
			if (BoardingBloodbathModeHandler.voteSucceded)
			{
				__result = 1;
			}
		}
	}

	[HarmonyPatch(typeof(PlayerHealth), "die")]
	internal static class PlayerDeathPatch
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		private static void Postfix(PlayerHealth __instance, Vector3 äåòéðññåîòì, int çññíïïíòóêê, bool îðòíæíæïðåê, bool ñïêêðêæíóòê, ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			if (BoardingBloodbathGameMode.Instance.started)
            {
				PlayerInfo plr = __instance.GetComponent<PlayerInfo>();
				if (plr != null)
                {
					if (GameMode.Instance.teamFactions[plr.team] == "pirates")
					{
						GameMode.Instance.pirateTickets--;
						if (GameMode.Instance.pirateTickets % BoardingBloodbathGameMode.Instance.pirateSteps == 0 || GameMode.Instance.pirateTickets < 10)
                        {
							BoardingBloodbathGameMode.updateTickets();
                        }
					}
                    else{
						GameMode.Instance.navyTickets--;
						if (GameMode.Instance.navyTickets % BoardingBloodbathGameMode.Instance.navySteps == 0 || GameMode.Instance.navyTickets < 10)
						{
							BoardingBloodbathGameMode.updateTickets();
						}
					}
                }

			}
		}
	}

	[HarmonyPatch(typeof(Chat), "sendChat")]
	static class ChatPatch
	{
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
				if (text.StartsWith("!force bb"))
				{
					Log.log("Force command used");
					string[] parameters = text.Split(new char[] { ' ' });
					if (parameters.Length >= 3)
					{
						int.TryParse(parameters[2], out int nextPreset);
						BoardingBloodbathModeHandler.forceVote(sender, info.éäñåíéíìééä, true, nextPreset);
					}
					else
					{
						BoardingBloodbathModeHandler.forceVote(sender, info.éäñåíéíìééä, false);
					}
					return false;
				}
				else if (text.StartsWith("!reload bb"))
                {
					Log.log("Reloading config");
					BoardingBloodbathGameMode.Instance.loadSettings();
					return false;
				}
			}

			if (text.StartsWith("!vote bb"))
            {
				Log.log("Got vote");
				BoardingBloodbathModeHandler.vote(sender, info.éäñåíéíìééä);
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
			return true;
			if (BoardingBloodbathGameMode.Instance.started)
            {
				if (GameMode.Instance.navyTickets < 0)
				{
					GameMode.Instance.handler.åñîïòíæêêåî.îëæêéïåðæìå("win", óëððîêðëóêó.îéäåéçèïïñí, new object[]
					{
					"Pirates",
					0
					});
					__result = true;

				}
                else if (GameMode.Instance.pirateTickets < 0)
                {
					GameMode.Instance.handler.åñîïòíæêêåî.îëæêéïåðæìå("win", óëððîêðëóêó.îéäåéçèïïñí, new object[]
					{
					"Navy",
					0
					});
					__result = true;
                }
                else
                {
					__result = false;
                }
				return false;
			}
            else
            {
				return true;
            }
		}
	}

	[HarmonyPatch(typeof(CannonUse), "äæäíêïæììðë")]
	static class cannonPatch
    {
		private static bool Prefix(string ëìåçäìääîéî, bool êóòæñåèêåéì)
        {
			return !BoardingBloodbathGameMode.Instance.started;
        }
    }

	[HarmonyPatch(typeof(SwivelUse), "fire")]
	static class swivelPatch
	{
		private static bool Prefix(ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			return !BoardingBloodbathGameMode.Instance.started;
		}
	}

	[HarmonyPatch(typeof(BotHandler), "Update")]
	static class botUpdatePatch
	{
		private static bool Prefix(BotHandler __instance)
		{
			if (BoardingBloodbathGameMode.Instance.started)
			{
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(BotHandler), "Start")]
	static class botStartPatch
	{
		private static bool Prefix(BotHandler __instance)
		{
			if (BoardingBloodbathGameMode.Instance.started)
			{
				BoardingBloodbathGameMode.Instance.StartCoroutine(BoardingBloodbathGameMode.Instance.bothandlerUpdateLoop(__instance));
			}
			return true;
		}
	}

}
