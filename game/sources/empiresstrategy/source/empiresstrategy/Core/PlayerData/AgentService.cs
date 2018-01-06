using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.PlayerData;
using Core.Main;
using Map.Data;
using Map.Data.Interfaces;
using Core;
using UnityEngine;
using Core.Interfaces;
using Graphs.PathFinder;
using StateMachine;

namespace Core.PlayerData
{
    public class AgentService : IAgentService
    {

        #region Services

        private static PlayerDataService _PlayerDataService;
        public static PlayerDataService PlayerDataService
        {
            get
            {
                if (_PlayerDataService == null) _PlayerDataService = ServiceLocator.GetService<PlayerDataService>();
                return _PlayerDataService;
            }
            set
            {
                _PlayerDataService = value;
            }
        }
        private static MatchDataService _matchDataService;
        public static MatchDataService MatchDataS
        {
            get
            {
                if (_matchDataService == null) _matchDataService = ServiceLocator.GetService<MatchDataService>();
                return _matchDataService;
            }
            set
            {
                _matchDataService = value;
            }
        }

        private static PathFinderService _PathFinderService;
        public static PathFinderService PathFinderService
        {
            get
            {
                if (_PathFinderService == null) _PathFinderService = ServiceLocator.GetService<PathFinderService>();
                return _PathFinderService;
            }
            set
            {
                _PathFinderService = value;
            }
        }

        private static IPrefabService _prefabService;
        public static IPrefabService prefabService
        {
            get
            {
                if (_prefabService == null) _prefabService = ServiceLocator.GetService<IPrefabService>();
                return _prefabService;
            }
            set
            {
                _prefabService = value;
            }
        }

        #endregion

        #region Properties
        private bool force = false;
        private Vector2? dest = null;
        public bool freeze = true;
        public IList<GameObject> visitedPoints = new List<GameObject>();
        static float timer = 0;

        #endregion




        #region Selecting
        public void agentUpd(GameObject obj, object agentObj)
        {
            Agent agent = (Agent)agentObj;
            if (agent != null)
            {
                agent.UpdVelocity();
            }
        }


        public void agentClicked(GameObject obj, object agentObj)
        {
            Agent agent = (Agent)agentObj;
            agent.objectUnity = obj;
            if ((int)agent.owner == _matchDataService.myMatch.mapData.currentPlayer)
            {
                IList<Agent> selectedAgents = _matchDataService.myMatch.mapData.GetCurrentPlayer().selectedAgents;
                if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
                {
                    if (selectedAgents.Contains(agent))
                    {
                        unselectAgent(agent, selectedAgents);
                        selectedAgents.Remove(agent);
                    }
                    else
                    {
                        selectAgent(agent, selectedAgents);
                        selectedAgents.Add(agent);
                    }
                }
                else
                {
                    if (selectedAgents.Contains(agent))
                    {
                        unselectAll(selectedAgents);
                    }
                    else
                    {
                        unselectAll(selectedAgents);
                        selectAgent(agent, selectedAgents);
                        selectedAgents.Add(agent);
                    }
                }
            }
            else
            {

            }
        }


        public void unselectAll()
        {
            IList<Agent> selectedAgents = _matchDataService.myMatch.mapData.GetCurrentPlayer().selectedAgents;
            foreach (Agent a in selectedAgents)
            {
                unselectAgent(a, selectedAgents);
            }
            selectedAgents.Clear();
        }

        public void unselectAll(IList<Agent> selectedAgents)
        {
            foreach (Agent a in selectedAgents)
            {
                unselectAgent(a, selectedAgents);
            }
            selectedAgents.Clear();
        }

        public void unselectAgent(Agent agent, IList<Agent> selectedAgents)
        {
            if (agent.destroyed) return;
            //Debug.Log("odznaczam");
            GameObject obj = agent.objectUnity;
            //Debug.Log("agentClicked id" + agent.id + "x " + agent.x + "y " + agent.y + " owner " + agent.owner);
            MeshRenderer mesh = obj.transform.Find("click").GetComponentInChildren(typeof(MeshRenderer)) as MeshRenderer;
            mesh.enabled = false;
        }
        public void selectAgent(Agent agent, IList<Agent> selectedAgents)
        {
            if (agent.destroyed) return;
            //Debug.Log("znaczam");
            GameObject obj = agent.objectUnity;
            //Debug.Log("agentClicked id" + agent.id + "x " + agent.x + "y " + agent.y + " owner " + agent.owner);
            MeshRenderer mesh = obj.transform.Find("click").GetComponentInChildren(typeof(MeshRenderer)) as MeshRenderer;
            mesh.enabled = true;
        }

        #endregion

        #region run

        public void setDestination(Vector2 destination, IMyGameObject a)
        {
            setDestination(destination, (Agent)a);
        }

        public void setDestination(Vector2 destination, Agent a)
        {
            if (a.destroyed) return;
            //Debug.Log("destination " + destination);
            IList<Vector2> l = PathFinderService.FindWay(new Vector2(a.x, a.y), destination, PlayerDataService.mySettings.pathFinderMethod, _matchDataService.myMatch.mapData, DataType.Matrix);
            if (l.Count > 0)
            {
                a.destinationWay = l;
                a.destination = destination;
                a.startedWay();
                clearPoints();
                if (PlayerDataService.mySettings.showPoints) drawPoints();
                if (a.currentGoal != Agent.Goal.Fight)
                {
                    a.currentGoal = Agent.Goal.None;
                    if (MatchDataS.myMatch.mapData.Table[(int)destination.x + 800][(int)destination.y + 800] != null)
                    {
                        IMyGameObject o = MatchDataS.myMatch.mapData.Table[(int)destination.x + 800][(int)destination.y + 800];
                        if (o.objectType == ObjectType.Food)
                        {
                            a.gold = 0;
                            a.wood = 0;
                            a.currentGoal = Agent.Goal.CollectFood;
                        }
                        if (o.objectType == ObjectType.Gold)
                        {
                            a.wood = 0;
                            a.food = 0;
                            a.currentGoal = Agent.Goal.CollectGold;
                        }
                        if (o.objectType == ObjectType.Wood)
                        {
                            a.gold = 0;
                            a.food = 0;
                            a.currentGoal = Agent.Goal.CollectWood;
                        }
                        else if (o.objectType == ObjectType.Building)
                        {
                            Building ob = (Building)o;
                            if ((int)ob.owner == _matchDataService.myMatch.mapData.currentPlayer)
                            {
                                a.currentGoal = Agent.Goal.Store;
                            }
                            else
                            {
                                a.currentGoal = Agent.Goal.Fight;
                            }
                        }
                    }
                }
            }
        }

        private void clearPoints()
        {
            foreach (GameObject el in visitedPoints)
            {
                GameObject.Destroy(el);
            }
            visitedPoints.Clear();
        }

        private void drawPoints()
        {
            var visited = PathFinderService.visited;
            if (visited != null)
            {
                foreach (var vec in visited)
                {
                    Transform prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.PointS);
                    prefab = UnityEngine.Object.Instantiate(prefab, new Vector3((int)vec.x, 0, (int)vec.y), Quaternion.identity) as Transform;
                    visitedPoints.Add(prefab.gameObject);
                }
            }
        }

        public void setDestination(Vector2 destination)
        {
            IList<Agent> selectedAgents = _matchDataService.myMatch.mapData.GetCurrentPlayer().selectedAgents;
            foreach (Agent a in selectedAgents)
            {
                setDestination(destination, a);
            }
        }

        #endregion

        #region init
        public void CreateNewVilager(Player player)
        {

            if (player.food < 50) return;
            player.food -= 50;
            Agent newAgent = new Agent(player.startXB, player.startXB, player);
            newAgent.type = Agent.Type.Vilager;
            player.agents.Add(newAgent.id, newAgent);
            newAgent.CreateMe();
            newAgent.owner = player.number;
        }


        public void Init()
        {
            Transform prefab = null;
            foreach (var p in MatchDataS.myMatch.mapData.players)
            {
                Player player = p.Value;
                Debug.Log("InitAgents Player " + player.id);
                foreach (var a in player.agents)
                {
                    Agent agent = a.Value;
                    agent.owner = player.number;
                    Debug.Log("InitAgents Player " + player.id + " Agent " + agent.id + " type " + agent.type);
                    switch (agent.type)
                    {
                        case Agent.Type.Vilager:
                            prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.Vilager);
                            if (player.controledBySI == false) prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.VilagerRed);
                            break;
                        case Agent.Type.Cow:
                            continue;
                    }
                    if (prefab != null)
                    {
                        agent.transform = UnityEngine.Object.Instantiate(prefab, new Vector3(agent.x, 0, agent.y), Quaternion.identity) as Transform;
                        agent.objectUnity = agent.transform.gameObject;
                        agent.objectUnity.name = "Agent-" + agent.id;
                        Debug.Log("Insert vilager " + agent.x + " " + agent.y);
                        MatchDataS.myMatch.mapData.Table[agent.x + 800][agent.y + 800] = agent;
                        IcontrollAgentScript controllAgentScript = agent.objectUnity.GetComponent(typeof(IcontrollAgentScript)) as IcontrollAgentScript;
                        controllAgentScript.agentData = agent;
                    }
                }
            }
        }
        #endregion



        public void update()
        {
            if (freeze) return;
            if (timer >= Time.time) return;
            timer = Time.time + 0.5f;
            try
            {
                foreach (var p in MatchDataS.myMatch.mapData.players)
                {
                    Player player = p.Value;

                    updateSI(player);

                    foreach (var a in player.agents)
                    {
                        Agent agent = a.Value;
                        agent.update(MatchDataS.myMatch.mapData);
                        if (force)
                        {
                            if (agent.currentGoal != player.currentGoal)
                            {
                                agent.currentGoal = player.currentGoal;
                                if (agent.currentGoal == Agent.Goal.Fight)
                                {
                                    setDestination((Vector2)dest, agent);
                                }
                            }
                        }
                        else
                        {
                            if (agent.currentGoal == Agent.Goal.None)
                            {
                                agent.currentGoal = player.currentGoal;
                                if (agent.currentGoal == Agent.Goal.Fight)
                                {
                                    setDestination((Vector2)dest, agent);
                                }
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException) { }
        }

        private poziom countAgentLevel(Player player, Player op)
        {
            int dif = 10;
            if (!PlayerDataService.mySettings.isFighterSI) dif = 20;
            poziom aLevel = poziom.Malo;
            if (player.agents.Count < dif) aLevel = poziom.Malo;
            else if (player.agents.Count < op.agents.Count) aLevel = poziom.Malo;
            else if (player.agents.Count < op.agents.Count + dif) aLevel = poziom.Srednio;
            else aLevel = poziom.Duzo;

            return aLevel;
        }


        public void updateSI(Player player)
        {
            if (player.controledBySI == false)
            {
                //Debug.Log(player.FoodLevelFuzzy.ToString());
            }
            if (player.controledBySI == true)
            {
                force = false;
                // pobieranie liczby agentów
                Player op = null;
                foreach (var pp in MatchDataS.myMatch.mapData.players)
                {
                    Player cur = pp.Value;
                    if (cur.id != player.id) op = cur;
                }
                poziom aLevel = countAgentLevel(player, op);
                dest = new Vector2(op.baseBuilding.x, op.baseBuilding.y);

                #region fighter
                if (PlayerDataService.mySettings.isFighterSI)
                {
                    //ROZWOJ
                    if (player._foodLevel > poziom.Malo)
                    {
                        CreateNewVilager(player);
                    }

                    //Debug.Log(player.FoodLevel + " " + player.WoodLevel + " " + player.GoldLevel);
                    // WALKA
                    if (aLevel == poziom.Duzo)
                    {
                        player.currentGoal = Agent.Goal.Fight;
                        force = true;
                        return;
                    }

                    // EKONOMIA
                    if (player.FoodLevel == poziom.Malo)
                    {
                        player.currentGoal = Agent.Goal.SearchFood;
                    }
                    else if (player.FoodLevel == poziom.Srednio)
                    {
                        player.currentGoal = Agent.Goal.SearchFood;
                    }
                }
                #endregion

                #region notfighter
                if (!PlayerDataService.mySettings.isFighterSI)
                {
                    //ROZWOJ
                    if (player._foodLevel > poziom.Malo && aLevel < poziom.Srednio)
                    {
                        CreateNewVilager(player);
                    }
                    else if (player._foodLevel > poziom.Srednio && aLevel < poziom.Duzo)
                    {
                        CreateNewVilager(player);
                    }
                    else if (player.GoldLevel > poziom.Srednio || (player.GoldLevel > poziom.Malo && player.collectSpeed < 10))
                    {
                        player.buySpeed();
                    }
                    else if ((player.GoldLevel > poziom.Srednio && player.WoodLevel > poziom.Srednio) ||
                        (player.WoodLevel > poziom.Malo && player.GoldLevel > poziom.Malo && player.collectMax < 50))
                    {
                        player.buySize();
                    }
                    else if (player._foodLevel > poziom.Malo)
                    {
                        CreateNewVilager(player);
                    }

                    // WALKA
                    if (aLevel == poziom.Duzo)
                    {
                        player.currentGoal = Agent.Goal.Fight;
                        return;
                    }

                    // EKONOMIA
                    if (player.FoodLevel == poziom.Malo)
                    {
                        //if (player.currentGoal != Agent.Goal.SearchFood) Debug.Log("Nowy cel " + Agent.Goal.SearchFood);
                        player.currentGoal = Agent.Goal.SearchFood;
                    }
                    else if (player.GoldLevel == poziom.Malo)
                    {
                        //if (player.currentGoal != Agent.Goal.SearchFood) Debug.Log("Nowy cel " + Agent.Goal.SearchFood);
                        player.currentGoal = Agent.Goal.SearchGold;
                    }
                    else if (player.WoodLevel == poziom.Malo)
                    {
                        //if (player.currentGoal != Agent.Goal.SearchFood) Debug.Log("Nowy cel " + Agent.Goal.SearchFood);
                        player.currentGoal = Agent.Goal.SearchWood;
                    }
                    else if (player.FoodLevel == poziom.Srednio)
                    {
                        //if (player.currentGoal != Agent.Goal.SearchFood) Debug.Log("Nowy cel " + Agent.Goal.SearchFood);
                        player.currentGoal = Agent.Goal.SearchFood;
                    }
                }
                #endregion

            }
        }
    }
}
