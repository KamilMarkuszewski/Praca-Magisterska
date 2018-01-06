using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphs.PathFinder;

namespace Core.Entities
{
    public class Settings
    {
        public float keysScrollSpeed = 0.5f;
        public float mouseScrollSpeed = 0.5f;

        public PathFinderMethod pathFinderMethod = PathFinderMethod.AModified2;

        public bool showPoints = false;
        public bool isFighterSI = true;
        public bool FuzzyLogic = true;
        public bool Stado = false;
    }
}
