using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities;
using UnityEngine;
using Core.PlayerData;
using Core.Interfaces;

namespace Core
{
    public class PlayService
    {
        #region Service

        private Match _match;
        public Match myMatch
        {
            get
            {
                if (_match == null) _match = new Match();
                return _match;
            }
            set
            {
                _match = value;
            }
        }
        private static MatchDataService _matchDataService;
        public static MatchDataService MatchDataS
        {
            get
            {
                if (_matchDataService == null) _matchDataService = ServiceLocator.GetService<MatchDataService>();
                return _matchDataService;
            }
        }

        private static PlayerDataService _PlayerDataService;
        public static PlayerDataService PlayerDataService
        {
            get
            {
                if (_PlayerDataService == null) _PlayerDataService = ServiceLocator.GetService<PlayerDataService>();
                return _PlayerDataService;
            }
        }
        private static AgentService _agentService;
        public static AgentService agentService
        {
            get
            {
                if (_agentService == null) _agentService = ServiceLocator.GetService<AgentService>();
                return _agentService;
            }
            set
            {
                _agentService = value;
            }
        }

        #endregion

        public bool started = false;


        public void Start()
        {
            int size = myMatch.mapData.size;

            started = true;
            if (Match.matchType == Match.MatchType.SinglePlayer)
            {
                string[] lines = System.IO.File.ReadAllLines(@"map2.txt");
                MatchDataS.myMatch.mapData.ReadMap(lines);
                agentService.freeze = false;
            }
        }
    }
}
