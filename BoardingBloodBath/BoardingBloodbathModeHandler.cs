using System;
using System.Collections.Generic;
using BWModLoader;
using UnityEngine;

namespace BoardingBloodbath
{
    [Mod]
    public class BoardingBloodbathModeHandler : MonoBehaviour
    {

        public static List<string> usersVoted = new List<string>();
        public static bool voteSucceded = false;

        public static void vote(string pName, int pSocket, bool isForced)
        {
            if (usersVoted.Contains(pName))
            {
                // To User only
                if (isForced && voteSucceded)
                {
                    BoardingBloodbathGameMode.Instance.wno.òäóæåòîððòä("broadcastChat", pSocket, new object[]
                    {
                        1,
                        1,
                        "game",
                        "The gamemode has already won the vote!"
                    });
                }
                else if (isForced)
                {
                    UI.Instance.GetComponent<WakeNetObject>().îëæêéïåðæìå("sendWarning", óëððîêðëóêó.îéäåéçèïïñí, new object[]
                    {
                        "all",
                        "Switching to Boarding Bloodbath mode next round..."
                    });
                    voteSucceded = true;
                }
                else
                {
                    BoardingBloodbathGameMode.Instance.wno.òäóæåòîððòä("broadcastChat", pSocket, new object[]
                    {
                        1,
                        1, 
                        "game",
                        "You have already voted!"
                    });
                }
            }
            else
            {
                usersVoted.Add(pName);

                // To User only
                BoardingBloodbathGameMode.Instance.wno.òäóæåòîððòä("broadcastChat", pSocket, new object[]
                {
                    1,
                    1,
                    "game",
                    "Your vote has been counted."
                });

                // To all users
                BoardingBloodbathGameMode.Instance.wno.îëæêéïåðæìå("broadcastChat", óëððîêðëóêó.îéäåéçèïïñí, new object[]
                {
                    1,
                    1,
                    "game",
                    string.Concat(new string[]
                    {
                        "User has voted for 'BoardingBloodbath' (",
                        usersVoted.Count.ToString(),
                        "/",
                        Math.Ceiling(GameMode.Instance.Players.Count / 2.0).ToString(),
                        ")",
                        " using '!vote' in chat!"
                    })
                });

                if ((usersVoted.Count >= Math.Ceiling(GameMode.Instance.Players.Count / 2.0) || isForced) && !voteSucceded)
                {
                    UI.Instance.GetComponent<WakeNetObject>().îëæêéïåðæìå("sendWarning", óëððîêðëóêó.îéäåéçèïïñí, new object[]
                    {
                        "all",
                        "Switching to Boarding Bloodbath mode next round..."
                    });
                    voteSucceded = true;
                }
            }
        }
    }
}
