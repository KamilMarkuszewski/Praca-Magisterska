using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interfaces;
using UnityEngine;


namespace Graphs.PathFinder
{
    class ServiceData
    {
        public const int INF = 1000000;

        public static int[][] macierzSasiedztwa = null;
        public static IList<Edge> listaSasiedztwa = null;

        public static IList<int>[] listaIncydencji = null;

        public static int start = 700; //powinno byc 0
        public static int end = 900; //powinno byc 1600
        public static int num = (end - start);
        static int offset = 800; //powinno byc 1600
        public static int NewTabLength = num * num; //powinno byc 1600

        public static Vector2 posInTableToVector(int pos)
        {
            int y = pos % num;
            int x = (pos - y) / num;
            y = y - num / 2;
            x = x - num / 2;
            return new Vector2(x, y);
        }

        public static int posInTable(int i, int j)
        {
            if (i < 0 || j < 0) return -1;
            int p = i * num + j;
            //Debug.Log("posInTable" + p + " i " + i + " j " + j);
            return p;
        }

        public static int posInTable(Vector2 p)
        {
            int i_ = (int)p.x + num / 2;
            int j_ = (int)p.y + num / 2;
            int me = posInTable(i_, j_);
            //Debug.Log("posInTable" + me + " i " + i_ + " j " + j_);
            return me;
        }

        public static int posInTableA(Vector2 p)
        {
            int i_ = (int)p.x + 1600 / 2;
            int j_ = (int)p.y + 1600 / 2;
            int me = posInTable(i_, j_);
            return me;
        }

        internal static void MacierzSasiedztwa(Map.Data.MapData map, bool debug)
        {
            System.GC.Collect();
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (macierzSasiedztwa == null)
            {
                macierzSasiedztwa = new int[NewTabLength][];
                for (int i = 0; i < NewTabLength; i++)
                {
                    macierzSasiedztwa[i] = new int[NewTabLength];
                    for (int j = 0; j < NewTabLength; j++)
                    {
                        macierzSasiedztwa[i][j] = INF;
                    }
                }
            }
            for (int i = start; i < end; i++)
            {
                for (int j = start; j < end; j++)
                {
                    int i_ = i - offset + num / 2;
                    int j_ = j - offset + num / 2;
                    int me = posInTable(i_, j_);
                    int top = posInTable(i_ + 1, j_);
                    int bot = posInTable(i_ - 1, j_);
                    int r = posInTable(i_, j_ + 1);
                    int l = posInTable(i_, j_ - 1);
                    //Debug.Log("W Nowej " + "i " + i_ + " j" + j_ + " me" + me);
                    //Debug.Log("W GLOWNEJ " + "i " + i + " j" + j);

                    if (map.Table[i][j] != null && (map.Table[i][j].objectType == ObjectType.Rock || map.Table[i][j].objectType == ObjectType.Building))
                    {

                    }
                    else
                    {
                        //Debug.Log("me " + me + "i " + i_ + " j" + j_);
                        //top
                        if (i_ + 1 < num && i_ + 1 >= 0 && top >= 0)
                        {
                            if (map.Table[i + 1][j] != null && (map.Table[i + 1][j].objectType == ObjectType.Rock || map.Table[i + 1][j].objectType == ObjectType.Building))
                            {
                                //tab[me][top] = 0;
                            }
                            else
                            {
                                //Debug.Log("top polaczenie " + me + ' ' + top);
                                macierzSasiedztwa[me][top] = 1;
                            }
                        }
                        //bot
                        if (i_ - 1 >= 0 && bot >= 0)
                        {
                            if (map.Table[i - 1][j] != null && (map.Table[i - 1][j].objectType == ObjectType.Rock || map.Table[i - 1][j].objectType == ObjectType.Building))
                            {
                                //tab[me][bot] = 0;
                            }
                            else
                            {
                                //Debug.Log("bot polaczenie " + me + ' ' + top);
                                macierzSasiedztwa[me][bot] = 1;
                            }
                        }
                        //r
                        if (j_ + 1 < num && j_ + 1 > 0 && r >= 0)
                        {
                            if (map.Table[i][j + 1] != null && (map.Table[i][j + 1].objectType == ObjectType.Rock || map.Table[i][j + 1].objectType == ObjectType.Building))
                            {
                                //tab[me][r] = 0;
                            }
                            else
                            {
                                //Debug.Log("r polaczenie " + me + ' ' + r);
                                macierzSasiedztwa[me][r] = 1;
                            }
                        }
                        //l
                        if (j_ - 1 >= 0 && l >= 0)
                        {
                            if (map.Table[i][j - 1] != null && (map.Table[i][j - 1].objectType == ObjectType.Rock || map.Table[i][j - 1].objectType == ObjectType.Building))
                            {
                                //tab[me][l] = 0;
                            }
                            else
                            {
                                //Debug.Log("l polaczenie " + me + ' ' + l);
                                macierzSasiedztwa[me][l] = 1;
                            }
                        }
                    }
                }
            }
            if (debug)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\logs-times.txt", true))
                {
                    //Debug.Log("Tworzenie macierzy sasiedztwa czas[ms]: " + elapsedMs);
                    file.WriteLine("Tworzenie macierzy sasiedztwa czas[ms]: " + elapsedMs);
                }

                //Debug.Log("savetofile");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\wygenerowana-macierz-sasiedztwa.txt"))
                {
                    for (int i = 0; i < NewTabLength; i++)
                    {
                        for (int j = 0; j < NewTabLength; j++)
                        {
                            file.Write(macierzSasiedztwa[i][j] + " ");
                        }
                        file.WriteLine("");
                    }
                }
            }
        }



        internal static void ListaSasiedztwa(Map.Data.MapData map, bool debug)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (listaSasiedztwa == null)
            {
                listaSasiedztwa = new List<Edge>();
            }
            for (int i = start; i < end; i++)
            {
                for (int j = start; j < end; j++)
                {
                    int i_ = i - offset + num / 2;
                    int j_ = j - offset + num / 2;
                    int me = posInTable(i_, j_);
                    int bot = posInTable(i_ - 1, j_);
                    int r = posInTable(i_, j_ + 1);

                    if (map.Table[i][j] != null && (map.Table[i][j].objectType == ObjectType.Rock || map.Table[i][j].objectType == ObjectType.Building))
                    {

                    }
                    else
                    {
                        //bot
                        if (i_ - 1 >= 0 && bot >= 0)
                        {
                            if (map.Table[i - 1][j] != null && (map.Table[i - 1][j].objectType == ObjectType.Rock || map.Table[i - 1][j].objectType == ObjectType.Building))
                            {
                                //tab[me][bot] = 0;
                            }
                            else
                            {
                                //Debug.Log("bot polaczenie " + me + ' ' + top);
                                //tab[me][bot] = 1;
                                listaSasiedztwa.Add(new Edge(me, bot, 1));
                            }
                        }
                        //r
                        if (j_ + 1 < num && j_ + 1 > 0 && r >= 0)
                        {
                            if (map.Table[i][j + 1] != null && (map.Table[i][j + 1].objectType == ObjectType.Rock || map.Table[i][j + 1].objectType == ObjectType.Building))
                            {
                                //tab[me][r] = 0;
                            }
                            else
                            {
                                //Debug.Log("r polaczenie " + me + ' ' + r);
                                //tab[me][r] = 1;
                                listaSasiedztwa.Add(new Edge(me, r, 1));
                            }
                        }
                    }
                }
            }
            if (debug)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\logs-times.txt", true))
                {
                    //Debug.Log("Tworzenie listy sasiedzctwa czas[ms]: " + elapsedMs);
                    file.WriteLine("Tworzenie listy sasiedzctwa czas[ms]: " + elapsedMs);
                }

                //Debug.Log("savetofile");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\wygenerowana-lista-sasiedztwa.txt"))
                {

                    foreach (Edge elem in listaSasiedztwa)
                    {
                        file.WriteLine(elem.toString());
                    }

                }
            }
        }

        internal static void ListaIncydencji(Map.Data.MapData map, bool debug)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (listaIncydencji == null)
            {
                listaIncydencji = new IList<int>[NewTabLength];
                for (int i = 0; i < NewTabLength; i++)
                {
                    listaIncydencji[i] = new List<int>();
                }
            }
            for (int i = start; i < end; i++)
            {
                for (int j = start; j < end; j++)
                {
                    int i_ = i - offset + num / 2;
                    int j_ = j - offset + num / 2;
                    int me = posInTable(i_, j_);
                    int top = posInTable(i_ + 1, j_);
                    int bot = posInTable(i_ - 1, j_);
                    int r = posInTable(i_, j_ + 1);
                    int l = posInTable(i_, j_ - 1);
                    //Debug.Log("W Nowej " + "i " + i_ + " j" + j_ + " me" + me);
                    //Debug.Log("W GLOWNEJ " + "i " + i + " j" + j);

                    if (map.Table[i][j] != null && (map.Table[i][j].objectType == ObjectType.Rock || map.Table[i][j].objectType == ObjectType.Building))
                    {

                    }
                    else
                    {
                        //Debug.Log("me " + me + "i " + i_ + " j" + j_);
                        //top
                        if (i_ + 1 < num && i_ + 1 >= 0 && top >= 0)
                        {
                            if (map.Table[i + 1][j] != null && (map.Table[i + 1][j].objectType == ObjectType.Rock || map.Table[i + 1][j].objectType == ObjectType.Building))
                            {
                            }
                            else
                            {
                                //Debug.Log("top polaczenie " + me + ' ' + top);
                                listaIncydencji[me].Add(top);
                            }
                        }
                        //bot
                        if (i_ - 1 >= 0 && bot >= 0)
                        {
                            if (map.Table[i - 1][j] != null && (map.Table[i - 1][j].objectType == ObjectType.Rock || map.Table[i - 1][j].objectType == ObjectType.Building))
                            {
                            }
                            else
                            {
                                //Debug.Log("bot polaczenie " + me + ' ' + top);
                                listaIncydencji[me].Add(bot);
                            }
                        }
                        //r
                        if (j_ + 1 < num && j_ + 1 > 0 && r >= 0)
                        {
                            if (map.Table[i][j + 1] != null && (map.Table[i][j + 1].objectType == ObjectType.Rock || map.Table[i][j + 1].objectType == ObjectType.Building))
                            {
                            }
                            else
                            {
                                //Debug.Log("r polaczenie " + me + ' ' + r);
                                listaIncydencji[me].Add(r);
                            }
                        }
                        //l
                        if (j_ - 1 >= 0 && l >= 0)
                        {
                            if (map.Table[i][j - 1] != null && (map.Table[i][j - 1].objectType == ObjectType.Rock || map.Table[i][j - 1].objectType == ObjectType.Building))
                            {
                                //tab[me][l] = 0;
                            }
                            else
                            {
                                //Debug.Log("l polaczenie " + me + ' ' + l);
                                listaIncydencji[me].Add(l);
                            }
                        }
                    }
                }
            }

            if (debug)
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\logs-times.txt", true))
                {
                    //Debug.Log("Tworzenie listy incydencji czas[ms]: " + elapsedMs);
                    file.WriteLine("Tworzenie listy incydencji czas[ms]: " + elapsedMs);
                }

                //Debug.Log("savetofile");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\wygenerowana-lista-incydencji.txt"))
                {
                    for (int i = 0; i < NewTabLength; i++)
                    {
                        file.Write("w: " + i + " laczy sie z: ");
                        foreach (var elem in listaIncydencji[i])
                        {
                            file.Write(elem + ", ");
                        }
                        file.WriteLine("");
                    }
                }
            }
        }


        public class Edge
        {
            public Edge(int w1, int w2, int w)
            {
                this.w1 = w1;
                this.w2 = w2;
                this.w = w;
            }
            public int w1;
            public int w2;
            public int w;

            public string toString()
            {
                return "w1: " + w1 + "w2: " + w2 + "w: " + w;
            }
        }
    }
}
