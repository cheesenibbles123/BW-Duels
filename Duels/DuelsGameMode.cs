using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BWModLoader;
using Harmony;

namespace Duels
{
    [Mod]
    public class DuelsGameMode : MonoBehaviour
    {
        public static DuelsGameMode Instance;
        public WakeNetObject wno;
        public static int GamemodeID;
        public bool Loaded = false;
        public bool started = false;

        private void Awake()
        {
            Debug.Log("Duels has been awakened");
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(this);
            }
        }

        private void Start()
        {
            GameMode.Instance.resetEvent += this.reset;
            wno = GameMode.Instance.handler.åñîïòíæêêåî;
            HarmonyInstance harmonyInstance = HarmonyInstance.Create("com.github.archie");
            harmonyInstance.PatchAll();
            Log.log("Started mod");
        }

        public void reset()
        {
            started = false;
            if (ModeHandler.voteSucceded)
            {
                ModeHandler.usersVoted.Clear();
                Loaded = true;
                ModeHandler.voteSucceded = false;
                Log.log("Starting coroutine");
                Instance.StartCoroutine(Instance.gameModeStart());
            }
            else
            {
                Loaded = false;
            }
        }

        private IEnumerator gameModeStart()
        {
            Log.log("Entered gameModeStart");
            wno.îëæêéïåðæìå("broadcastChat", óëððîêðëóêó.îéäåéçèïïñí, new object[]
            {
                1,
                1,
                "game",
                "Loading Duels mode..."
            });
            Log.log("Waiting 20sec");
            yield return new WaitForSeconds(20f);
            Log.log("Passed 20sec");
            ShipsHandler.spawnShip(Ships.Schooner, 0);
            ShipsHandler.spawnShip(Ships.Galleon, 1);
            ShipsHandler.spawnShip(Ships.Schooner, 2);
            ShipsHandler.spawnShip(Ships.Schooner, 4);
            ShipsHandler.spawnShip(Ships.Galleon, 5);
            ShipsHandler.spawnShip(Ships.Schooner, 6);
            Log.log("Spawned ships");

            GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-75f, 4f, -95.6f);
            GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-35f, 4f, -95.6f);
            GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 4f, 0f);
            GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(10f, 4f, -95.6f);
            GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Log.log("Moved ships");

            GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-75f, 4f, 20f);
            GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-35f, 4f, 20f);
            GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(10f, 4f, 20f);
            GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Log.log("Rotated ships");

            yield return new WaitUntil(() => GameMode.Instance.votingOver);
            started = true;
            Loaded = true;
            Log.log("Waiting complete!");
            yield break;
        }
    }
}
