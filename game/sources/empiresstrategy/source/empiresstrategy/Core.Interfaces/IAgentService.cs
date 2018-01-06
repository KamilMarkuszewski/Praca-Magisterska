using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IAgentService
    {
        void setDestination(Vector2 destination, IMyGameObject a);

        void Init();


    }
}
