using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interfaces;

namespace Map.Data.Interfaces
{
    public class Building : IMyGameObject
    {

        static int maxId = 1;
        public UnityEngine.Transform transform = null;
        public int x;
        public int y;
        public int id;

        public int zniszczony = 0;

        public Map.Data.Interfaces.Player.PlayerNumberEnum owner;
        public UnityEngine.GameObject objectUnity;

        public Building(int x, int y)
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
                return ObjectType.Building;
            }
        }
    }
}
