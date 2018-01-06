using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Core.PlayerData;
using Map.Data;
using Core.Main;
using Core;
using Map.Data.Interfaces;
using Core.Interfaces;
using Graphs.PathFinder;
using Core.Entities;

namespace Gui.Presentation.GamePlay
{
    public static class GamePlay
    {
        #region Services

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

        private static CameraService _cameraService;
        public static CameraService cameraService
        {
            get
            {
                if (_cameraService == null) _cameraService = ServiceLocator.GetService<CameraService>();
                return _cameraService;
            }
        }

        private static TextureService _textureService;
        public static TextureService textureService
        {
            get
            {
                if (_textureService == null) _textureService = ServiceLocator.GetService<TextureService>();
                return _textureService;
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

        private static Match _match;
        public static Match myMatch
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

        public enum MenuOptions { None = 0, Tester, Options, Building, Map };

        #region Vars
        private static Rect miniCamRect;
        private static int widthRight;
        private static bool menuOn = false;
        private static MenuOptions menuCurrentOption = MenuOptions.None;


        #endregion
        public static void Start()
        {
            Debug.Log("Start");
            miniCamRect = new Rect(Screen.width - 210, 10, 200, 200);
            if (cameraService.mapCam != null)
            {
                cameraService.mapCam.pixelRect = miniCamRect;
                cameraService.mapCam.enabled = true;
            }
            System.IO.Directory.CreateDirectory(@"pomiar");
            System.IO.Directory.CreateDirectory(@"pomiar/pomiary");
        }


        public static void Draw()
        {
            try
            {
                foreach (var pl in _matchDataService.myMatch.mapData.players)
                {
                    Player p = pl.Value;
                    if (p.controledBySI && p.won)
                    {
                        GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "PRZEGRALES!");
                    }
                    if (!p.controledBySI && p.won)
                    {
                        GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "WYGRALES!");
                    }
                }
            }
            catch (Exception) { }
            cameraService.onScroll();
            DrawMimimapTxt();
            DrawTop();
            if (menuOn)
            {
                Rect Box2 = new Rect(Screen.width - widthRight, 30, widthRight, Screen.height - 30 - 220);
                GUI.Box(Box2, "");
                GUI.BeginGroup(Box2);

                switch (menuCurrentOption)
                {
                    case MenuOptions.None:
                        {
                            if (GUI.Button(new Rect(10, 10, 200, 25), Resources.GamePlay.Building))
                            {
                                menuCurrentOption = MenuOptions.Building;
                            }
                            if (Match.matchType == Match.MatchType.Developement)
                            {
                                if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10, 200, 25), Resources.GamePlay.Map))
                                {
                                    menuCurrentOption = MenuOptions.Map;
                                }
                                if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10 + 25 + 10, 200, 25), Resources.GamePlay.Tester))
                                {
                                    menuCurrentOption = MenuOptions.Tester;
                                }
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10 + 25 + 10 + 25 + 10, 200, 25), Resources.GamePlay.Options))
                            {
                                menuCurrentOption = MenuOptions.Options;
                            }

                            break;
                        }
                    case MenuOptions.Tester:
                        {
                            if (GUI.Button(new Rect(10, 10, 200, 25), "Generowanie/wielkosc - pomiary"))
                            {
                                TestHelper.generateTest(MatchDataS.myMatch.mapData);
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10, 200, 25), "Znajd drogi/wielkosc - pomiary"))
                            {
                                TestHelper.FindWaySizeTest(MatchDataS.myMatch.mapData);
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10, 200, 25), "Znajd drogi/odleglosc - pomiary"))
                            {
                                TestHelper.FindWayDistTest(MatchDataS.myMatch.mapData);
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10 + 25 + 10, 200, 25), "Generowanie A*Auth map"))
                            {
                                TestHelper.generateTestCzworki(MatchDataS.myMatch.mapData);
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10 + 25 + 10 + 25 + 10, 200, 25), "A*Auth i A* czas"))
                            {
                                TestHelper.FindWayDistTestCzworki(MatchDataS.myMatch.mapData);
                            }

                            break;
                        }
                    case MenuOptions.Map:
                        {
                            if (GUI.Button(new Rect(10, 10, 200, 25), "Losuj przeszkody"))
                            {
                                MatchDataS.myMatch.mapData.randomFill(10);
                                agentService.freeze = false;
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10, 200, 25), "Zapisz do pliku"))
                            {
                                MatchDataS.myMatch.mapData.SaveMap();
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10, 200, 25), "Mapa 1"))
                            {
                                string[] lines = System.IO.File.ReadAllLines(@"map1.txt");
                                MatchDataS.myMatch.mapData.ReadMap(lines);
                                agentService.freeze = false;
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10 + 25 + 10, 200, 25), "Mapa 2"))
                            {
                                string[] lines = System.IO.File.ReadAllLines(@"map2.txt");
                                MatchDataS.myMatch.mapData.ReadMap(lines);
                                agentService.freeze = false;
                            }
                            break;
                        }
                    case MenuOptions.Options:
                        {
                            if (GUI.Button(new Rect(10, 10, 200, 25), "Uzywany: " + PathFinderService.PathFinderMethodString[(int)PlayerDataService.mySettings.pathFinderMethod]))
                            {
                                PlayerDataService.mySettings.pathFinderMethod = PlayerDataService.mySettings.pathFinderMethod + 1;
                                if (PlayerDataService.mySettings.pathFinderMethod > PathFinderMethod.AModified2) PlayerDataService.mySettings.pathFinderMethod = PathFinderMethod.None + 1;
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10, 200, 25), "Mini mapa "))
                            {
                                cameraService.mapCam.enabled = !cameraService.mapCam.enabled;
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10, 200, 25), "Pokazuj punkty " + PlayerDataService.mySettings.showPoints))
                            {
                                PlayerDataService.mySettings.showPoints = !PlayerDataService.mySettings.showPoints;
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10 + 25 + 10, 200, 25), "Przeciwnik: " + (PlayerDataService.mySettings.isFighterSI == true ? "Wojownik" : "Budowniczy")))
                            {
                                PlayerDataService.mySettings.isFighterSI = !PlayerDataService.mySettings.isFighterSI;
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10 + 25 + 10 + 25 + 10, 200, 25), "Fuzzy Logic: " + PlayerDataService.mySettings.FuzzyLogic))
                            {
                                PlayerDataService.mySettings.FuzzyLogic = !PlayerDataService.mySettings.FuzzyLogic;
                            }

                            break;
                        }
                    case MenuOptions.Building:
                        {
                            Player player = MatchDataS.myMatch.mapData.GetCurrentPlayer();
                            Building a = player.baseBuilding;

                            if (GUI.Button(new Rect(10, 10, 200, 25), "Stworz mieszkanca [50 j]"))
                            {
                                if (player.food >= 50)
                                {
                                    agentService.CreateNewVilager(player);
                                }
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10, 200, 25), "Przyspiesz zbieranie [50 z]"))
                            {
                                player.buySpeed();
                            }
                            if (GUI.Button(new Rect(10, 10 + 25 + 10 + 25 + 10, 200, 25), "Zwiększ koszyki [20 z, 50 d]"))
                            {
                                player.buySize();
                            }
                            break;
                        }
                }
                GUI.EndGroup();
            }
            else
            {
                DrawRight();
            }
        }

        private static void DrawRight()
        {
            widthRight = 220;
            Rect Box1 = new Rect(Screen.width - widthRight, 30, widthRight, Screen.height - 30 - 220);
            GUI.Box(Box1, "");
            GUI.BeginGroup(Box1);
            Player player = MatchDataS.myMatch.mapData.GetCurrentPlayer();

            if (player.selectedAgents.Count > 1)
            {

            }
            else if (player.selectedAgents.Count == 1)
            {
                Agent a = player.selectedAgents.FirstOrDefault();
                GUI.Label(new Rect(10, 10, 200, 25), "Predkosc zbierania: " + a.collectSpeed);
                GUI.Label(new Rect(10, 10 + 25 + 10, 200, 25), "Uniesie: " + a.collectMax);
                GUI.Label(new Rect(10, 10 + 25 + 10 + 25 + 10, 200, 25), "Zebral jedzenia: " + a.food);
                GUI.Label(new Rect(10, 10 + 25 + 10 + 25 + 10 + 25 + 10, 200, 25), "Zebral zlota: " + a.gold);
                GUI.Label(new Rect(10, 10 + 25 + 10 + 25 + 10 + 25 + 10 + 25 + 10, 200, 25), "Zebral drewna: " + a.wood);
                GUI.Label(new Rect(10, 10 + 25 + 10 + 25 + 10 + 25 + 10 + 25 + 10 + 25 + 10, 200, 25), "Cel: " + a.currentGoal.ToString());
            }


            GUI.EndGroup();
        }

        private static void DrawTop()
        {
            Rect Box1 = new Rect(0, -5, Screen.width, 35);
            GUI.Box(Box1, "");
            GUI.BeginGroup(Box1);
            Player player = MatchDataS.myMatch.mapData.GetCurrentPlayer();
            Player op = MatchDataS.myMatch.mapData.players.Where(f => f.Value.number != player.number).ToList().First().Value;
            GUI.Label(new Rect(5, 10, 100, 25), Resources.GamePlay.Wood + ": " + player.wood);
            GUI.Label(new Rect(105, 10, 100, 25), Resources.GamePlay.Food + ": " + player.food);
            GUI.Label(new Rect(205, 10, 100, 25), Resources.GamePlay.Gold + ": " + player.gold);

            if (player.agents != null) GUI.Label(new Rect(405, 10, 150, 25), "G agenci" + ": " + player.agents.Count);
            if (op.agents != null) GUI.Label(new Rect(505, 10, 100, 25), "P agenci" + ": " + op.agents.Count);

            GUI.Label(new Rect(605, 10, 200, 25), "Czas" + ": " + (Time.timeSinceLevelLoad));

            if (GUI.Button(new Rect(Screen.width - 80 - 5, 7, 80, 25), "Menu"))
            {
                menuOn = !menuOn;
                menuCurrentOption = MenuOptions.None;
            }
            GUI.EndGroup();
        }

        private static void DrawMimimapTxt()
        {
            GUIStyle style = new GUIStyle();
            Texture2D txt = textureService.GetTexture(TextureService.TextureNames.GuiMiniMap);
            style.normal.background = txt;
            Rect Box1 = new Rect(Screen.width - widthRight, Screen.height - 220, widthRight, 220);
            GUI.Box(Box1, "", style);
        }

    }
}
