using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.Data.Interfaces;
using Core.Interfaces;
using UnityEngine;

namespace Map.Data
{
    public class MapData
    {
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

        #endregion

        public string mapName;
        public int size;
        public Dictionary<int, Player> players;

        public int currentPlayer = 1;

        public Player GetCurrentPlayer()
        {
            Player ret;
            if (players.TryGetValue(currentPlayer, out ret) == false)
            {
                throw new Exception("Nie znaleziono gracza w słowniku!");
            }
            return ret;
        }
        public void Clear()
        {
            int start = 0; //powinno byc 0
            int end = 1600; //powinno byc 1600
            for (int i = start; i < end; i++)
            {
                for (int j = start; j < end; j++)
                {
                    Table[i][j] = null;
                }
            }
        }


        public void ReadMap(string[] lines)
        {
            fillBuildings();
            int start = 600; //powinno byc 0
            int end = 1000; //powinno byc 1600
            for (int i = start; i < end; i++)
            {
                string curLine = lines[i - 600];
                string[] fields = curLine.Split(',');
                for (int j = start; j < end; j++)
                {
                    string curField = fields[j- 600];
                    if (curField.Equals("r")) {
                        Rock r = new Rock(i - 800, j - 800);
                        Table[i][j] = r;
                    }
                    if (curField.Equals("f"))
                    {
                        Food r = new Food(i - 800, j - 800);
                        Table[i][j] = r;
                    }
                    if (curField.Equals("g"))
                    {
                        Gold r = new Gold(i - 800, j - 800);
                        Table[i][j] = r;
                    }
                    if (curField.Equals("w"))
                    {
                        Wood r = new Wood(i - 800, j - 800);
                        Table[i][j] = r;
                    }
                }
            }
            FillObjects(start, end);
        }


        public void SaveMap()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"map.txt", true))
            {
                int start = 600; //powinno byc 0
                int end = 1000; //powinno byc 1600
                for (int i = start; i < end; i++)
                {
                    string line = "";
                    for (int j = start; j < end; j++)
                    {
                        string ObjToSave = "0";
                        if (Table[i][j] != null)
                        {
                            if (Table[i][j].objectType == ObjectType.None)
                            {
                                ObjToSave = "0";
                            }
                            if (Table[i][j].objectType == ObjectType.Wood)
                            {
                                ObjToSave = "w";
                            }
                            if (Table[i][j].objectType == ObjectType.Rock)
                            {
                                ObjToSave = "r";
                            }
                            if (Table[i][j].objectType == ObjectType.Gold)
                            {
                                ObjToSave = "g";
                            }
                            if (Table[i][j].objectType == ObjectType.Food)
                            {
                                ObjToSave = "f";
                            }
                            if (Table[i][j].objectType == ObjectType.Building)
                            {
                                ObjToSave = "b";
                            }
                        }
                        line += ObjToSave + ",";
                    }
                    file.WriteLine(line);
                }
            }
        }

        public void randomFill(int percentage)
        {
            fillBuildings();

            int start = 600; //powinno byc 0
            int end = 1000; //powinno byc 1600
            for (int i = start; i < end; i++)
            {
                for (int j = start; j < end; j++)
                {
                    if (Table[i][j] != null)
                    {
                        if (Table[i][j].objectType != ObjectType.None) continue;
                    }
                    if (UnityEngine.Random.Range(0, 100) < percentage)
                    {
                        Rock r = new Rock(i - 800, j - 800);
                        Table[i][j] = r;
                    }
                    if (Table[i][j] != null)
                    {
                        if (Table[i][j].objectType != ObjectType.None) continue;
                    }
                    if (UnityEngine.Random.Range(0, 100) < percentage / 10)
                    {
                        Food r = new Food(i - 800, j - 800);
                        Table[i][j] = r;
                    }
                    if (Table[i][j] != null)
                    {
                        if (Table[i][j].objectType != ObjectType.None) continue;
                    }
                    if (UnityEngine.Random.Range(0, 100) < percentage / 10 && UnityEngine.Random.Range(0, 10) < 4)
                    {
                        Gold r = new Gold(i - 800, j - 800);
                        Table[i][j] = r;
                    }
                    if (Table[i][j] != null)
                    {
                        if (Table[i][j].objectType != ObjectType.None) continue;
                    }
                    if (UnityEngine.Random.Range(0, 100) < percentage / 10 && UnityEngine.Random.Range(0, 10) < 1)
                    {
                        Wood r = new Wood(i - 800, j - 800);
                        Table[i][j] = r;
                    }
                }
            }
            FillObjects(start, end);
        }

        private void FillObjects (int start, int end) {

            for (int i = start; i < end; i++)
            {
                for (int j = start; j < end; j++)
                {
                    if (Table[i][j] == null) continue;
                    if (Table[i][j].objectType == ObjectType.Food)
                    {
                        Food r = (Food)Table[i][j];
                        if (r.transform != null) continue;
                        Transform prefab = null;
                        prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.Food);

                        r.transform = UnityEngine.Object.Instantiate(prefab, new Vector3(r.x, 0, r.y + 0.5f), Quaternion.identity) as Transform;
                        r.transform.localScale = new Vector3(r.transform.localScale.x, r.transform.localScale.y, r.transform.localScale.z);
                        // r.transform.Rotate(new Vector3(0, 0, 90));
                        r.objectUnity = r.transform.gameObject;
                        r.objectUnity.name = "Food-" + r.id;
                    }
                    if (Table[i][j].objectType == ObjectType.Gold)
                    {
                        Gold r = (Gold)Table[i][j];
                        if (r.transform != null) continue;
                        Transform prefab = null;
                        prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.Gold);

                        r.transform = UnityEngine.Object.Instantiate(prefab, new Vector3(r.x, 0, r.y + 0.5f), Quaternion.identity) as Transform;
                        r.transform.localScale = new Vector3(r.transform.localScale.x, r.transform.localScale.y, r.transform.localScale.z);
                        // r.transform.Rotate(new Vector3(0, 0, 90));
                        r.objectUnity = r.transform.gameObject;
                        r.objectUnity.name = "Gold-" + r.id;
                    }
                    if (Table[i][j].objectType == ObjectType.Wood)
                    {
                        Wood r = (Wood)Table[i][j];
                        if (r.transform != null) continue;
                        Transform prefab = null;
                        prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.Wood);

                        r.transform = UnityEngine.Object.Instantiate(prefab, new Vector3(r.x, 0, r.y + 0.5f), Quaternion.identity) as Transform;
                        r.transform.localScale = new Vector3(r.transform.localScale.x, r.transform.localScale.y, r.transform.localScale.z);
                        // r.transform.Rotate(new Vector3(0, 0, 90));
                        r.objectUnity = r.transform.gameObject;
                        r.objectUnity.name = "Wood-" + r.id;
                    }
                    if (Table[i][j].objectType == ObjectType.Rock)
                    {
                        Rock r = (Rock)Table[i][j];
                        if (r.transform != null) continue;
                        Transform prefab = null;
                        prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.RealRock);
                        bool drawed = false;

                        if (Table[i][j] != null && Table[i + 1][j] != null && Table[i][j + 1] != null && Table[i + 1][j + 1] != null && !drawed)
                        {
                            if (Table[i][j].objectType == ObjectType.Rock && Table[i + 1][j].objectType == ObjectType.Rock && Table[i][j + 1].objectType == ObjectType.Rock && Table[i + 1][j + 1].objectType == ObjectType.Rock)
                            {
                                Rock r2 = (Rock)Table[i + 1][j];
                                Rock r3 = (Rock)Table[i][j + 1];
                                Rock r4 = (Rock)Table[i + 1][j + 1];
                                if (r2.transform == null && r3.transform == null && r4.transform == null)
                                {
                                    r.transform = UnityEngine.Object.Instantiate(prefab, new Vector3(r.x + 0.5f, 0, r.y + 0.5f), Quaternion.identity) as Transform;
                                    r.transform.localScale = r.transform.localScale * 2;
                                    r.objectUnity = r.transform.gameObject;
                                    r.objectUnity.name = "Rock-" + r.id;


                                    r2.transform = r.transform;
                                    r2.objectUnity = r.objectUnity;

                                    r3.transform = r.transform;
                                    r3.objectUnity = r.objectUnity;

                                    r4.transform = r.transform;
                                    r4.objectUnity = r.objectUnity;

                                    drawed = true;
                                }
                            }
                        }
                        if (Table[i][j] != null && Table[i + 1][j] != null && !drawed)
                        {
                            if (Table[i][j].objectType == ObjectType.Rock && Table[i + 1][j].objectType == ObjectType.Rock)
                            {
                                Rock r2 = (Rock)Table[i + 1][j];
                                if (r2.transform == null)
                                {
                                    r.transform = UnityEngine.Object.Instantiate(prefab, new Vector3(r.x + 0.5f, 0, r.y), Quaternion.identity) as Transform;
                                    r.transform.localScale = new Vector3(r.transform.localScale.x * 2, r.transform.localScale.y, r.transform.localScale.z);
                                    r.objectUnity = r.transform.gameObject;
                                    r.objectUnity.name = "Rock-" + r.id;


                                    r2.transform = r.transform;
                                    r2.objectUnity = r.objectUnity;
                                    drawed = true;
                                }
                            }
                        }
                        if (Table[i][j] != null && Table[i][j + 1] != null && !drawed)
                        {
                            if (Table[i][j].objectType == ObjectType.Rock && Table[i][j + 1].objectType == ObjectType.Rock)
                            {
                                Rock r3 = (Rock)Table[i][j + 1];
                                if (r3.transform == null)
                                {
                                    r.transform = UnityEngine.Object.Instantiate(prefab, new Vector3(r.x, 0, r.y + 0.5f), Quaternion.identity) as Transform;
                                    r.transform.localScale = new Vector3(r.transform.localScale.x, r.transform.localScale.y, r.transform.localScale.z * 2);
                                    r.objectUnity = r.transform.gameObject;
                                    r.objectUnity.name = "Rock-" + r.id;


                                    r3.transform = r.transform;
                                    r3.objectUnity = r.objectUnity;
                                    drawed = true;
                                }
                            }
                        }


                        if (!drawed)
                        {
                            r.transform = UnityEngine.Object.Instantiate(prefab, new Vector3(r.x, 0, r.y), Quaternion.identity) as Transform;
                            r.objectUnity = r.transform.gameObject;
                            r.objectUnity.name = "Rock-" + r.id;
                        }
                    }
                }
            }
        }

        private IMyGameObject[][] _table = null;
        public IMyGameObject[][] Table
        {
            get
            {
                if (_table == null)
                {
                    _table = new IMyGameObject[1600][];
                    for (int x = 0; x < _table.Length; x++)
                    {
                        _table[x] = new IMyGameObject[1600];
                    }
                }
                return _table;
            }
            set
            {
                _table = value;
            }
        }


        public void fillBuildings()
        {
            foreach (var pl in players)
            {
                Player p = pl.Value;
                Vector2 b1 = new Vector2(p.startXB, p.startYB);
                ClearAroundSpot(b1);
                insertB(b1, p);
            }

        }

        public void insertB(Vector2 b1, Map.Data.Interfaces.Player owner)
        {
            Building r = new Building((int)b1.x, (int)b1.y);
            r.owner = owner.number;
            owner.baseBuilding = r;
            Table[(int)b1.x + 800][(int)b1.y + 800] = r;

            Transform prefab = null;
            prefab = prefabService.GetPrefab(Core.Interfaces.PrefabNames.Building);

            r.transform = UnityEngine.Object.Instantiate(prefab, new Vector3(r.x, 0, r.y + 0.5f), Quaternion.identity) as Transform;
            r.transform.localScale = new Vector3(r.transform.localScale.x, r.transform.localScale.y, r.transform.localScale.z);
            r.transform.Rotate(new Vector3(0, 0, 90));
            r.objectUnity = r.transform.gameObject;
            r.objectUnity.name = "Building-" + r.id;
        }

        public void ClearAroundSpot(Vector2 b1)
        {
            ClearSpot(b1);
            ClearSpot(new Vector2(b1.x + 1, b1.y));
            ClearSpot(new Vector2(b1.x - 1, b1.y));
            ClearSpot(new Vector2(b1.x, b1.y + 1));
            ClearSpot(new Vector2(b1.x, b1.y - 1));
        }

        public void ClearSpot(Vector2 b1)
        {
            try
            {
                if (Table[(int)b1.x + 800][(int)b1.y + 800].objectType == ObjectType.Rock)
                {
                    var obj = ((Rock)Table[(int)b1.x + 800][(int)b1.y + 800]).objectUnity;
                    GameObject.Destroy(obj);
                    Table[(int)b1.x + 800][(int)b1.y + 800] = null;
                }
            }
            catch (Exception) { }
        }


    }
}
