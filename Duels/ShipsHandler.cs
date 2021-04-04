using System;
using System.Collections.Generic;
using UnityEngine;
using BWModLoader;

namespace Duels
{
    [Mod]
    public class ShipsHandler : MonoBehaviour
    {
        public static WakeNetObject wno;
        void Start()
        {
            wno = wno = GameMode.Instance.handler.åñîïòíæêêåî;
        }
        public static void spawnShip(Ships shipNum, int team)
        {
            if (GameMode.Instance.teamIsShip[team])
            {
                return;
            }
            else
            {
                DuelsGameMode.Instance.wno.îëæêéïåðæìå("allBuildShip", óëððîêðëóêó.îéäåéçèïïñí, new object[]
                {
                    Enum.GetName(typeof(Ships), shipNum).ToLower(),
                    team
                });
            }
        }
    }

    public enum Ships
    {
        Galleon = 1,
        Hoy = 2,
        Schooner = 3
    }
}
