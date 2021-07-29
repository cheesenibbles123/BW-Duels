using System;
using System.Collections;
using UnityEngine;
using BWModLoader;
using Harmony;
using System.Reflection;
using System.IO;

namespace BoardingBloodbath
{
    [Mod]
    public class BoardingBloodbathGameMode : MonoBehaviour
    {
        public static BoardingBloodbathGameMode Instance;
        public WakeNetObject wno;
        public static int GamemodeID;
        public bool Loaded = false;
        public bool started = false;
        int navyTickets = 200;
        public int navySteps = 20;
        int pirateTickets = 200;
        public int pirateSteps = 20;
        string configFile = "BoardingBloodbath.cfg";
        float respawnTimer = 15f;

        private void Awake()
        {
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
            try
            {
                HarmonyInstance harmonyInstance = HarmonyInstance.Create("com.github.archie");
                Log.log("Made harmony Instance");
                harmonyInstance.PatchAll();
                Log.log("Patched all");
            }
            catch (Exception e)
            {
                Log.log(e.Message);
                Log.log("##### Stack Trace #####");
                Log.log(e.StackTrace);
                Log.log("##### Source #####");
                Log.log(e.Source);
            }
            GameMode.Instance.resetEvent += this.reset;
            GameMode.Instance.resetEvent += ShipsHandler.Reset;
            Log.log("Setup reset event");
            wno = UI.Instance.chatbox.GetComponent<WakeNetObject>();
            Log.log("Make new wakenet object");
            loadSettings();
            Log.log("Loaded settings");
        }

        public void reset()
        {
            started = false;
            if (BoardingBloodbathModeHandler.voteSucceded)
            {
                BoardingBloodbathModeHandler.usersVoted.Clear();
                Loaded = true;
                BoardingBloodbathModeHandler.voteSucceded = false;
                Log.log("Starting coroutine");
                Instance.StartCoroutine(Instance.gameModeStart());
            }
            else
            {
                Loaded = false;
            }
        }

        void generateSettings()
        {
            StreamWriter streamWriter = new StreamWriter(configFile);
            streamWriter.WriteLine("navyTickets=" + navyTickets);
            streamWriter.WriteLine("pirateTickets=" + pirateTickets);
            streamWriter.WriteLine("respawnTimer=" + respawnTimer);
            streamWriter.Close();
        }

        public void loadSettings()
        {
            if (!File.Exists(configFile))
            {
                generateSettings();
            }
            string[] allLines = File.ReadAllLines(configFile);
            char splitCharacter = '=';
            for (int i=0; i<allLines.Length; i++)
            {
                if (allLines[i].Contains("="))
                {
                    string[] line = allLines[i].Split(splitCharacter);
                    if (line.Length == 2)
                    {
                        switch (line[0])
                        {
                            case "navyTickets":
                                int.TryParse(line[1], out navyTickets);
                                break;
                            case "pirateTickets":
                                int.TryParse(line[1], out pirateTickets);
                                break;
                            case "respawnTimer":
                                float.TryParse(line[1], out respawnTimer);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            navySteps = (int)(100 % ((10 / navyTickets) * 100));
            pirateSteps = (int)(100 % ((10 / pirateTickets) * 100));
        }

        void setTickets(int pirate, int navy)
        {
            GameMode.Instance.pirateTickets = pirate;
            GameMode.Instance.navyTickets = navy;
            updateTickets();
        }

        public static void updateTickets()
        {
            for (int i = 0; i < GameMode.Instance.teamCount; i++)
            {
                UI.Instance.matchHUD.GetComponent<WakeNetObject>().îëæêéïåðæìå("setTickets", óëððîêðëóêó.îéäåéçèïïñí, new object[]{
                    i,
                    GameMode.Instance.getTickets(i)
                }); ;
            }
        }

        private IEnumerator gameModeStart()
        {
            Log.log("Started [BoardingBloodbath] mode");
            GameMode.Instance.useTickets = true;
            wno.îëæêéïåðæìå("broadcastChat", óëððîêðëóêó.îéäåéçèïïñí, new object[]
            {
                1,
                1,
                "game",
                "Loading BoardingBloodbath mode..."
            });
            setTickets(pirateTickets, navyTickets);
            Log.log("Waiting 20sec");

            BoardingBloodbathModeHandler.usersVoted.Clear();
            BoardingBloodbathModeHandler.voteSucceded = false;

            yield return new WaitForSeconds(20f);
            Log.log("Passed 20sec");

            try
            {
                ShipsHandler.spawnShip(ShipsHandler.Ships.Hoy, 0);
                ShipsHandler.spawnShip(ShipsHandler.Ships.Galleon, 1);
                ShipsHandler.spawnShip(ShipsHandler.Ships.Hoy, 2);
                ShipsHandler.spawnShip(ShipsHandler.Ships.Hoy, 4);
                ShipsHandler.spawnShip(ShipsHandler.Ships.Galleon, 5);
                ShipsHandler.spawnShip(ShipsHandler.Ships.Hoy, 6);
            }
            catch(Exception e)
            {
                Log.log(e.Message);
            }
            Log.log("Spawned ships");

            try
            {
                GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-80f, 4.5f, -60f); // Distance from each other, height, distance from center
                GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 0f, 0f);

                GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-40f, 9f, -60f);
                GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 0f, 0f);

                GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(0f, 4.5f, -60f);
                GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 0, 0f);

                GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(40f, 4.5f, -60f);
                GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 180f, 0f);

                GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(80f, 9f, -60f);
                GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 180f, 0f);

                GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(120f, 4.5f, -60f);
                GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            catch (Exception e)
            {
                Log.log(e.Message);
            }
            Log.log("Moved ships to init");

            try
            {
                GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-37f, 4.5f, -10f); // Distance from each other, height, distance from center
                GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, -10f, 0f);
                GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-16f, 4.5f, 13f);
                GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, -10f, 0f);
            }
            catch (Exception e)
            {
                Log.log(e.Message);
            }
            Log.log("Moved pirate ships");

            try
            {
                GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-46.5f, 4.5f, 23f);
                GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 165f, 0f);
                GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-25f, 4.5f, 44f);
                GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 160f, 0f);
            }
            catch (Exception e)
            {
                Log.log(e.Message);
            }
            Log.log("Moved navy ships");

            try {
                GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-26f, 9f, 4f);
                GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-36f, 9f, 35f);
            }
            catch (Exception e)
            {
                Log.log(e.Message);
            }
            Log.log("Setup big ships");

            yield return new WaitUntil(() => GameMode.Instance.votingOver);
            started = true;
            Loaded = true;
            Log.log("Waiting complete!");
            yield break;
        }

        public IEnumerator bothandlerUpdateLoop(BotHandler __instance)
        {
            yield return new WaitForSeconds(0.1f);
            for (;;)
            {
                for (int i = 0; i < __instance.êóæìíîìñäîí.Length; i++)
                {
                    __instance.êóæìíîìñäîí[i].GetComponent<BotPlayer>().åñîïòíæêêåî.îëæêéïåðæìå("Unload", óëððîêðëóêó.îéäåéçèïïñí, new object[0]);
                }
                yield return new WaitForSeconds(5f);
            }
        }
    }
}
