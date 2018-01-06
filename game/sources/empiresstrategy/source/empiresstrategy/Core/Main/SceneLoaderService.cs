using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Main
{
    public class SceneLoaderService
    {
        public enum Scene { None, scGameMenu, scAuth, scCreateMatch, scPlay, scPoints, scMapEditor };
        public Scene current = Scene.scGameMenu;

        private string getSceneName(Scene sc)
        {
            switch (sc)
            {
                case Scene.scGameMenu:
                    return "scGameMenu";

                case Scene.scAuth:
                    return "scAuth";

                case Scene.scCreateMatch:
                    return "scCreateMatch";

                case Scene.scPlay:
                    return "scPlay";

                case Scene.scPoints:
                    return "scPoints";

                case Scene.scMapEditor:
                    return "scMapEditor";

                default:
                    throw new Core.Main.ExceptionHandler.WrongSceneException();
            }
        }

        public void changeScene(Scene sc)
        {
            current = sc;
            Application.LoadLevel(getSceneName(sc));
        }




    }
}
