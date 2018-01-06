using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interfaces;
using UnityEngine;

namespace Map.Data.Interfaces
{
    public class Agent : IMyGameObject
    {
        public enum Goal { None, Fight, CollectFood, CollectWood, CollectGold, Store, SearchFood, SearchWood, SearchGold, Scout, Destroy };

        public Goal currentGoal = Goal.None;
        public int collectSpeed
        {
            get
            {
                return ownerPlayer.collectSpeed;
            }
        }
        public int collectMax
        {
            get
            {
                return ownerPlayer.collectMax;
            }
        }
        public int food = 0;
        public int wood = 0;
        public int gold = 0;

        private int XDirection = UnityEngine.Random.Range(-1, 1);
        private int YDirection = UnityEngine.Random.Range(-1, 1);

        #region Services
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
        private static IAgentService _agentService;
        public static IAgentService agentService
        {
            get
            {
                if (_agentService == null) _agentService = ServiceLocator.GetService<IAgentService>();
                return _agentService;
            }
            set
            {
                _agentService = value;
            }
        }

        #endregion


        static public int maxId = 1;
        public UnityEngine.GameObject objectUnity;
        public UnityEngine.Transform transform;
        public enum Type { None, Vilager, Cow }
        public int id;
        public Map.Data.Interfaces.Player.PlayerNumberEnum owner;
        public Type type;
        public int x;
        public int y;
        public Vector3 forw;

        public Map.Data.Interfaces.Player ownerPlayer;
        public bool freeze = false;

        public Vector2 destination;
        public Vector2? destinationRemember;

        public IList<Vector2> destinationWay;
        public IList<GameObject> destinationWayPoints = new List<GameObject>();

        public Agent(int x, int y, Player p)
        {
            this.x = x;
            this.y = y;
            this.id = maxId;
            maxId++;
            ownerPlayer = p;
            if (XDirection == 0) XDirection = 1;
            if (YDirection == 0) YDirection = -1;
        }

        public ObjectType objectType
        {
            get
            {
                return ObjectType.Agent;
            }
        }

        public void finishedWay()
        {
            if (currentGoal == Goal.Scout) currentGoal = Goal.None;
            foreach (var p in destinationWayPoints)
            {
                GameObject.Destroy(p);
            }
            destinationWayPoints.Clear();
            forw = new Vector3(0, 0, 0);
        }

        public void SetStore()
        {
            this.currentGoal = Goal.Store;
            agentService.setDestination(new Vector2(ownerPlayer.startXB, ownerPlayer.startYB), this);
        }

        public void AgentSI(MapData map)
        {
            if (this.currentGoal == Goal.None)
            {
                this.currentGoal = ownerPlayer.currentGoal;
            }
            if (this.currentGoal == Goal.SearchFood)
            {
                Vector2? where = Search(ObjectType.Food, map, new Vector2(x + 800, y + 800));
                if (where != null)
                {
                    Vector2 where2 = new Vector2(where.Value.x - 800, where.Value.y - 800);
                    agentService.setDestination(where2, this);
                }
                else
                {
                    int newX = x + UnityEngine.Random.Range(1, 10) * XDirection;
                    int newY = y + UnityEngine.Random.Range(1, 10) * YDirection;
                    agentService.setDestination(new Vector2(newX, newY), this);
                    currentGoal = Goal.Scout;
                }
            }
            if (this.currentGoal == Goal.SearchWood)
            {
                Vector2? where = Search(ObjectType.Wood, map, new Vector2(x + 800, y + 800));
                if (where != null)
                {
                    Vector2 where2 = new Vector2(where.Value.x - 800, where.Value.y - 800);
                    agentService.setDestination(where2, this);
                }
                else
                {
                    int newX = x + UnityEngine.Random.Range(1, 10) * XDirection;
                    int newY = y + UnityEngine.Random.Range(1, 10) * YDirection;
                    agentService.setDestination(new Vector2(newX, newY), this);
                    currentGoal = Goal.Scout;
                }
            }
            if (this.currentGoal == Goal.SearchGold)
            {
                Vector2? where = Search(ObjectType.Wood, map, new Vector2(x + 800, y + 800));
                if (where != null)
                {
                    Vector2 where2 = new Vector2(where.Value.x - 800, where.Value.y - 800);
                    agentService.setDestination(where2, this);
                }
                else
                {
                    int newX = x + UnityEngine.Random.Range(1, 10) * XDirection;
                    int newY = y + UnityEngine.Random.Range(1, 10) * YDirection;
                    agentService.setDestination(new Vector2(newX, newY), this);
                    currentGoal = Goal.Scout;
                }
            }
        }


        float timer = 0;
        public bool destroyed = false;
        public void update(MapData map)
        {
            Player p;

            if (timer < Time.time && !destroyed)
            {
                AgentSI(map);

                foreach (var pl in map.players)
                {
                    p = pl.Value;
                    if (p.controledBySI == true && this.ownerPlayer.controledBySI == false)
                    {
                        var obcy_ = p.agents.Where(f => (
                            (f.Value.x == x && f.Value.y == y)
                            || (f.Value.x == x + 1 && f.Value.y == y)
                            || (f.Value.x == x - 1 && f.Value.y == y)
                            || (f.Value.x == x && f.Value.y == y + 1)
                            || (f.Value.x == x && f.Value.y == y - 1)
                            || (f.Value.x == x + 1 && f.Value.y == y + 1)
                            || (f.Value.x == x - 1 && f.Value.y == y - 1)
                            || (f.Value.x == x - 1 && f.Value.y == y + 1)
                            || (f.Value.x == x + 1 && f.Value.y == y - 1)
                            ) && f.Value.destroyed == false).ToList();

                        if (obcy_.Count > 0)
                        {
                            Agent obcy = obcy_.FirstOrDefault().Value;

                            finishedWay();
                            obcy.finishedWay();
                            obcy.DestroyMe();
                            DestroyMe();
                            if (ownerPlayer.agents.Count == 0)
                            {
                                obcy.ownerPlayer.won = true;
                            }
                            else if (obcy.ownerPlayer.agents.Count == 0) {
                                ownerPlayer.won = true;
                            }
                        }
                    }
                    p = null;
                }
                timer = Time.time + 0.5f;
                if (destinationWay == null || destinationWay.Count == 0)
                {
                    finishedWay();
                    var mygo = map.Table[x + 800][y + 800];
                    if (mygo != null)
                    {
                        if (mygo.objectType == ObjectType.Building)
                        {
                            Building b = (Building)mygo;
                            if (b.owner == owner)
                            {
                                map.players.TryGetValue((int)this.owner, out p);
                                if (p != null)
                                {
                                    p.food += food;
                                    food = 0;
                                    p.gold += gold;
                                    gold = 0;
                                    p.wood += wood;
                                    wood = 0;

                                    currentGoal = Goal.None;

                                    if (destinationRemember.HasValue)
                                    {
                                        agentService.setDestination(destinationRemember.Value, this);
                                        if (map.Table[(int)destinationRemember.Value.x + 800][(int)destinationRemember.Value.y + 800] != null)
                                        {
                                            ObjectType t = map.Table[(int)destinationRemember.Value.x + 800][(int)destinationRemember.Value.y + 800].objectType;
                                            if (t == ObjectType.Food) currentGoal = Goal.CollectFood;
                                            if (t == ObjectType.Gold) currentGoal = Goal.CollectGold;
                                            if (t == ObjectType.Wood) currentGoal = Goal.CollectWood;
                                        }
                                    }
                                    destinationRemember = null;
                                }
                            }
                            else
                            {
                                b.zniszczony++;
                                finishedWay();
                                DestroyMe();
                                if (b.zniszczony >= 10)
                                {
                                    ownerPlayer.won = true;
                                }
                            }
                        }
                        else if (currentGoal == Goal.CollectFood)
                        {

                            if (mygo.objectType == ObjectType.Food)
                            {

                                Food f = (Food)mygo;
                                map.players.TryGetValue((int)this.owner, out p);
                                if (p != null && f != null)
                                {
                                    if (food + collectSpeed <= collectMax)
                                    {
                                        food += collectSpeed;
                                        f.value -= collectSpeed;
                                        if (f.value < 0) food += f.value;
                                        if (f.value < 1)
                                        {
                                            GameObject.Destroy(f.objectUnity);
                                            map.Table[x + 800][y + 800] = null;
                                            SetStore();
                                        }
                                    }
                                    else
                                    {
                                        destinationRemember = new Vector2(x, y);
                                        SetStore();
                                    }

                                }
                            }
                            else
                            {
                                currentGoal = Goal.None;
                            }
                        }
                        else if (currentGoal == Goal.CollectGold)
                        {

                            if (mygo.objectType == ObjectType.Gold)
                            {

                                Gold f = (Gold)mygo;
                                map.players.TryGetValue((int)this.owner, out p);
                                if (p != null && f != null)
                                {
                                    if (gold + collectSpeed <= collectMax)
                                    {
                                        gold += collectSpeed;
                                        f.value -= collectSpeed;
                                        if (f.value < 0) gold += f.value;
                                        if (f.value < 1)
                                        {
                                            GameObject.Destroy(f.objectUnity);
                                            map.Table[x + 800][y + 800] = null;
                                            SetStore();
                                        }
                                    }
                                    else
                                    {
                                        destinationRemember = new Vector2(x, y);
                                        SetStore();
                                    }
                                }
                            }
                            else
                            {
                                currentGoal = Goal.None;
                            }
                        }
                        else if (currentGoal == Goal.CollectWood)
                        {

                            if (mygo.objectType == ObjectType.Wood)
                            {

                                Wood f = (Wood)mygo;
                                map.players.TryGetValue((int)this.owner, out p);
                                if (p != null && f != null)
                                {
                                    if (gold + collectSpeed <= collectMax)
                                    {
                                        wood += collectSpeed;
                                        f.value -= collectSpeed;
                                        if (f.value < 0) wood += f.value;
                                        if (f.value < 1)
                                        {
                                            GameObject.Destroy(f.objectUnity);
                                            map.Table[x + 800][y + 800] = null;
                                            SetStore();
                                        }
                                    }
                                    else
                                    {
                                        destinationRemember = new Vector2(x, y);
                                        SetStore();
                                    }
                                }
                            }
                            else
                            {
                                currentGoal = Goal.None;
                            }
                        }
                    }
                    else
                    {
                        currentGoal = Goal.None;
                    }
                }
                if (destinationWay != null)
                {
                    Vector2 first = destinationWay.FirstOrDefault();
                    if (first != null)
                    {
                        SetVelocity();
                    }
                    else
                    {
                        finishedWay();
                    }
                }
            }
        }

        public void startedWay()
        {
            if (destinationWay.Count < 1) return;
            finishedWay();
            destinationWay.RemoveAt(0);
            Transform prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.Point);
            if (destinationWay != null)
            {
                foreach (var vec in destinationWay)
                {
                    if (!ownerPlayer.controledBySI)
                    {
                        prefab = UnityEngine.Object.Instantiate(prefab, new Vector3((int)vec.x, 0, (int)vec.y), Quaternion.identity) as Transform;
                        destinationWayPoints.Add(prefab.gameObject);
                    }
                    //Debug.Log("Point  " + vec.x + " " + vec.y);
                }
            } 
            Vector2 second = destinationWay.First();
            if (second != null)
            {
                forw = new Vector3(second.x - x, 0, second.y - y);
            }
        }

        public void SetVelocity()
        {
            Vector2 first = destinationWay.FirstOrDefault();
            if (first != null && destinationWay.Count > 0 && !destroyed)
            {
                objectUnity.transform.position = new Vector3(first.x, 0, first.y);
                x = (int)first.x;
                y = (int)first.y;

                GameObject o = destinationWayPoints.FirstOrDefault();
                if (o != null)
                {
                    GameObject.Destroy(o);
                    destinationWayPoints.RemoveAt(0);
                }
                destinationWay.RemoveAt(0);
                Vector2 second = destinationWay.First();
                forw = new Vector3();
                objectUnity.transform.Translate(forw);
                if (second != null)
                {
                    forw = new Vector3(second.x - first.x, 0, second.y - first.y);
                }
                else
                {
                    forw = new Vector3(0, 0, 0);
                }
            }
        }

        public void UpdVelocity()
        {
            if (destinationWay != null)
            {
                if (destinationWay.Count == 0)
                {
                    objectUnity.transform.Translate(new Vector3());
                }
                else
                {
                    objectUnity.transform.Translate(forw * Time.deltaTime * 1.5f);
                }
            }
            else
            {
                objectUnity.transform.Translate(new Vector3());
            }
        }

        public void DestroyMe()
        {
            GameObject.Destroy(objectUnity);
            ownerPlayer.agents.Remove(id);
            destroyed = true;
        }


        public void CreateMe()
        {
            Transform prefab = null;
            prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.Vilager);
            if (ownerPlayer.controledBySI == false) prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.VilagerRed);

            transform = UnityEngine.Object.Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity) as Transform;
            objectUnity = transform.gameObject;
            objectUnity.name = "Agent-" + id;
            Debug.Log("Insert vilager " + x + " " + y);
            //MatchDataS.myMatch.mapData.Table[x + 800][y + 800] = this;
            IcontrollAgentScript controllAgentScript = objectUnity.GetComponent(typeof(IcontrollAgentScript)) as IcontrollAgentScript;
            controllAgentScript.agentData = this;
        }


        public Vector2? Search(ObjectType type, MapData map, Vector2 c)
        {
            for (int k = 0; k < 25; k++)
            {
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < k; j++)
                    {
                        if (map.Table[(int)c.x + i][(int)c.y + j] != null)
                        {
                            if (map.Table[(int)c.x + i][(int)c.y + j].objectType == type)
                            {
                                return new Vector2(c.x + i, c.y + j);
                            }
                        }
                        if (map.Table[(int)c.x - i][(int)c.y - j] != null)
                        {
                            if (map.Table[(int)c.x - i][(int)c.y - j].objectType == type)
                            {
                                return new Vector2(c.x - i, c.y - j);
                            }
                        }
                        if (map.Table[(int)c.x - i][(int)c.y + j] != null)
                        {
                            if (map.Table[(int)c.x - i][(int)c.y + j].objectType == type)
                            {
                                return new Vector2(c.x - i, c.y + j);
                            }
                        }
                        if (map.Table[(int)c.x + i][(int)c.y - j] != null)
                        {
                            if (map.Table[(int)c.x + i][(int)c.y - j].objectType == type)
                            {
                                return new Vector2(c.x + i, c.y - j);
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
