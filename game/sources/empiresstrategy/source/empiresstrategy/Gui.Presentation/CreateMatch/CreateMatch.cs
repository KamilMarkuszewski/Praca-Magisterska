using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.PlayerData;
using Core.Main;
using UnityEngine;
using Map.Data;
using Graphs;
using Map.Data.Interfaces;
using Core.Interfaces;

namespace Gui.Presentation.CreateMatch
{
    public static class CreateMatch
    {
        public enum CreateMatchMenus { None, ChoseMap };

        #region Services

        private static MatchDataService _matchDataService;
        public static MatchDataService MatchDataS
        {
            get
            {
                if (_matchDataService == null) _matchDataService = ServiceLocator.GetService<MatchDataService>();
                return _matchDataService;
            }
            set
            {
                _matchDataService = value;
            }
        }


        #endregion

        public static void Draw()
        {
            Rect position = new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 25);
            if (GUI.Button(position, Resources.Splash.ButtonPlay))
            {
                MatchDataS.myMatch.mapData.mapName = "Developement map";
                MatchDataS.myMatch.mapData.size = 50;
                MatchDataS.myMatch.mapData.players = new Dictionary<int, Player>();
                Player p1 = new Player(Player.PlayerNumberEnum.Player1, false, 23, 23);
                Player p2 = new Player(Player.PlayerNumberEnum.Player2, false, -23, -23);
                p1.controledBySI = false;

                MatchDataS.myMatch.mapData.players.Add((int)Player.PlayerNumberEnum.Player1, p1);
                MatchDataS.myMatch.mapData.players.Add((int)Player.PlayerNumberEnum.Player2, p2);
                MatchDataS.Run();
            }
        }


    }
}
