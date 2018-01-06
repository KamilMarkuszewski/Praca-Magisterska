using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateMachine
{
    public enum poziom { Malo, Srednio, Duzo };
    public class FuzzyLogic
    {
        public int[] ProcentPrzynaleznosci;
        public int delta;

        public FuzzyLogic(int delta)
        {
            ProcentPrzynaleznosci = new int[(int)poziom.Duzo + 1];
            this.delta = delta;
        }
        private int CalcPerc(int val, int threshold)
        {
            return (Math.Abs(val - threshold + delta / 2) * 100 / delta);
        }
        public void CalcObj(int threshold1, int threshold2, int val)
        {
            if (val < threshold1 + delta / 2)
            {
                if (val < threshold1 - delta / 2) 
                    ProcentPrzynaleznosci[(int)poziom.Malo] = 100;
                else 
                    ProcentPrzynaleznosci[(int)poziom.Malo] = 100 - CalcPerc(val, threshold1);
            }
            if (val < threshold2 + delta / 2 && val >= threshold1 - delta / 2)
            {
                if (val < threshold2 - delta / 2 && val > threshold1 + delta / 2) 
                    ProcentPrzynaleznosci[(int)poziom.Srednio] = 100;
                else if (val >= threshold2 - delta / 2) 
                    ProcentPrzynaleznosci[(int)poziom.Srednio] = 100 - CalcPerc(val, threshold2);
                else if (val <= threshold1 + delta / 2) 
                    ProcentPrzynaleznosci[(int)poziom.Srednio] = CalcPerc(val, threshold1);
            }
            if (val >= threshold2 - delta / 2)
            {
                if (val > threshold2 + delta / 2) 
                    ProcentPrzynaleznosci[(int)poziom.Duzo] = 100;
                else 
                    ProcentPrzynaleznosci[(int)poziom.Duzo] = CalcPerc(val, threshold2);
            }
        }

        override public string ToString()
        {
            return "Malo: " + ProcentPrzynaleznosci[(int)poziom.Malo] +
                    "Srednio: " + ProcentPrzynaleznosci[(int)poziom.Srednio] +
                    "Duzo: " + ProcentPrzynaleznosci[(int)poziom.Duzo];
        }
    }
}
