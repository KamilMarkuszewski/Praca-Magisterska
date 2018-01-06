using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.Data.Interfaces;
using Map.Data;

namespace StateMachineInterfaces
{
    public interface IAgentSI
    {
        public void UpdateAgentSI(MapData map, Agent agent);

    }
}
