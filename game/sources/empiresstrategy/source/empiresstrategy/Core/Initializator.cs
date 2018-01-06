using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Main;
using Core.PlayerData;
using Core.Interfaces;

namespace Core
{
    public class Initializator
    {
        public static void Init()
        {
            try
            {
                AgentService _IAgentService = new AgentService();
                ServiceLocator.GetDictionary().Add(typeof(IPrefabService), new PrefabService());
                ServiceLocator.GetDictionary().Add(typeof(TextureService), new TextureService());
                ServiceLocator.GetDictionary().Add(typeof(PlayerDataService), new PlayerDataService());
                ServiceLocator.GetDictionary().Add(typeof(SceneLoaderService), new SceneLoaderService());
                ServiceLocator.GetDictionary().Add(typeof(MatchDataService), new MatchDataService());
                ServiceLocator.GetDictionary().Add(typeof(PlayService), new PlayService());
                ServiceLocator.GetDictionary().Add(typeof(IAgentService), _IAgentService);
                ServiceLocator.GetDictionary().Add(typeof(AgentService), _IAgentService);
                ServiceLocator.GetDictionary().Add(typeof(CameraService), new CameraService());
            }
            catch (Exception) { }
        }
    }
}
