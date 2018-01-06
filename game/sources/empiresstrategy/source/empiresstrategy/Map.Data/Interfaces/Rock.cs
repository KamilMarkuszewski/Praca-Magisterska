using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interfaces;
using UnityEngine;

namespace Map.Data.Interfaces
{
    class Rock : IMyGameObject
    {


        static int maxId = 1;
        public UnityEngine.GameObject objectUnity = null;
        public UnityEngine.Transform transform = null;
        public int x;
        public int y;
        public int id;

        public Rock(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.id = maxId;
            maxId++;
        }

        public ObjectType objectType
        {
            get
            {
                return ObjectType.Rock;
            }
        }
    }
}
