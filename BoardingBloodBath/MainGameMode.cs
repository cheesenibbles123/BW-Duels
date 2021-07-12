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
    public class MainGameMode : MonoBehaviour
    {
        public static MainGameMode Instance;
        public WakeNetObject wno;
        public static int GamemodeID;
        public bool Loaded = false;
        public bool started = false;
        int navyTickets = 160;
        int pirateTickets = 160;
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
        }

        void setTickets(int pirate, int navy)
        {
            GameMode.Instance.pirateTickets = pirate;
            GameMode.Instance.navyTickets = navy;
            for (int i = 0; i < GameMode.Instance.teamCount; i++)
            {
                wno.îëæêéïåðæìå("setTickets", óëððîêðëóêó.îéäåéçèïïñí, new object[]{
                    i,
                    GameMode.Instance.getTickets(i)
                }); ;
            }
        }

        void setSpawnTimers()
        {
            GameMode.setSpawnTimes(new float[]
            {
                respawnTimer,
                respawnTimer,
                respawnTimer,
                respawnTimer,
                respawnTimer,
                respawnTimer
            });
        }

        private IEnumerator gameModeStart()
        {
            Log.log("Started [BoardingBloodbath] mode");
            wno.îëæêéïåðæìå("broadcastChat", óëððîêðëóêó.îéäåéçèïïñí, new object[]
            {
                1,
                1,
                "game",
                "Loading BoardingBloodbath mode..."
            });
            setTickets(pirateTickets, navyTickets);
            //setSpawnTimers();
            Log.log("Waiting 20sec");
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
                GameMode.Instance.teamParents[0].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-49f, 8f, -12f); // Distance from each other, height, distance from center
                GameMode.Instance.teamParents[1].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-36f, 0f, -22f);
                GameMode.Instance.teamParents[2].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-18f, 8f, -12f);
            }
            catch (Exception e)
            {
                Log.log(e.Message);
            }
            Log.log("Moved pirate ships");

            try
            {
                GameMode.Instance.teamParents[4].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-48f, 8f, 12f);
                GameMode.Instance.teamParents[5].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-35f, 0f, 22f);
                GameMode.Instance.teamParents[6].GetComponent<ShipHealth>().ññçäîèäíñðó.transform.position = new Vector3(-17f, 8f, 12f);
            }
            catch (Exception e)
            {
                Log.log(e.Message);
            }
            Log.log("Moved navy ships");
            
            yield return new WaitUntil(() => GameMode.Instance.votingOver);
            started = true;
            Loaded = true;
            Log.log("Waiting complete!");
            yield break;
        }
    }
}
