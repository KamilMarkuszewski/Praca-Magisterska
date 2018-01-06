using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Map.Data;

namespace Graphs.PathFinder
{
    public class ServiceFord
    {
        public IList<Vector2> FindWay(Vector2 from, Vector2 to, MapData map, bool useList)
        {
            IList<Vector2> list = new List<Vector2>();
            int DEST = ServiceData.posInTable(to);
            int START = ServiceData.posInTable(from);
            //Debug.Log("start " + START + " dest " + DEST);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            Queue<int> q = new Queue<int>();
            int wielkoscGrafu = ServiceData.NewTabLength;
            // int[] odl = new int[ServiceData.NewTabLength];         // aktualna najmniejsza odleglosc
            int[] odleglosc = new int[wielkoscGrafu];
            int[] poprzedni = new int[wielkoscGrafu];
            int[] wezly = new int[wielkoscGrafu];

            for (int i = 0; i < ServiceData.NewTabLength; i++)
            {
                odleglosc[i] = poprzedni[i] = ServiceData.INF;
                wezly[i] = i;
            }

            odleglosc[START] = 0;

            if (!useList)
            {
                for (int i = 1; i < wielkoscGrafu - 1; i++)
                {
                    for (int w1 = 0; w1 < wielkoscGrafu; w1++)
                    {
                        for (int w2 = 0; w2 < wielkoscGrafu; w2++)
                        {
                            if (ServiceData.macierzSasiedztwa[w1][w2] != 0)
                            {
                                int tmp = odleglosc[w2] + ServiceData.macierzSasiedztwa[w1][w2];
                                if (odleglosc[w1] > tmp)
                                {
                                    odleglosc[w1] = tmp;
                                    poprzedni[w1] = w2;
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                for (int i = 1; i < wielkoscGrafu - 1; i++)
                {
                    for (int w1 = 0; w1 < wielkoscGrafu; w1++)
                    {
                        foreach (var w2 in ServiceData.listaIncydencji[w1])
                        {

                            int waga = w2;
                            if (waga != 0)
                            {
                                //Debug.Log(" waga "+ waga + " w2 " + w2 + " w1 " + w1);
                                int tmp = odleglosc[w2] + 1;
                                // Debug.Log(" ustawiam " + odleglosc[w1] + " odl" + (odleglosc[w2] + 1));
                                if (odleglosc[w1] > tmp)
                                {

                                    odleglosc[w1] = tmp;
                                    poprzedni[w1] = w2;
                                }
                            }
                        }

                    }
                }
            }



            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            int[] sciezka = OdtworzSciezke(poprzedni, START, DEST);
            saveLogs(elapsedMs, odleglosc, poprzedni, wezly, sciezka, useList);
            foreach (var elem in sciezka)
            {
                list.Add(ServiceData.posInTableToVector(elem));
                //Debug.Log("pkt " + ServiceData.posInTableToVector(elem));
            }
            return list;
        }

        public int[] OdtworzSciezke(int[] prev, int SRC, int DEST)
        {
            try
            {
                int[] ret = new int[prev.Length];
                int currentNode = 0;
                ret[currentNode] = DEST;
                while (ret[currentNode] != ServiceData.INF && ret[currentNode] != SRC)
                {
                    ret[currentNode + 1] = prev[ret[currentNode]];
                    currentNode++;
                }

                if (ret[currentNode] != SRC)
                    return null;
                int[] reversed = new int[currentNode + 1];
                for (int i = currentNode; i >= 0; i--)
                    reversed[currentNode - i] = ret[i];
                return reversed;
            }
            catch (Exception) { }
            return new int[0];
        }

        private void saveLogs(long elapsedMs, int[] dist, int[] prev, int[] nodes, int[] sciezka, bool useList)
        {
            string typ = "";
            if (!useList) typ = "macierz";
            else typ = "lista";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\logs-times.txt", true))
            {
                //Debug.Log("Najkrotsza droga ford czas[ms]: " + elapsedMs);
                file.WriteLine("Najkrotsza droga ford " + typ + " czas[ms]: " + elapsedMs);
            }
            string fileName = @"pomiar\ford-" + typ + "-wyniki.txt";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                if (sciezka != null)
                {
                    file.WriteLine("Wypisanie po algorytmie: ");
                    file.WriteLine("Sciezka: ");
                    for (int i = 0; i < sciezka.Length; i++)
                    {
                        file.Write(sciezka[i] + ", ");
                    }
                }
                else
                {
                    file.WriteLine("Sciezka nie znaleziona ");
                }
                file.WriteLine("");
                file.WriteLine("Najkrotsza droga z 0 do kolejnych wierzcholkow: ");

                for (int i = 0; i < ServiceData.NewTabLength; i++)
                {
                    file.WriteLine("dist " + dist[i] + " , prev  " + prev[i] + " ,  node " + nodes[i] + " ,  " + i);

                }
            }
        }

    }
}
