using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.Data.Interfaces;
using StateMachine;
using UnityEngine;

namespace Map.Data.Interfaces
{
    public class Player
    {

        public enum PlayerNumberEnum { None, Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8 };

        public PlayerNumberEnum number;
        public Dictionary<int, Agent> agents = new Dictionary<int, Agent>();
        public Building baseBuilding;
        public bool AI;
        public int id;

        public bool won = false;

        #region zasoby

        public int food;
        public int wood;
        public int gold;

        public poziom FoodLevel
        {
            get
            {
                if (food < 50) _foodLevel = poziom.Malo;
                else if (food < 500) _foodLevel = poziom.Srednio;
                else _foodLevel = poziom.Duzo;
                return _foodLevel;
            }
        }

        public poziom GoldLevel
        {
            get
            {
                if (gold < 50) _goldLevel = poziom.Malo;
                else if (gold < 500) _goldLevel = poziom.Srednio;
                else _goldLevel = poziom.Duzo;
                return _goldLevel;
            }
        }

        public poziom WoodLevel
        {
            get
            {
                if (wood < 50) _woodLevel = poziom.Malo;
                else if (wood < 500) _woodLevel = poziom.Srednio;
                else _woodLevel = poziom.Duzo;
                return _woodLevel;
            }
        }

        public FuzzyLogic FoodLevelFuzzy
        {
            get
            {
                FuzzyLogic ret = new FuzzyLogic(20);
                int val = food;
                ret.CalcObj(50, 500, val);
                return ret;
            }
        }

        public FuzzyLogic GoldLevelFuzzy
        {
            get
            {
                FuzzyLogic ret = new FuzzyLogic(20);
                int val = gold;
                ret.CalcObj(50, 500, val);
                return ret;
            }
        }

        public FuzzyLogic WoodLevelFuzzy
        {
            get
            {
                FuzzyLogic ret = new FuzzyLogic(20);
                int val = wood;
                ret.CalcObj(50, 500, val);
                return ret;
            }
        }

        public poziom _foodLevel = poziom.Malo;
        public poziom _woodLevel = poziom.Malo;
        public poziom _goldLevel = poziom.Malo;

        #endregion


        public int startXB;
        public int startYB;

        public bool controledBySI = true;


        public Agent.Goal currentGoal = Agent.Goal.None;


        private IList<Agent> _selectedAgents;
        public IList<Agent> selectedAgents
        {
            get
            {
                if (_selectedAgents == null) _selectedAgents = new List<Agent>();
                return _selectedAgents;
            }
        }

        private IList<Building> _selectedBuilding;
        public IList<Building> selectedBuilding
        {
            get
            {
                if (_selectedBuilding == null) _selectedBuilding = new List<Building>();
                return _selectedBuilding;
            }
        }

        public Player(PlayerNumberEnum number, bool ai, int startX, int startY)
        {
            this.number = number;
            this.AI = ai;
            id = (int)number;
            Agent firstAgent = new Agent(startX, startY, this);
            firstAgent.type = Agent.Type.Vilager;
            agents.Add(firstAgent.id, firstAgent);
            startXB = startX + 2;
            startYB = startY + 2;
        }

        public int collectSpeed = 3;
        public int collectMax = 20;


        public void buySize()
        {
            if (gold >= 20 && wood >= 50)
            {
                gold -= 20;
                wood -= 50;
                collectMax += 10;
            }
        }

        public void buySpeed()
        {
            if (gold >= 50)
            {
                gold -= 50;
                collectSpeed += 3;
            }
        }


    }
}
