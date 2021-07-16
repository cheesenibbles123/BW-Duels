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

	[HarmonyPatch(typeof(PlayerHealth), "die")]
	internal static class PlayerDeathPatch
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		private static void Postfix(PlayerHealth __instance, Vector3 äåòéðññåîòì, int çññíïïíòóêê, bool îðòíæíæïðåê, bool ñïêêðêæíóòê, ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			if (MainGameMode.Instance.started)
            {
				PlayerInfo plr = __instance.GetComponent<PlayerInfo>();
				if (plr != null)
                {
					if (GameMode.Instance.teamFactions[plr.team] == "pirates")
					{
						GameMode.Instance.pirateTickets--;
						if (GameMode.Instance.pirateTickets % MainGameMode.Instance.pirateSteps == 0 || GameMode.Instance.pirateTickets < 10)
                        {
							MainGameMode.updateTickets();
                        }
					}
                    else{
						GameMode.Instance.navyTickets--;
						if (GameMode.Instance.navyTickets % MainGameMode.Instance.navySteps == 0 || GameMode.Instance.navyTickets < 10)
						{
							MainGameMode.updateTickets();
						}
					}
                }

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
				if (text.StartsWith("!xyz"))
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
				else if (text.StartsWith("!force"))
				{
					Log.log("force command used");
					ModeHandler.vote(sender, info.éäñåíéíìééä, true);
					return false;
				}
				else if (text.StartsWith("!reload"))
                {
					Log.log("Reloading config");
					MainGameMode.Instance.loadSettings();
					return false;
				}else if (text.StartsWith("!location"))
                {
					string[] txt = text.Split(' ');
					if (txt.Length == 2)
                    {
						Vector3 pos;
						Vector3 rot;
						switch (txt[1])
                        {
							case "0":
								pos = GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position; // Distance from each other, height, distance from center
								rot = GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles;
								break;
							case "1":
								pos = GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position;
								rot = GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles;
								break;
							case "2":
								pos = GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position;
								rot = GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles;
								break;
							case "4":
								pos = GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position;
								rot = GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles;
								break;
							case "5":
								pos = GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position;
								rot = GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles;
								break;
							case "6":
								pos = GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position;
								rot = GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles;
								break;
                            default:
								pos = new Vector3(0, 0, 0);
								rot = new Vector3(0, 0, 0);
								break;
                        }
						MainGameMode.Instance.wno.òäóæåòîððòä("broadcastChat", info.éäñåíéíìééä, new object[]
						{
							1,
							1,
							"game",
							string.Concat(new object[]
							{
								"X: ",
								pos.x,
								" Y: ",
								pos.y,
								" Z: ",
								pos.z
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
								rot.x,
								" rY: ",
								rot.y,
								" rZ: ",
								rot.z
							})
						});
						return false;
					}
                }else if (text.StartsWith("!tp"))
                {
					string[] txt = text.Split(' ');
					
					Vector3 newPos = new Vector3(float.Parse(txt[2]), float.Parse(txt[3]), float.Parse(txt[4]));
					switch (txt[1])
					{
						case "0":
							GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = newPos; // Distance from each other, height, distance from center
							break;
						case "1":
							GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = newPos;
							break;
						case "2":
							GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = newPos;
							break;
						case "4":
							GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = newPos;
							break;
						case "5":
							GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = newPos;
							break;
						case "6":
							GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = newPos;
							break;
					}
					Debug.Log($"Set team {txt[1]} to location {txt[2]}:{txt[3]}:{txt[4]}");
				}
				else if (text.StartsWith("!rot"))
				{
					string[] txt = text.Split(' ');

					Vector3 newRot = new Vector3(float.Parse(txt[2]), float.Parse(txt[3]), float.Parse(txt[4]));
					switch (txt[1])
					{
						case "0":
							GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = newRot; // Distance from each other, height, distance from center
							break;
						case "1":
							GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = newRot;
							break;
						case "2":
							GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = newRot;
							break;
						case "4":
							GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = newRot;
							break;
						case "5":
							GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = newRot;
							break;
						case "6":
							GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = newRot;
							break;
					}
					Debug.Log($"Set team {txt[1]} to rot {txt[2]}:{txt[3]}:{txt[4]}");
				}
			}

			if (text.StartsWith("!vote"))
            {
				Log.log("Got vote");
				ModeHandler.vote(sender, info.éäñåíéíìééä, false);
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
			if (MainGameMode.Instance.started)
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
			return !MainGameMode.Instance.started;
        }
    }

	[HarmonyPatch(typeof(SwivelUse), "fire")]
	static class swivelPatch
	{
		private static bool Prefix(ïçîìäîóäìïæ.åéðñðçîîïêç äíìíëðñïñéè)
		{
			return !MainGameMode.Instance.started;
		}
	}

	[HarmonyPatch(typeof(BotHandler), "Update")]
	static class botPatch
	{
		private static bool Prefix(BotHandler __instance)
		{
			if (MainGameMode.Instance.started)
			{
				for (int i = 0; i < __instance.êóæìíîìñäîí.Length; i++)
				{
					__instance.êóæìíîìñäîí[i].GetComponent<BotPlayer>().åñîïòíæêêåî.îëæêéïåðæìå("Unload", óëððîêðëóêó.îéäåéçèïïñí, new object[0]);
				}
				return false;
			}
			return true;
		}
	}

}
