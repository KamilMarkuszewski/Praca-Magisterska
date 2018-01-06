using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities;
using Core.Main;
using UnityEngine;
using Core.Interfaces;

namespace Core.PlayerData
{
    public class MatchDataService
    {
        #region Services

        private static PlayService _playService;
        public static PlayService playService
        {
            get
            {
                if (_playService == null) _playService = ServiceLocator.GetService<PlayService>();
                return _playService;
            }
        }

        private static SceneLoaderService _sceneLoader;
        public static SceneLoaderService sceneLoader
        {
            get
            {
                if (_sceneLoader == null) _sceneLoader = ServiceLocator.GetService<SceneLoaderService>();
                return _sceneLoader;
            }
        }

        private static IAgentService _agentService;
        public static IAgentService agentService
        {
            get
            {
                if (_agentService == null) _agentService = ServiceLocator.GetService<IAgentService>();
                return _agentService;
            }
            set
            {
                _agentService = value;
            }
        }

        #endregion

        #region Properties

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

        #endregion

        #region functions

        public void AfterLoaded()
        {
            playService.Start();
            agentService.Init();
        }

        public void Run()
        {
            Debug.Log(sceneLoader.ToString());
            sceneLoader.changeScene(SceneLoaderService.Scene.scPlay);

        }

        public void startDevelopementPlay()
        {
            Match.matchType = Match.MatchType.Developement;
            myMatch.matchNetworkType = Match.MatchNetworkType.Offline;
            sceneLoader.changeScene(SceneLoaderService.Scene.scCreateMatch);
        }

        public void startSinglePlayerPlay()
        {
            Match.matchType = Match.MatchType.SinglePlayer;
            myMatch.matchNetworkType = Match.MatchNetworkType.Offline;
            sceneLoader.changeScene(SceneLoaderService.Scene.scCreateMatch);
        }

        public void startMultiPlayerPlay()
        {
            // throw new NotImplementedException();
        }

        #endregion
    }
}
