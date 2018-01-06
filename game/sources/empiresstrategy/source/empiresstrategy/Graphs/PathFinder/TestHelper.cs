using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Core.Interfaces;

namespace Graphs.PathFinder
{
    public class TestHelper
    {
        public static void generateTest(Map.Data.MapData map)
        {
            int powtorzenia = 100;
            map.randomFill(10);

            for (int j = 10; j < 1000; j = j + 10)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\pomiary\generateTest.txt", true))
                {
                    file.WriteLine("Rozmiar " + j);
                }
                ServiceData.start = 800 - j / 2;
                ServiceData.end = 800 + j / 2;
                ServiceData.num = (ServiceData.end - ServiceData.start);
                ServiceData.NewTabLength = ServiceData.num * ServiceData.num;

                long[] elapsedMs1 = new long[powtorzenia];
                long[] elapsedMs2 = new long[powtorzenia];
                long[] elapsedMs3 = new long[powtorzenia];

                for (int i = 0; i < powtorzenia; i++)
                {
                    System.GC.Collect();
                    ServiceData.listaSasiedztwa = null;
                    ServiceData.macierzSasiedztwa = null;
                    ServiceData.listaIncydencji = null;


                    if (ServiceData.listaSasiedztwa == null)
                    {
                        var watch1 = System.Diagnostics.Stopwatch.StartNew();
                        ServiceData.ListaSasiedztwa(map, false);
                        watch1.Stop();
                        elapsedMs1[i] = watch1.ElapsedMilliseconds;
                    }
                    if (ServiceData.macierzSasiedztwa == null)
                    {
                        var watch2 = System.Diagnostics.Stopwatch.StartNew();
                        ServiceData.MacierzSasiedztwa(map, false);
                        watch2.Stop();
                        elapsedMs2[i] = watch2.ElapsedMilliseconds;
                    }
                    if (ServiceData.listaIncydencji == null)
                    {
                        var watch3 = System.Diagnostics.Stopwatch.StartNew();
                        ServiceData.ListaIncydencji(map, false);
                        watch3.Stop();
                        elapsedMs3[i] = watch3.ElapsedMilliseconds;
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\pomiary\generateTest.txt", true))
                {
                    file.WriteLine("ListaSasiedztwa");
                    foreach (var el in elapsedMs1)
                    {
                        file.WriteLine(el);
                    }
                    file.WriteLine("macierzSasiedztwa");
                    foreach (var el in elapsedMs2)
                    {
                        file.WriteLine(el);
                    }
                    file.WriteLine("listaIncydencji");
                    foreach (var el in elapsedMs3)
                    {
                        file.WriteLine(el);
                    }
                }
            }
        }



        public static void FindWayDistTestCzworki(Map.Data.MapData map)
        {
            int powtorzenia = 10;
            map.randomFill(10);

            for (int j = 1; j < 300; j = j + 50)
            {
                Vector2 from = new Vector2(0, 0);
                Vector2 to = new Vector2(j, j);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\pomiary\FindWayDistTestCzworki.txt", true))
                {
                    file.WriteLine("PKTY (0, 0) (" + j + ", " + j + ")  odleglosc: " + ServiceA.cost(from, to));
                }

                ServiceAAuth.tab = null;

                if (ServiceAAuth.tab == null)
                {
                    ServiceAAuth.makeTab(map, false);
                }

                long[] elapsedMs1 = new long[powtorzenia];
                long[] elapsedMs5 = new long[powtorzenia];

                System.GC.Collect();


                for (int i = 0; i < powtorzenia; i++)
                {

                    map.Table[(int)from.x + 800][(int)from.y + 800] = null;
                    map.Table[(int)to.x + 800][(int)to.y + 800] = null;
                    var watch1 = System.Diagnostics.Stopwatch.StartNew();
                    ServiceAAuth.FindWay(from, to, map, false);

                    watch1.Stop();
                    elapsedMs1[i] = watch1.ElapsedMilliseconds;

                    var watch5 = System.Diagnostics.Stopwatch.StartNew();
                    ServiceA.FindWay(from, to, map, false);
                    watch5.Stop();
                    elapsedMs5[i] = watch5.ElapsedMilliseconds;
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\pomiary\FindWayDistTestCzworki.txt", true))
                {
                   
                    file.WriteLine("A* Auth ");
                    long sum = 0;
                    foreach (var el in elapsedMs1)
                    {
                        sum = sum + el;
                    }
                    file.WriteLine(sum / powtorzenia);
                    sum = 0;
                    file.WriteLine("A*");
                    foreach (var el in elapsedMs5)
                    {
                        sum = sum + el;
                    }
                    file.WriteLine(sum / powtorzenia);
                }
            }
        }


        public static void generateTestCzworki(Map.Data.MapData map)
        {
            int powtorzenia = 100;
            map.randomFill(10);

            long[] elapsedMs1 = new long[powtorzenia];

            for (int i = 0; i < powtorzenia; i++)
            {
                System.GC.Collect();
                ServiceAAuth.tab = null;

                if (ServiceAAuth.tab == null)
                {
                    var watch1 = System.Diagnostics.Stopwatch.StartNew();
                    ServiceAAuth.makeTab(map, false);
                    watch1.Stop();
                    elapsedMs1[i] = watch1.ElapsedMilliseconds;
                }
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\pomiary\generateTestCzworki.txt", true))
            {
                file.WriteLine("Generowanie czworki");
                foreach (var el in elapsedMs1)
                {
                    file.WriteLine(el);
                }
            }
        }

        public static void FindWaySizeTest(Map.Data.MapData map)
        {
            int powtorzenia = 100;
            map.randomFill(10);

            for (int j = 10; j < 200; j = j + 10)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\pomiary\FindWaySizeTest.txt", true))
                {
                    file.WriteLine("Rozmiar " + j);
                }
                ServiceData.start = 800 - j / 2;
                ServiceData.end = 800 + j / 2;
                ServiceData.num = (ServiceData.end - ServiceData.start);
                ServiceData.NewTabLength = ServiceData.num * ServiceData.num;

                long[] elapsedMs1 = new long[powtorzenia];
                long[] elapsedMs2 = new long[powtorzenia];
                long[] elapsedMs3 = new long[powtorzenia];
                long[] elapsedMs4 = new long[powtorzenia];
                long[] elapsedMs5 = new long[powtorzenia];
                long[] elapsedMs6 = new long[powtorzenia];

                System.GC.Collect();
                ServiceData.listaSasiedztwa = null;
                ServiceData.macierzSasiedztwa = null;
                ServiceData.listaIncydencji = null;

                ServiceData.ListaSasiedztwa(map, false);
                ServiceData.MacierzSasiedztwa(map, false);
                ServiceData.ListaIncydencji(map, false);

                for (int i = 0; i < powtorzenia; i++)
                {
                    Vector2 from = new Vector2(2, 2);
                    Vector2 to = new Vector2(50, 50);

                    map.Table[(int)from.x][(int)from.y] = null;
                    map.Table[(int)to.x][(int)to.y] = null;

                    var watch1 = System.Diagnostics.Stopwatch.StartNew();
                    ServiceDjikstra.FindWay(from, to, map, true);
                    watch1.Stop();
                    elapsedMs1[i] = watch1.ElapsedMilliseconds;

                    var watch2 = System.Diagnostics.Stopwatch.StartNew();
                    ServiceDjikstra.FindWay(from, to, map, false);
                    watch2.Stop();
                    elapsedMs2[i] = watch2.ElapsedMilliseconds;

                    var watch3 = System.Diagnostics.Stopwatch.StartNew();
                    //ServiceFord.FindWay(from, to, map, true);
                    watch3.Stop();
                    elapsedMs3[i] = watch3.ElapsedMilliseconds;

                    var watch4 = System.Diagnostics.Stopwatch.StartNew();
                    //ServiceFord.FindWay(from, to, map, false);
                    watch4.Stop();
                    elapsedMs4[i] = watch4.ElapsedMilliseconds;

                    var watch5 = System.Diagnostics.Stopwatch.StartNew();
                    ServiceA.FindWay(from, to, map);
                    watch5.Stop();
                    elapsedMs5[i] = watch5.ElapsedMilliseconds;


                    var watch6 = System.Diagnostics.Stopwatch.StartNew();
                    ServiceA.FindWay(from, to, map);
                    watch6.Stop();
                    elapsedMs6[i] = watch6.ElapsedMilliseconds;
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\pomiary\FindWaySizeTest.txt", true))
                {
                    file.WriteLine("Djikstra lista ");
                    foreach (var el in elapsedMs1)
                    {
                        file.WriteLine(el);
                    }
                    file.WriteLine("Djikstra macierz ");
                    foreach (var el in elapsedMs2)
                    {
                        file.WriteLine(el);
                    }
                    file.WriteLine("Ford Lista");
                    foreach (var el in elapsedMs3)
                    {
                        file.WriteLine(el);
                    }
                    file.WriteLine("Ford macierz");
                    foreach (var el in elapsedMs4)
                    {
                        file.WriteLine(el);
                    }
                    file.WriteLine("A*");
                    foreach (var el in elapsedMs5)
                    {
                        file.WriteLine(el);
                    }
                    file.WriteLine("A* Modified");
                    foreach (var el in elapsedMs6)
                    {
                        file.WriteLine(el);
                    }
                }
            }
        }

        public static void FindWayDistTest(Map.Data.MapData map)
        {
            int powtorzenia = 100;
            map.randomFill(10);

            for (int j = 1; j < 300; j = j + 50)
            {
                Vector2 from = new Vector2(0, 0);
                Vector2 to = new Vector2(j, j);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\pomiary\FindWayDistTest.txt", true))
                {
                    file.WriteLine("PKTY (0, 0) (" + j + ", " + j + ")  odleglosc: " + ServiceA.cost(from, to));
                }

                ServiceData.start = 800 - 300 / 2;
                ServiceData.end = 800 + 300 / 2;
                ServiceData.num = (ServiceData.end - ServiceData.start);
                ServiceData.NewTabLength = ServiceData.num * ServiceData.num;
                Debug.Log("0");
                long[] elapsedMs1 = new long[powtorzenia];
                long[] elapsedMs2 = new long[powtorzenia];
                long[] elapsedMs3 = new long[powtorzenia];
                long[] elapsedMs4 = new long[powtorzenia];
                long[] elapsedMs5 = new long[powtorzenia];

                System.GC.Collect();
                ServiceData.listaSasiedztwa = null;
                ServiceData.macierzSasiedztwa = null;
                ServiceData.listaIncydencji = null;

                ServiceData.ListaIncydencji(map, false);
                //ServiceData.MacierzSasiedztwa(map, false);

                for (int i = 0; i < powtorzenia; i++)
                {

                    Debug.Log("1 " + ((int)from.x + 800) + " " + ((int)from.y + 800));
                    map.Table[(int)from.x + 800][(int)from.y + 800] = null;
                    Debug.Log("1 " + ((int)to.x + 800) + " " + ((int)to.y + 800));
                    map.Table[(int)to.x + 800][(int)to.y + 800] = null;
                    var watch1 = System.Diagnostics.Stopwatch.StartNew();
                    ServiceDjikstra.FindWay(from, to, map, true);
                    watch1.Stop();
                    elapsedMs1[i] = watch1.ElapsedMilliseconds;

                    var watch5 = System.Diagnostics.Stopwatch.StartNew();
                    ServiceA.FindWay(from, to, map);
                    watch5.Stop();
                    elapsedMs5[i] = watch5.ElapsedMilliseconds;
                    Debug.Log("3");
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"pomiar\pomiary\FindWayDistTest.txt", true))
                {
                    file.WriteLine("Djikstra lista ");
                    long sum = 0;
                    foreach (var el in elapsedMs1)
                    {
                        sum = sum + el;
                    }
                    file.WriteLine(sum / powtorzenia);
                    sum = 0;
                    file.WriteLine("A*");
                    foreach (var el in elapsedMs5)
                    {
                        sum = sum + el;
                    }
                    file.WriteLine(sum / powtorzenia);
                }
            }
        }




        #region Services

        private static ServiceDjikstra _ServiceDjikstra;
        public static ServiceDjikstra ServiceDjikstra
        {
            get
            {
                if (_ServiceDjikstra == null) _ServiceDjikstra = ServiceLocator.GetService<ServiceDjikstra>();
                return _ServiceDjikstra;
            }
            set
            {
                _ServiceDjikstra = value;
            }
        }

        private static ServiceFord _ServiceFord;
        public static ServiceFord ServiceFord
        {
            get
            {
                if (_ServiceFord == null) _ServiceFord = ServiceLocator.GetService<ServiceFord>();
                return _ServiceFord;
            }
            set
            {
                _ServiceFord = value;
            }
        }


        private static ServiceA _ServiceA;
        public static ServiceA ServiceA
        {
            get
            {
                if (_ServiceA == null) _ServiceA = ServiceLocator.GetService<ServiceA>();
                return _ServiceA;
            }
            set
            {
                _ServiceA = value;
            }
        }

        private static ServiceAAuth _ServiceAAuth;
        public static ServiceAAuth ServiceAAuth
        {
            get
            {
                if (_ServiceAAuth == null) _ServiceAAuth = ServiceLocator.GetService<ServiceAAuth>();
                return _ServiceAAuth;
            }
            set
            {
                _ServiceAAuth = value;
            }
        }

        #endregion
    }
}
