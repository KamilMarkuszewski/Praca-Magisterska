using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interfaces;
using Graphs.PathFinder;

namespace Graphs
{
    public class Initializator
    {
        public static void Init()
        {
            try{
                ServiceLocator.GetDictionary().Add(typeof(PathFinderService), new PathFinderService());
                ServiceLocator.GetDictionary().Add(typeof(ServiceData), new ServiceData());
                ServiceLocator.GetDictionary().Add(typeof(ServiceDjikstra), new ServiceDjikstra());
                ServiceLocator.GetDictionary().Add(typeof(ServiceA), new ServiceA());
                ServiceLocator.GetDictionary().Add(typeof(ServiceAAuth), new ServiceAAuth());
                ServiceLocator.GetDictionary().Add(typeof(ServiceFord), new ServiceFord());
            }
            catch (Exception) { }
        }
    }
}
