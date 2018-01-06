using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Map.Data;
using Core.Interfaces;

namespace Graphs.PathFinder
{
    public enum PathFinderMethod { None, A, AModified, AModified2, AAuth };

    public enum DataType { None, Matrix, List }

    public class PathFinderService
    {
        static public string[] PathFinderMethodString = { "None", "A*", "A* Modified", "A* Modified2", "A* Auth" };

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

        public IEnumerable<Vector2> visited;


        public IList<Vector2> FindWay(Vector2 from, Vector2 to, PathFinderMethod metoda, MapData map, DataType dt)
        {
            IList<Vector2> list = new List<Vector2>();
            if (metoda == PathFinderMethod.A)
            {
                list = ServiceA.FindWay(from, to, map);
                visited = ServiceA.visited.Distinct();
            }
            else if (metoda == PathFinderMethod.AModified)
            {
                list = ServiceA.FindWay_AModified(from, to, map);
                visited = ServiceA.visited.Distinct();
            }
            else if (metoda == PathFinderMethod.AModified2)
            {
                list = ServiceA.FindWay_AModified2(from, to, map);
                visited = ServiceA.visited.Distinct();
            }
            else
            {
                //if (ServiceData.listaSasiedztwa == null)
                //{
                //    ServiceData.ListaSasiedztwa(map, true);
                //}
                //if (ServiceData.macierzSasiedztwa == null)
                //{
                //    ServiceData.MacierzSasiedztwa(map, true);
                //}
                //if (ServiceData.listaIncydencji == null)
                //{
                //    ServiceData.ListaIncydencji(map, true);
                //}

                //if (metoda == PathFinderMethod.Djikstra)
                //{
                //    list = ServiceDjikstra.FindWay(from, to, map, true);
                //    //ServiceDjikstra.FindWay(from, to, map, false);
                //}

                //if (metoda == PathFinderMethod.Ford)
                //{
                //    //ServiceFord.FindWay(from, to, map, false);
                //    list = ServiceFord.FindWay(from, to, map, true);
                //}
            }


            return list;
        }

    }
}
