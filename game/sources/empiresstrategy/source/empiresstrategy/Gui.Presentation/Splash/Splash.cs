using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Core.PlayerData;
using Core.Main;
using Core.Interfaces;

namespace Gui.Presentation.Splash
{
    public static class Splash
    {
        public enum SplashMenus { None, Main, Play, MapEditor, Settings, About };

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

        #region Vars
        static public SplashMenus currentMenu = SplashMenus.Main;

        static private int boxX;
        static private int boxY;
        static private int width;
        static private int height;
        static private string menuTitle;

        static private int buttonX;
        static private int buttonY;

        #endregion

        #region Menus

        static public void Draw()
        {
            reCalcVars();
            Rect Box1 = new Rect(boxX, boxY, width, height);
            GUI.Box(Box1, menuTitle);
            GUI.BeginGroup(Box1);
            switch (currentMenu)
            {
                case SplashMenus.None:
                    break;

                case SplashMenus.Main:
                    mainMenu();
                    break;

                case SplashMenus.Play:
                    playMenu();
                    break;

                case SplashMenus.MapEditor:
                    mapEditorMenu();
                    break;

                case SplashMenus.Settings:
                    settingsMenu();
                    break;

                case SplashMenus.About:
                    aboutMenu();
                    break;
            }
            GUI.EndGroup();
        }


        static private void mainMenu()
        {
            if (addButton(Resources.Splash.ButtonPlay))
            {
                currentMenu = SplashMenus.Play;
            }
            //if (addButton(Resources.Splash.ButtonMapEditor))
            //{
            //    currentMenu = SplashMenus.MapEditor;
            //}
            //if (addButton(Resources.Splash.ButtonSettings))
            //{
            //    currentMenu = SplashMenus.Settings;
            //}
            if (addButton(Resources.Splash.ButtonsAbout))
            {
                currentMenu = SplashMenus.About;
            }
            if (addButtonBottom(Resources.Splash.ButtonsExit))
            {
                currentMenu = SplashMenus.None;
            }
        }

        static private void aboutMenu()
        {
            GUIStyle labelStyle = GUI.skin.GetStyle("Label");
            if (labelStyle != null)
            {
                labelStyle = new GUIStyle(labelStyle);
                labelStyle.alignment = TextAnchor.UpperCenter;
            }
            Rect position = new Rect(width / 2 - (width / 2 - 100 / 2), 25, width - 100, height - 100);
            GUI.Label(position, Resources.Splash.LabelAbout, labelStyle);
            if (addButtonBottom(Resources.Splash.ButtonsExit))
            {
                currentMenu = SplashMenus.Main;
            }
        }

        static private void settingsMenu()
        {
            if (addButtonBottom(Resources.Splash.ButtonsExit))
            {
                currentMenu = SplashMenus.Main;
            }
        }

        static private void mapEditorMenu()
        {
            if (addButton(Resources.Splash.ButtonNewMap))
            {
                throw new NotImplementedException();
            }
            if (addButton(Resources.Splash.ButtonLoadMap))
            {
                throw new NotImplementedException();
            }
            if (addButtonBottom(Resources.Splash.ButtonsExit))
            {
                currentMenu = SplashMenus.Main;
            }
        }

        static private void playMenu()
        {
            if (addButton(Resources.Splash.ButtonDevelopement))
            {
                MatchDataS.startDevelopementPlay();
            }
            if (addButton(Resources.Splash.ButtonSinglePlayer))
            {
                MatchDataS.startSinglePlayerPlay();
            }
            //if (addButton(Resources.Splash.ButtonMultiPlayer))
            //{
            //    MatchDataS.startMultiPlayerPlay();
            //}
            if (addButtonBottom(Resources.Splash.ButtonsExit))
            {
                currentMenu = SplashMenus.Main;
            }
        }


        #endregion

        #region Funkcje
        static private void reCalcVars()
        {
            buttonX = 0;
            buttonY = 0;
            boxX = Screen.width / 2 - 300;
            boxY = Screen.height / 2 - 200;
            width = 600;
            height = 400;
            switch (currentMenu)
            {
                case SplashMenus.None:
                    break;

                case SplashMenus.Main:
                    menuTitle = Resources.Splash.MenuTitleMain;
                    break;

                case SplashMenus.Play:
                    menuTitle = Resources.Splash.ButtonPlay;
                    break;

                case SplashMenus.MapEditor:
                    menuTitle = Resources.Splash.MenuTitleMapEditor;
                    break;

                case SplashMenus.Settings:
                    menuTitle = Resources.Splash.MenuTitleSettings;
                    break;

                case SplashMenus.About:
                    menuTitle = Resources.Splash.MenuTitleAbout;
                    break;
            }
        }

        static private bool addButtonBottom(string text)
        {
            buttonY = 0;
            buttonY += height - 50 - 10 - 25;
            return addButton(text);
        }

        static private bool addButton(string text)
        {
            buttonY += 10 + 25;
            Rect position = new Rect(width / 2 - 100, buttonY, 200, 25);
            return GUI.Button(position, text);
        }

        #endregion
    }
}
