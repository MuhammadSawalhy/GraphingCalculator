//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MathPackage.SuperCalculator
//{

//    public class Number_ : object
//    {

//        public List<string> digits = new List<string>();

//        private bool Negate = false;

//        public Number_()
//        {
//        }

//        public Number_(string num)
//        {
//            if (Main.IsNumeric(num) && !num.Contains("."))
//            {
//                if (num.Contains("-"))
//                    Negate = true;

//                string str = num.Substring(1, num.Length - 1);
//                for (int i = num.Length - 1; i >= 0; i--)
//                    digits.Add(num[i].ToString());
//            }
//        }

//        public override string ToString()
//        {
//            string str = "";
//            for (int i = digits.Count - 1; i >= 0; i += -1)
//                str += digits[i];
//            return str;
//        }

//    }

//}