using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interfaces
{
    public enum ObjectType { None, Agent, Building, Rock, Food, Gold, Wood };
    public interface IMyGameObject
    {
        ObjectType objectType { get; }


    }
}
