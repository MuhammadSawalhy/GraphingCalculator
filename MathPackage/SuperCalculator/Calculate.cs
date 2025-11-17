//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MathPackage.SuperCalculator
//{
//    public static class Calculate
//    {

//        public static Number_ CalculatePower(Number_ num1, int exponent)
//        {
//            Number_ num = new Number_(num1.ToString());
//            for (int i = 2; i <= exponent; i++)
//            {
//                num = CalculateMult(num, num1);
//            }
//            return num;
//        }

//        public static Number_ CalculateMult(Number_ num1, Number_ num2)
//        {
//            List<Number_> AddList = new List<Number_>();
//            string Zeros = ",";
//            Number_ num = new Number_();
//            foreach (string i in num1.digits)
//            {
//                num = new Number_();
//                if (Zeros != ",") num.digits.AddRange(Zeros.Substring(1, Zeros.Length - 2).Split(',')); 
//                Zeros += "0,";
//                num.digits.AddRange(Mult(int.Parse(i), num2).digits.ToArray());
//                AddList.Add(num);
//            }
//            num = AddList[0];
//            for (int i = 1;i< AddList.Count;i++)
//            {
//                num = CalculateSum(num, AddList[i]);
//            }
//            return num;
//        }

//        private static Number_ Mult(int num, Number_ num1)
//        {
//            Number_ num_ = new Number_();
//            string remain = "0";
//            string process = "";
//            foreach (string i in num1.digits)
//            {
//                process = (num * int.Parse(i) + int.Parse(remain)).ToString();
//                if (process.Length == 1)
//                    process = "0" + process;
//                num_.digits.Add(process.Substring(1, 1));
//                remain = process.Substring(0, 1);
//            }
//            if (remain != "0") num_.digits.Add(remain);
//            return num_;
//        }

//        public static Number_ CalculateSum(Number_ num1, Number_ num2)
//        {
//            string remain = "0";
//            Number_ sum = new Number_();
//            string process;
//            if (num1.digits.Count > num2.digits.Count)
//            {
//                for (int i = 0; i <= num2.digits.Count - 1; i++)
//                {
//                    process = (int.Parse(num2.digits[i]) + int.Parse(num1.digits[i]) + int.Parse(remain)).ToString();
//                    if (process.Length == 1)
//                        process = "0" + process;
//                    sum.digits.Add(process.Substring(1, 1));
//                    remain = process.Substring(0, 1);
//                }
//                for (int i = num2.digits.Count; i <= num1.digits.Count - 1; i++)
//                {
//                    process = ( int.Parse(num1.digits[i]) + int.Parse(remain)).ToString();
//                    if (process.Length == 1)
//                        process = "0" + process;
//                    sum.digits.Add(process.Substring(1, 1));
//                    remain = process.Substring(0, 1);
//                }
//                if (remain != "0") sum.digits.Add(remain);
//            }
//            else
//            {
//                for (int i = 0; i <= num1.digits.Count - 1; i++)
//                {
//                    process = (int.Parse(num2.digits[i]) + int.Parse(num1.digits[i]) + int.Parse(remain)).ToString();
//                    if (process.Length == 1)
//                        process = "0" + process;
//                    sum.digits.Add(process.Substring(1, 1));
//                    remain = process.Substring(0, 1);
//                }
//                for (int i = num1.digits.Count; i <= num2.digits.Count - 1; i++)
//                {
//                    process = (int.Parse(num2.digits[i]) + int.Parse(remain)).ToString();
//                    if (process.Length == 1)
//                        process = "0" + process;
//                    sum.digits.Add(process.Substring(1, 1));
//                    remain = process.Substring(0, 1);
//                }
//                if (remain != "0") sum.digits.Add(remain);
//            }
//            return sum;
//        }

//    }
//}
