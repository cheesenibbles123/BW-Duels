using System;
using System.Collections.Generic;
using BWModLoader;
using UnityEngine;

namespace BoardingBloodbath
{
    [Mod]
    public class ModeHandler : MonoBehaviour
    {

        public static List<string> usersVoted = new List<string>();
        public static bool voteSucceded = false;

        public static void vote(string pName, int pSocket, bool isForced)
        {
            if (usersVoted.Contains(pName))
            {
                // To User only
                if (isForced)
                {
                    MainGameMode.Instance.wno.òäóæåòîððòä("broadcastChat", pSocket, new object[]
                    {
                    1,
                    1,
                    "game",
                    "You have already forced."
                    });
                }
                else
                {
                    UI.Instance.GetComponent<WakeNetObject>().îëæêéïåðæìå("sendWarning", óëððîêðëóêó.îéäåéçèïïñí, new object[]
                    {
                        "all",
                        "Switching to BoardingBloodbath mode..."
                    });
                    voteSucceded = true;
                }
            }
            else
            {
                usersVoted.Add(pName);

                // To User only
                MainGameMode.Instance.wno.òäóæåòîððòä("broadcastChat", pSocket, new object[]
                {
                    1,
                    1,
                    "game",
                    "Your vote has been counted."
                });

                // To all users
                MainGameMode.Instance.wno.îëæêéïåðæìå("broadcastChat", óëððîêðëóêó.îéäåéçèïïñí, new object[]
                {
                    1,
                    1,
                    "game",
                    string.Concat(new string[]
                    {
                        "User has voted for 'BoardingBloodbath' (",
                        usersVoted.Count.ToString(),
                        "/",
                        Math.Ceiling((double)GameMode.Instance.Players.Count / 2.0).ToString(),
                        ")"
                    })
                });

                if ((double)usersVoted.Count >= Math.Ceiling((double)GameMode.Instance.Players.Count / 2.0) || isForced)
                {
                    UI.Instance.GetComponent<WakeNetObject>().îëæêéïåðæìå("sendWarning", óëððîêðëóêó.îéäåéçèïïñí, new object[]
                    {
                        "all",
                        "Switching to Boarding Bloodbath mode..."
                    });
                    voteSucceded = true;
                }
            }
        }
    }
}
