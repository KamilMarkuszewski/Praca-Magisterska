using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Core.Interfaces;

namespace Core
{
    public class PrefabService : IPrefabService
    {
       public IDictionary<PrefabNames, Transform> prefabs;


        public void Init(IDictionary<PrefabNames, Transform> textures)
        {
            this.prefabs = textures;
        }


        public Transform GetPrefab(PrefabNames name)
        {
            Transform txt;
            try
            {
                prefabs.TryGetValue(name, out txt);
            }
            catch (Exception)
            {
                return null;
            }
            return txt;
        }

    }
}
