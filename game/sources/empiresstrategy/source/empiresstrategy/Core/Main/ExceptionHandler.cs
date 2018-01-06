using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

namespace Core.Main
{
    public class ExceptionHandler
    {

        public static void catchException(Exception exc)
        {
            saveExceptionToFile(exc);
            drawWindow = true;
            windowMessage = exc.Message;
        }

        private static void saveExceptionToFile(Exception exc)
        {
            try
            {
                System.IO.Directory.CreateDirectory(@"Logs");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Logs\Exceptions.txt", true))
                {
                    file.WriteLine("");
                    file.WriteLine("----- ----- ----- ----- ----- ----- ----- ----- ----- -----");
                    file.WriteLine("");

                    file.WriteLine(exc.ToString());

                    file.WriteLine("");
                }
            }
            catch (Exception) { }
        }

        #region drawErrorWindow

        public static bool drawWindow = false;
        public static string windowMessage = "";

        public static void drawErrorWindow()
        {
            ExceptionHandler exceptionHandler = new ExceptionHandler();
            if (drawWindow)
            {
                GUI.Window(0, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 200), exceptionHandler.drawErrorWindowContent, Resources.Core.Error);
            }
        }

        private void drawErrorWindowContent(int windowID)
        {
            GUIStyle style = GUI.skin.GetStyle("Label");
            if (style != null)
            {
                style = new GUIStyle(style);
                style.alignment = TextAnchor.MiddleCenter;
            }
            GUI.Label(new Rect(10, 20, 380, 140), windowMessage, style);
            if (GUI.Button(new Rect(400 / 2 - 40, 160, 80, 20), Resources.Core.Close))
            {
                drawWindow = false;
                windowMessage = "";
            }
        }

        #endregion

        #region MyExceptions


        public class WrongSceneException : ApplicationException
        {

        }


        #endregion

    }
}
