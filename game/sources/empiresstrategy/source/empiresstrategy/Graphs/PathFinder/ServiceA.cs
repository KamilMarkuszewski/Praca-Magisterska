using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Map.Data;
using Core.Interfaces;

namespace Graphs.PathFinder
{
    public class ServiceA
    {
        public IList<Vector2> visited;

        #region logs

        private void saveLogs(long elapsedMs, IList<Vector2> sciezka)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\logs-times.txt", true))
            {
                file.WriteLine("Najkrotsza droga A czas[ms]: " + elapsedMs);
            }
            string fileName = @"pomiar\A-wyniki.txt";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                if (sciezka != null)
                {
                    file.WriteLine("Wypisanie po algorytmie: ");
                    file.WriteLine("Sciezka: ");
                    foreach (var el in sciezka)
                    {
                        file.Write(el + " (" + ServiceData.posInTable(el) + ") " + ", ");
                    }
                }
                else
                {
                    file.WriteLine("Sciezka nie znaleziona ");
                }
            }
        }
        #endregion

        #region AModified

        public IList<Vector2> FindWay_AModified(Vector2 from, Vector2 to, MapData map)
        {
            visited = new List<Vector2>();
            IList<Vector2> list = new List<Vector2>();
            from = new Vector2((int)from.x, (int)from.y);
            to = new Vector2((int)to.x, (int)to.y);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //Debug.Log(from + " to " + to);
            Dictionary<int, ANode> open = new Dictionary<int, ANode>();
            Dictionary<int, ANode> closed = new Dictionary<int, ANode>();

            Vector2 cur = from;
            ANode s = new ANode(from, 0, cost(cur, to), null);
            if (!open.ContainsKey(ServiceData.posInTableA(s.vector))) open.Add(ServiceData.posInTableA(s.vector), s);
            int max = 0;
            while (open.Count > 0 && max++ < 5000)
            {
                var best = open.OrderBy(f => f.Value.TotalCost).ToList().FirstOrDefault();

                if (best.Value.vector.Equals(to))
                {
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    list.Add(best.Value.vector);
                    ANode act = best.Value.parent;
                    do
                    {
                        if (act != null)
                        {
                            list.Add(act.vector);
                            act = act.parent;
                        }
                    } while (act != null);
                    list = list.Reverse().ToList();
                    saveLogs(elapsedMs, list);
                    break;
                }
                else
                {
                    int i = (int)best.Value.vector.x;
                    int j = (int)best.Value.vector.y;

                    //top
                    if (isEmpty(i, j + 1, map))
                    {
                        checkNode_AModified(i, j + 1, from, to, open, closed, best.Value);
                    }
                    //bot
                    if (isEmpty(i, j - 1, map))
                    {
                        checkNode_AModified(i, j - 1, from, to, open, closed, best.Value);
                    }
                    //r
                    if (isEmpty(i + 1, j, map))
                    {
                        checkNode_AModified(i + 1, j, from, to, open, closed, best.Value);
                    }
                    //l
                    if (isEmpty(i - 1, j, map))
                    {
                        checkNode_AModified(i - 1, j, from, to, open, closed, best.Value);
                    }
                }
                open.Remove(ServiceData.posInTableA(best.Value.vector));

                if (!closed.ContainsKey(ServiceData.posInTableA(best.Value.vector)))
                    closed.Add(ServiceData.posInTableA(best.Value.vector), best.Value);
            }


            return list;
        }

        public void checkNode_AModified(int i, int j, Vector2 from, Vector2 to, Dictionary<int, ANode> open, Dictionary<int, ANode> closed, ANode best)
        {
            Vector2 curNe = new Vector2(i, j);
            ANode curNeNode = new ANode(curNe, cost(from, curNe), cost(curNe, to), best);
            float NewCost = best.costFromStart + 1f;

            if(best.parent != null){
                if (best.vector.x != curNe.x || best.vector.x != best.parent.vector.x) {
                    if (best.vector.y != curNe.y || best.vector.y != best.parent.vector.y)
                    {
                        NewCost = best.costFromStart + 1.1f;
                    }
                }
            }
            ANode foundOpen = null;
            open.TryGetValue(ServiceData.posInTableA(curNe), out foundOpen);
            ANode foundClosed = null;
            closed.TryGetValue(ServiceData.posInTableA(curNe), out foundClosed);

            if (curNeNode.costFromStart <= NewCost && (foundClosed != null || foundOpen != null)) return;
            curNeNode.costFromStart = NewCost;
            curNeNode.TotalCost = curNeNode.costToGoal + curNeNode.costFromStart;
            if (foundClosed != null)
            {
                closed.Remove(ServiceData.posInTableA(foundClosed.vector));
                Debug.Log("foundClosed");
            }
            if (foundOpen != null)
            {
                open.Remove(ServiceData.posInTableA(foundOpen.vector));
                open.Add(ServiceData.posInTableA(curNeNode.vector), curNeNode);
                Debug.Log("foundOpen");
            }
            else
            {
                open.Add(ServiceData.posInTableA(curNe), curNeNode);
                visited.Add(new Vector2(i, j));
            }
        }

        #endregion

        #region AModified2

        public IList<Vector2> FindWay_AModified2(Vector2 from, Vector2 to, MapData map)
        {
            visited = new List<Vector2>();
            IList<Vector2> list = new List<Vector2>();
            from = new Vector2((int)from.x, (int)from.y);
            to = new Vector2((int)to.x, (int)to.y);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //Debug.Log(from + " to " + to);
            Dictionary<int, ANode> open = new Dictionary<int, ANode>();
            Dictionary<int, ANode> closed = new Dictionary<int, ANode>();

            Vector2 cur = from;
            ANode s = new ANode(from, 0, cost(cur, to), null);
            if (!open.ContainsKey(ServiceData.posInTableA(s.vector))) open.Add(ServiceData.posInTableA(s.vector), s);
            int max = 0;
            while (open.Count > 0 && max++ < 5000)
            {
                var best = open.OrderBy(f => f.Value.TotalCost).ToList().FirstOrDefault();

                if (best.Value.vector.Equals(to))
                {
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    list.Add(best.Value.vector);
                    ANode act = best.Value.parent;
                    do
                    {
                        if (act != null)
                        {
                            list.Add(act.vector);
                            act = act.parent;
                        }
                    } while (act != null);
                    list = list.Reverse().ToList();
                    saveLogs(elapsedMs, list);
                    break;
                }
                else
                {
                    int i = (int)best.Value.vector.x;
                    int j = (int)best.Value.vector.y;

                    //top
                    if (isEmpty(i, j + 1, map))
                    {
                        checkNode_AModified2(i, j + 1, from, to, open, closed, best.Value);
                    }
                    //bot
                    if (isEmpty(i, j - 1, map))
                    {
                        checkNode_AModified2(i, j - 1, from, to, open, closed, best.Value);
                    }
                    //r
                    if (isEmpty(i + 1, j, map))
                    {
                        checkNode_AModified2(i + 1, j, from, to, open, closed, best.Value);
                    }
                    //l
                    if (isEmpty(i - 1, j, map))
                    {
                        checkNode_AModified2(i - 1, j, from, to, open, closed, best.Value);
                    }
                }
                open.Remove(ServiceData.posInTableA(best.Value.vector));

                if (!closed.ContainsKey(ServiceData.posInTableA(best.Value.vector)))
                    closed.Add(ServiceData.posInTableA(best.Value.vector), best.Value);
            }


            return list;
        }

        public void checkNode_AModified2(int i, int j, Vector2 from, Vector2 to, Dictionary<int, ANode> open, Dictionary<int, ANode> closed, ANode best)
        {
            Vector2 curNe = new Vector2(i, j);
            ANode curNeNode = new ANode(curNe, cost(from, curNe), cost(curNe, to), best);
            float NewCost = best.costFromStart + 1f;

            if (best.parent != null)
            {
                if (best.vector.x == curNe.x && best.vector.x == best.parent.vector.x)
                {
                    NewCost = best.costFromStart + 1.1f;
                }
                if (best.vector.y == curNe.y && best.vector.y == best.parent.vector.y)
                {
                    NewCost = best.costFromStart + 1.1f;
                }
            }
            ANode foundOpen = null;
            open.TryGetValue(ServiceData.posInTableA(curNe), out foundOpen);
            ANode foundClosed = null;
            closed.TryGetValue(ServiceData.posInTableA(curNe), out foundClosed);

            if (curNeNode.costFromStart <= NewCost && (foundClosed != null || foundOpen != null)) return;
            curNeNode.costFromStart = NewCost;
            curNeNode.TotalCost = curNeNode.costToGoal + curNeNode.costFromStart;
            if (foundClosed != null)
            {
                closed.Remove(ServiceData.posInTableA(foundClosed.vector));
                Debug.Log("foundClosed");
            }
            if (foundOpen != null)
            {
                open.Remove(ServiceData.posInTableA(foundOpen.vector));
                open.Add(ServiceData.posInTableA(curNeNode.vector), curNeNode);
                Debug.Log("foundOpen");
            }
            else
            {
                open.Add(ServiceData.posInTableA(curNe), curNeNode);
                visited.Add(new Vector2(i, j));
            }
        }

        #endregion

        #region A

        public IList<Vector2> FindWay(Vector2 from, Vector2 to, MapData map, bool logs = true)
        {
            visited = new List<Vector2>();
            IList<Vector2> list = new List<Vector2>();
            from = new Vector2((int)from.x, (int)from.y);
            to = new Vector2((int)to.x, (int)to.y);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //Debug.Log(from + " to " + to);
            Dictionary<int, ANode> open = new Dictionary<int, ANode>();
            Dictionary<int, ANode> closed = new Dictionary<int, ANode>();

            Vector2 cur = from;
            ANode s = new ANode(from, 0, cost(cur, to), null);
            if (!open.ContainsKey(ServiceData.posInTableA(s.vector))) open.Add(ServiceData.posInTableA(s.vector), s);
            int max = 0;
            while (open.Count > 0 && max++ < 5000)
            {
                var best = open.OrderBy(f => f.Value.TotalCost).ToList().FirstOrDefault();

                if (best.Value.vector.Equals(to))
                {
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    list.Add(best.Value.vector);
                    ANode act = best.Value.parent;
                    do
                    {
                        if (act != null)
                        {
                            list.Add(act.vector);
                            act = act.parent;
                        }                        
                    } while (act != null);
                    list = list.Reverse().ToList();
                    if(logs) saveLogs(elapsedMs, list);
                    break;
                }
                else
                {
                    int i = (int)best.Value.vector.x;
                    int j = (int)best.Value.vector.y;

                    //top
                    if (isEmpty(i, j + 1, map))
                    {
                        checkNode(i, j + 1, from, to, open, closed, best.Value);
                    }
                    //bot
                    if (isEmpty(i, j - 1, map))
                    {
                        checkNode(i, j - 1, from, to, open, closed, best.Value);
                    }
                    //r
                    if (isEmpty(i + 1, j, map))
                    {
                        checkNode(i + 1, j, from, to, open, closed, best.Value);
                    }
                    //l
                    if (isEmpty(i - 1, j, map))
                    {
                        checkNode(i - 1, j, from, to, open, closed, best.Value);
                    }
                }
                open.Remove(ServiceData.posInTableA(best.Value.vector));

                if (!closed.ContainsKey(ServiceData.posInTableA(best.Value.vector)))
                    closed.Add(ServiceData.posInTableA(best.Value.vector), best.Value);
            }


            return list;
        }

        public void checkNode(int i, int j, Vector2 from, Vector2 to, Dictionary<int, ANode> open, Dictionary<int, ANode> closed, ANode best)
        {
            Vector2 curNe = new Vector2(i, j);
            ANode curNeNode = new ANode(curNe, cost(from, curNe), cost(curNe, to), best);
            float NewCost = best.costFromStart + 1f;


            ANode foundOpen = null;
            open.TryGetValue(ServiceData.posInTableA(curNe), out foundOpen);
            ANode foundClosed = null;
            closed.TryGetValue(ServiceData.posInTableA(curNe), out foundClosed);

            if (curNeNode.costFromStart <= NewCost && (foundClosed != null || foundOpen != null)) return;
            curNeNode.costFromStart = NewCost;
            curNeNode.TotalCost = curNeNode.costToGoal + curNeNode.costFromStart;

            if (foundClosed != null)
            {
                closed.Remove(ServiceData.posInTableA(foundClosed.vector));
            }

            if (foundOpen != null)
            {
                foundOpen = curNeNode;
            }
            else
            {
                open.Add(ServiceData.posInTableA(curNeNode.vector), curNeNode);
                visited.Add(new Vector2(i, j));
            }
        }

        public bool isEmpty(int i, int j, MapData map)
        {
            i = i + 800;
            j = j + 800;
            try
            {
                if (map.Table[i][j] != null && (map.Table[i][j].objectType == ObjectType.Rock))
                {
                    return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }

        public int cost(Vector2 from, Vector2 to)
        {
            int x = Math.Abs((int)from.x - (int)to.x);
            int y = Math.Abs((int)from.y - (int)to.y);
            return x + y;
        }

        #endregion 



        #region ANode
        public class ANode
        {
            public Vector2 vector;
            public float costFromStart;
            public float costToGoal;
            public ANode parent;
            public float TotalCost;
            public ANode(Vector2 vector, float costFromStart, float costToGoal, ANode parent)
            {
                this.vector = vector;
                this.costFromStart = costFromStart;
                this.costToGoal = costToGoal;
                TotalCost = costToGoal + costFromStart;
                this.parent = parent;
            }
        }

        #endregion

    }
}
