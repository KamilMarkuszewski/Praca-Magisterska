using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Interfaces
{
    public enum PrefabNames
    {
        None,
        Vilager,
        VilagerRed,
        Rock,
        RealRock,
        Point, 
        Building, 
        Food,
        Gold,
        Wood,
        PointS

    };

    public interface IPrefabService
    {

         Transform GetPrefab(PrefabNames name);
         void Init(IDictionary<PrefabNames, Transform> textures);
    }
}
