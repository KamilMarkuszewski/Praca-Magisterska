using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Main;
using Core.Interfaces;

namespace Gui
{
    public class Initializator
    {
        public static void Init()
        {
            try
            {


                ServiceLocator.GetDictionary().Add(typeof(GuiService), new GuiService());
            }
            catch (Exception) { }
        }
    }
}
