using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gui.Presentation.Splash;
using Core.Main;
using Gui.Presentation.CreateMatch;
using Gui.Presentation.GamePlay;
using Core.Interfaces;


namespace Gui
{
    public class GuiService
    {
        #region Services


        private SceneLoaderService _sceneLoader;
        public SceneLoaderService sceneLoader
        {
            get
            {
                if (_sceneLoader == null) _sceneLoader = ServiceLocator.GetService<SceneLoaderService>();
                return _sceneLoader;
            }
            set
            {
                _sceneLoader = value;
            }
        }

        #endregion
        public void Start()
        {
            try
            {

                if (sceneLoader.current == SceneLoaderService.Scene.scPlay)
                {
                    GamePlay.Start();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.catchException(e);
            }
        }


        public void onGUI()
        {
            try
            {
                switch (sceneLoader.current)
                {
                    case SceneLoaderService.Scene.scGameMenu:
                        // Scene0 - Splash
                        Splash.Draw();
                        break;
                    case SceneLoaderService.Scene.scCreateMatch:
                        // Scene1 - Wybor Mapy
                        CreateMatch.Draw();
                        break;
                    case SceneLoaderService.Scene.scPlay:
                        // Scene3 - Play
                        GamePlay.Draw();
                        break;
                }



                ExceptionHandler.drawErrorWindow();
            }
            catch (Exception e)
            {
                ExceptionHandler.catchException(e);
            }
        }


    }
}
