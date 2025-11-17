using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Drawing;

namespace Graphing.Objects
{

    public class Coordinates 
    {
        private PointF Center_;

        protected CalcRange XRange_ { get; set; }
        protected CalcRange YRange_ { get; set; }

        public Bitmap Bitmap;
        public Graphics Graphics;
        GraphSetting GraphSetting;

        public Coordinates(Coordinates Coor, GraphSetting GraphSetting_)
        {
            GraphSetting = GraphSetting_;
            CoorSetting = new CoorSetting(GraphSetting);
            Center_ = Coor.Center_;
            CoorSetting = Coor.CoorSetting;
        }

        public Coordinates(GraphSetting GraphSetting_)
        {
            GraphSetting = GraphSetting_;
            CoorSetting = new CoorSetting(GraphSetting);
        }

        public CoorSetting CoorSetting;

        public void Draw()
        {
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.SmoothingMode = MainFunctions.Smoothing;
            Graphics.CompositingQuality = MainFunctions.Composition;

            Graphics.Clear(CoorSetting.BackColor);

            Center_ = GraphSetting.Center;
            ChangeDomains_();

            switch (CoorSetting.Type)
            {
                case CoorSetting.CoorType.Custom:
                    {
                       Coordinate();
                        break;
                    }

                case CoorSetting.CoorType.Radian:
                    {
                        break;
                    }
            }
        }

        public void Draw(PointF center)
        {
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.SmoothingMode = MainFunctions.Smoothing;
            Graphics.CompositingQuality = MainFunctions.Composition;
            Graphics.Clear(CoorSetting.BackColor);

            Center_ = center;
            ChangeDomains_();

            switch (CoorSetting.Type)
            {
                case CoorSetting.CoorType.Custom:
                    {
                     Coordinate();
                        break;
                    }

                case CoorSetting.CoorType.Radian:
                    {
                        break;
                    }
            }
        }

        public virtual void DrawTo(Graphics g, int x, int y)
        {
            g.DrawImage(Bitmap, x, y);
        }
        public virtual void DrawTo(Graphics g)
        {
            g.DrawImage(Bitmap, 0, 0);
        }

        public float xChangeToPixel_(double xCoorValue, double yCoorValue)
        {
            return (float)(Center_.X + (xCoorValue * GraphSetting.X_Stretch) * Math.Cos(GraphSetting.XAngle) + (yCoorValue * GraphSetting.Y_Stretch) * Math.Cos(GraphSetting.YAngle));
        }

        public float yChangeToPixel_(double xCoorValue, double yCoorValue)
        {
            return (float)(Center_.Y - ((yCoorValue * GraphSetting.Y_Stretch) * Math.Sin(GraphSetting.YAngle) + (xCoorValue * GraphSetting.X_Stretch) * Math.Sin(GraphSetting.XAngle)));
        }

        public double xChangeToCoor_(float xPixelValue, float yPixelValue)
        {
            xPixelValue = xPixelValue - Center_.X;
            yPixelValue = Center_.Y - yPixelValue;
            return ((Math.Cos(GraphSetting.YAngle) * yPixelValue - Math.Sin(GraphSetting.YAngle) * xPixelValue) / Math.Sin(GraphSetting.XAngle - GraphSetting.YAngle)) / GraphSetting.X_Stretch;
        }

        public double yChangeToCoor_(float xPixelValue, float yPixelValue)
        {
            xPixelValue = xPixelValue - Center_.X;
            yPixelValue = Center_.Y - yPixelValue;
            return ((Math.Cos(GraphSetting.XAngle) * yPixelValue - Math.Sin(GraphSetting.XAngle) * xPixelValue) / Math.Sin(GraphSetting.YAngle - GraphSetting.XAngle)) / GraphSetting.Y_Stretch;
        }

        private void ChangeDomains_()
        {
            XRange_ = new CalcRange(xChangeToCoor_(0, Math.Tan(GraphSetting.YAngle) > 0 ? 0 : GraphSetting.Height), xChangeToCoor_(GraphSetting.Width, Math.Tan(GraphSetting.YAngle) < 0 ? 0 : GraphSetting.Height), GraphSetting.Width);
            YRange_ = new CalcRange(yChangeToCoor_(Math.Tan(GraphSetting.XAngle) > 0 ? GraphSetting.Width : 0, GraphSetting.Height), yChangeToCoor_(Math.Tan(GraphSetting.XAngle) < 0 ? GraphSetting.Width : 0, 0), GraphSetting.Height);
        }

        public void Coordinate()
        {
            double start_x, start_y, end_x, end_y;
            start_x = Math.Floor(XRange_.Lo / GraphSetting.X_SpaceValue) * GraphSetting.X_SpaceValue;
            end_x = Math.Ceiling(XRange_.Hi / GraphSetting.X_SpaceValue) * GraphSetting.X_SpaceValue;
            start_y = Math.Floor(YRange_.Lo / GraphSetting.Y_SpaceValue) * GraphSetting.Y_SpaceValue;
            end_y = Math.Ceiling(YRange_.Hi / GraphSetting.Y_SpaceValue) * GraphSetting.Y_SpaceValue;

            if (CoorSetting.DrawDecimal)
            {
                // x
                for (double i = start_x; i <= end_x; i +=  GraphSetting.X_SpaceValue / CoorSetting.Decimal_space_x)
                    Graphics.DrawLine(CoorSetting.Decimal_Pen, new PointF(xChangeToPixel_(i, start_y), yChangeToPixel_(i, start_y)), new PointF(xChangeToPixel_(i, end_y), yChangeToPixel_(i, end_y)));
                // y
                for (double i = start_y; i <= end_y; i += GraphSetting.Y_SpaceValue / CoorSetting.Decimal_space_y)
                    Graphics.DrawLine(CoorSetting.Decimal_Pen, new PointF(xChangeToPixel_(start_x, i), yChangeToPixel_(start_x, i)), new PointF(xChangeToPixel_(end_x, i), yChangeToPixel_(end_x, i)));
            }

            if (CoorSetting.DrawCoor)
            {
                // x
                for (double i = start_x; i <= end_x; i += GraphSetting.X_SpaceValue)
                    Graphics.DrawLine(CoorSetting.Coor_pen, new PointF(xChangeToPixel_(i, start_y), yChangeToPixel_(i, start_y)), new PointF(xChangeToPixel_(i, end_y), yChangeToPixel_(i, end_y)));
                // y
                for (double i = start_y; i <= end_y; i += GraphSetting.Y_SpaceValue)
                    Graphics.DrawLine(CoorSetting.Coor_pen, new PointF(xChangeToPixel_(start_x, i), yChangeToPixel_(start_x, i)), new PointF(xChangeToPixel_(end_x, i), yChangeToPixel_(end_x, i)));
            }

            if (CoorSetting.DrawAxises)
            {
                Graphics.DrawLine(CoorSetting.Axises_pen, new PointF(xChangeToPixel_(0, start_y), yChangeToPixel_(0, start_y)), new PointF(xChangeToPixel_(0, end_y), yChangeToPixel_(0, end_y)));
                Graphics.DrawLine(CoorSetting.Axises_pen, new PointF(xChangeToPixel_(start_x, 0), yChangeToPixel_(start_x, 0)), new PointF(xChangeToPixel_(end_x, 0), yChangeToPixel_(end_x, 0)));
            }


            start_x = Math.Floor(start_x / GraphSetting.X_SpaceValue) ;
            end_x = Math.Ceiling(end_x / GraphSetting.X_SpaceValue) ;
            start_y = Math.Floor(start_y / GraphSetting.Y_SpaceValue) ;
            end_y = Math.Ceiling(end_y / GraphSetting.Y_SpaceValue) ;

            double y, x;
            string num;
            if (CoorSetting.DrawNumbers == true)
            {
                switch (MainFunctions.GetLanguage())
                {
                    case MathPackage.Main.Language.AR:
                        break;
                    case MathPackage.Main.Language.EN:
                        {
                            // label position x
                            for (double i = start_x; i <= end_x; i += 1)
                            {
                                x = i * GraphSetting.X_SpaceValue;
                                num = MathPackage.Main.Approximate(x, "###").ToString() + CoorSetting.X_unit;
                                Graphics.DrawString(num, CoorSetting.Number_font, new SolidBrush(CoorSetting.Number_color), get1(x, num));
                            }
                            // label position y
                            for (double i = start_y; i <= end_y; i += 1)
                            {
                                y = i * GraphSetting.Y_SpaceValue;
                                num = MathPackage.Main.Approximate(y, "###").ToString() + CoorSetting.Y_unit;
                                Graphics.DrawString(num, CoorSetting.Number_font, new SolidBrush(CoorSetting.Number_color), get2(y, num));
                            }
                            break;
                        }
                }
            }
        }

        protected PointF get1(double x, string number)
        {
            PointF point = new PointF(xChangeToPixel_(x, 0), yChangeToPixel_(x, 0));
            if (point.Y < 4)
            {

                return new PointF((float)(point.X + (point.Y - 4) / Math.Tan(GraphSetting.YAngle)), 4);
            }
            SizeF size = Graphics.MeasureString(number, CoorSetting.Number_font);
            if (point.Y + size.Height + 4 > GraphSetting.Height)
            {
                return new PointF((float)(point.X + (point.Y - GraphSetting.Height) /Math.Tan(GraphSetting.YAngle)), GraphSetting.Height - 4 - size.Height);
            }
            else
            {
                return point;
            }
        }
        protected PointF get2(double y, string number)
        {
            PointF point = new PointF(xChangeToPixel_(0, y), yChangeToPixel_(0, y));
            if (point.X < 4)
            {
                return new PointF(4, (float)(point.Y + (point.X - 4) * Math.Tan(GraphSetting.XAngle)));
            }
            SizeF size = Graphics.MeasureString(number, CoorSetting.Number_font);
            if (point.X + size.Width + 4 > GraphSetting.Width)
            {
                return new PointF(GraphSetting.Width - 4 - size.Width, (float)(point.Y + (point.X - GraphSetting.Width) * Math.Tan(GraphSetting.XAngle)));
            }
            else
            {
                return point;
            }
        }

        //        public void Coordinate_Radian(Graphics g)
        //        {
        //            decimal start_x, start_y, end_x, end_y;
        //            // 'To Get The Region of the coor <the begining and the end of the xAxis> 
        //            // 'The value will be the distance between the center and the edgePoint.x as number of pixels
        //            start_x = Math.Floor(-Center_.X / (double)GraphSetting.Zoom / (double)GraphSetting.X_Space) * GraphSetting.X_Space * GraphSetting.Zoom;
        //            end_x = Math.Ceiling((-Center_.X + GraphSetting.Width) / (double)GraphSetting.Zoom / (double)GraphSetting.X_Space) * GraphSetting.X_Space * GraphSetting.Zoom;
        //            // 'To Get The Region of the coor <the begining and the end of the yAxis> 
        //            // 'The value will be the distance between the center and the edgePoint.y as number of pixels
        //            start_y = Math.Floor((Center_.Y - GraphSetting.Height) / (double)GraphSetting.Zoom / (double)GraphSetting.Y_Space) * GraphSetting.Y_Space * GraphSetting.Zoom;
        //            end_y = Math.Ceiling(Center_.Y / (double)GraphSetting.Zoom / (double)GraphSetting.Y_Space) * GraphSetting.Y_Space * GraphSetting.Zoom;

        //            if (CoorSetting.DrawDecimal)
        //            {
        //                // x
        //                for (decimal i = start_x; i <= end_x; i += GraphSetting.X_Space * GraphSetting.Zoom / (double)CoorSetting.decimal_space_x)
        //                    Graphics.DrawLine(CoorSetting.Decimal_Pen, new Point(Center_.X + i, Center_.Y - start_y), new Point(Center_.X + i, Center_.Y - end_y));
        //                // y
        //                for (decimal i = start_y; i <= end_y; i += GraphSetting.Y_Space * GraphSetting.Zoom / (double)CoorSetting.decimal_space_y)
        //                    Graphics.DrawLine(CoorSetting.Decimal_Pen, new Point(Center_.X + start_x, Center_.Y - i), new Point(Center_.X + end_x, Center_.Y - i));
        //            }

        //            if (CoorSetting.DrawCoor)
        //            {

        //                // x
        //                for (decimal i = start_x; i <= end_x; i += GraphSetting.X_Space * GraphSetting.Zoom)
        //                    Graphics.DrawLine(CoorSetting.coor_pen, new Point(Center_.X + i, Center_.Y - start_y), new Point(Center_.X + i, Center_.Y - end_y));
        //                // y
        //                for (decimal i = start_y; i <= end_y; i += GraphSetting.Y_Space * GraphSetting.Zoom)
        //                    Graphics.DrawLine(CoorSetting.coor_pen, new Point(Center_.X + start_x, Center_.Y - i), new Point(Center_.X + end_x, Center_.Y - i));
        //            }

        //            if (CoorSetting.DrawAxises)
        //            {
        //                Graphics.DrawLine(CoorSetting.axises_pen, new Point(Center_.X, Center_.Y - start_y), new Point(Center_.X, Center_.Y - end_y));
        //                Graphics.DrawLine(CoorSetting.axises_pen, new Point(Center_.X + start_x, Center_.Y), new Point(Center_.X + end_x, Center_.Y));
        //            }




        //            // 
        //            // To Get The Region of the coor <the begining and the end of the xAxis> 
        //            // 
        //            // The value will be the distance between the center and the edgePoint.x as number of pixels
        //            // 
        //            start_x = Math.Floor(-Center_.X / (double)GraphSetting.Zoom / (double)GraphSetting.X_Space / (double)CoorSetting.number_space_x) * GraphSetting.X_Space * GraphSetting.Zoom * CoorSetting.number_space_x;
        //            end_x = Math.Ceiling((-Center_.X + GraphSetting.Width) / (double)GraphSetting.Zoom / (double)GraphSetting.X_Space / (double)CoorSetting.number_space_x) * GraphSetting.X_Space * GraphSetting.Zoom * CoorSetting.number_space_x;

        //            // 
        //            // To Get The Region of the coor <the begining and the end of the yAxis> 
        //            // 
        //            // The value will be the distance between the center and the edgePoint.y as number of pixels
        //            // 
        //            end_y = Math.Ceiling(Center_.Y / (double)GraphSetting.Zoom / (double)GraphSetting.Y_Space / (double)CoorSetting.number_space_y) * GraphSetting.Y_Space * GraphSetting.Zoom * CoorSetting.number_space_y;
        //            start_y = Math.Floor((Center_.Y - GraphSetting.Height) / (double)GraphSetting.Zoom / (double)GraphSetting.Y_Space / (double)CoorSetting.number_space_y) * GraphSetting.Y_Space * GraphSetting.Zoom * CoorSetting.number_space_y;

        //            decimal StartX, StartY;
        //            decimal X_SpaceValue_ = GraphSetting.X_SpaceValue / (Math.PI / 2);
        //            StartX = Math.Round(start_x / (double)(GraphSetting.X_Space * GraphSetting.Zoom)) * X_SpaceValue_;
        //            StartY = Math.Round(start_y / (double)(GraphSetting.Y_Space * GraphSetting.Zoom)) * GraphSetting.Y_SpaceValue;
        //            int num1 = 0;
        //            string num;
        //            if (CoorSetting.DrawNumbers == true)
        //            {
        //                switch (MathPackage.Main.Lang)
        //                {
        //                    case object _ when MathPackage.Main.Language.AR:
        //                        {
        //                            if (CoorSetting.Block_numbers == true)
        //                            {
        //                                if (Center_.X < GraphSetting.Width - 25)
        //                                {
        //                                    if (Center_.X > 0)
        //                                    {
        //                                        // label position y
        //                                        for (decimal i = start_y; i <= end_y; i += GraphSetting.Zoom * GraphSetting.Y_Space * CoorSetting.number_space_y)
        //                                        {
        //                                            num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_y * GraphSetting.Y_SpaceValue + StartY, "###");
        //                                            num1 += 1;
        //                                            Graphics.DrawString((CoorSetting.y_unit + MathPackage.TextReformer.ToArNumbers(num)).ToString, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + 4, Center_.Y - i + 4);
        //                                        }
        //                                    }
        //                                    else
        //                                        // label position y
        //                                        for (decimal i = start_y; i <= end_y; i += GraphSetting.Zoom * GraphSetting.Y_Space * CoorSetting.number_space_y)
        //                                        {
        //                                            num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_y * GraphSetting.Y_SpaceValue + StartY, "###");
        //                                            num1 += 1;
        //                                            Graphics.DrawString((CoorSetting.y_unit + MathPackage.TextReformer.ToArNumbers(num)).ToString, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), 4, Center_.Y - i + 4);
        //                                        }
        //                                }
        //                                else
        //                                    // label position y
        //                                    for (decimal i = start_y; i <= end_y; i += GraphSetting.Zoom * GraphSetting.Y_Space * CoorSetting.number_space_y)
        //                                    {
        //                                        num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_y * GraphSetting.Y_SpaceValue + StartY, "###");

        //                                        num1 += 1;
        //                                        Graphics.DrawString((CoorSetting.y_unit + MathPackage.TextReformer.ToArNumbers(num)).ToString, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), GraphSetting.Width - 25, Center_.Y - i + 4);
        //                                    }

        //                                num1 = 0;

        //                                if (Center_.Y < GraphSetting.Height - 20)
        //                                {
        //                                    if (Center_.Y > 4)
        //                                    {
        //                                        // label position x
        //                                        for (decimal i = start_x; i <= end_x; i += GraphSetting.Zoom * GraphSetting.X_Space * CoorSetting.number_space_x)
        //                                        {
        //                                            num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_x * X_SpaceValue_ + StartX, "###");
        //                                            num1 += 1;
        //                                            if (num != 0)
        //                                                Graphics.DrawString(MathPackage.TextReformer.ToArNumbers(num, "ط" + @"\٢" + CoorSetting.x_unit), GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + i + 4, Center_.Y + 4);
        //                                        }
        //                                    }
        //                                    else
        //                                        // label position x
        //                                        for (decimal i = start_x; i <= end_x; i += GraphSetting.Zoom * GraphSetting.X_Space * CoorSetting.number_space_x)
        //                                        {
        //                                            num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_x * X_SpaceValue_ + StartX, "###");
        //                                            num1 += 1;
        //                                            if (num != 0)
        //                                                Graphics.DrawString(MathPackage.TextReformer.ToArNumbers(num, "ط" + @"\٢" + CoorSetting.x_unit), GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + i + 4, 4);
        //                                        }
        //                                }
        //                                else
        //                                    // label position x
        //                                    for (decimal i = start_x; i <= end_x; i += GraphSetting.Zoom * GraphSetting.X_Space * CoorSetting.number_space_x)
        //                                    {
        //                                        num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_x * X_SpaceValue_ + StartX, "###");
        //                                        num1 += 1;
        //                                        if (num != 0)
        //                                            Graphics.DrawString(MathPackage.TextReformer.ToArNumbers(num, "ط" + @"\٢" + CoorSetting.x_unit), GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + i + 4, GraphSetting.Height - 20);
        //                                    }
        //                            }
        //                            else
        //                            {
        //                                // label position y
        //                                for (decimal i = start_y; i <= end_y; i += GraphSetting.Zoom * GraphSetting.Y_Space * CoorSetting.number_space_y)
        //                                {
        //                                    num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_y * GraphSetting.Y_SpaceValue + StartY, "###");
        //                                    num1 += 1;
        //                                    Graphics.DrawString((CoorSetting.y_unit + MathPackage.TextReformer.ToArNumbers(num)).ToString, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + 4, Center_.Y - i + 4);
        //                                }
        //                                // label position x
        //                                num1 = 0;
        //                                for (decimal i = start_x; i <= end_x; i += GraphSetting.Zoom * GraphSetting.X_Space * CoorSetting.number_space_x)
        //                                {
        //                                    num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_x * X_SpaceValue_ + StartX, "###");
        //                                    num1 += 1;
        //                                    if (num != 0)
        //                                        Graphics.DrawString(MathPackage.TextReformer.ToArNumbers(num, "ط" + @"\٢" + CoorSetting.x_unit), GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + i + 4, Center_.Y + 4);
        //                                }
        //                            }

        //                            break;
        //                        }

        //                    case object _ when MathPackage.Main.Language.EN:
        //                        {
        //                            if (CoorSetting.Block_numbers == true)
        //                            {
        //                                if (Center_.X < GraphSetting.Width - 25)
        //                                {
        //                                    if (Center_.X > 0)
        //                                    {
        //                                        // label position y
        //                                        for (decimal i = start_y; i <= end_y; i += GraphSetting.Zoom * GraphSetting.Y_Space * CoorSetting.number_space_y)
        //                                        {
        //                                            num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_y * GraphSetting.Y_SpaceValue + StartY, "###");
        //                                            num1 += 1;
        //                                            Graphics.DrawString(num + CoorSetting.y_unit, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + 4, Center_.Y - i + 4);
        //                                        }
        //                                    }
        //                                    else
        //                                        // label position y
        //                                        for (decimal i = start_y; i <= end_y; i += GraphSetting.Zoom * GraphSetting.Y_Space * CoorSetting.number_space_y)
        //                                        {
        //                                            num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_y * GraphSetting.Y_SpaceValue + StartY, "###");
        //                                            num1 += 1;
        //                                            Graphics.DrawString(num + CoorSetting.y_unit, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), 4, Center_.Y - i + 4);
        //                                        }
        //                                }
        //                                else
        //                                    // label position y
        //                                    for (decimal i = start_y; i <= end_y; i += GraphSetting.Zoom * GraphSetting.Y_Space * CoorSetting.number_space_y)
        //                                    {
        //                                        num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_y * GraphSetting.Y_SpaceValue + StartY, "###");
        //                                        num1 += 1;
        //                                        Graphics.DrawString(num + CoorSetting.y_unit, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), GraphSetting.Width - 25, Center_.Y - i + 4);
        //                                    }

        //                                num1 = 0;

        //                                if (Center_.Y < GraphSetting.Height - 20)
        //                                {
        //                                    if (Center_.Y > 4)
        //                                    {
        //                                        // label position x
        //                                        for (decimal i = start_x; i <= end_x; i += GraphSetting.Zoom * GraphSetting.X_Space * CoorSetting.number_space_x)
        //                                        {
        //                                            num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_x * X_SpaceValue_ + StartX, "###");
        //                                            num1 += 1;
        //                                            if (num != 0)
        //                                                Graphics.DrawString(num + "π/2" + CoorSetting.x_unit, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + i + 4, Center_.Y + 4);
        //                                        }
        //                                    }
        //                                    else
        //                                        // label position x
        //                                        for (decimal i = start_x; i <= end_x; i += GraphSetting.Zoom * GraphSetting.X_Space * CoorSetting.number_space_x)
        //                                        {
        //                                            num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_x * X_SpaceValue_ + StartX, "###");
        //                                            num1 += 1;
        //                                            if (num != 0)
        //                                                Graphics.DrawString(num + "π/2" + CoorSetting.x_unit, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + i + 4, 4);
        //                                        }
        //                                }
        //                                else
        //                                    // label position x
        //                                    for (decimal i = start_x; i <= end_x; i += GraphSetting.Zoom * GraphSetting.X_Space * CoorSetting.number_space_x)
        //                                    {
        //                                        num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_x * X_SpaceValue_ + StartX, "###");
        //                                        num1 += 1;
        //                                        if (num != 0)
        //                                            Graphics.DrawString(num + "π/2" + CoorSetting.x_unit, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + i + 4, GraphSetting.Height - 20);
        //                                    }
        //                            }
        //                            else
        //                            {
        //                                // label position y
        //                                for (decimal i = start_y; i <= end_y; i += GraphSetting.Zoom * GraphSetting.Y_Space * CoorSetting.number_space_y)
        //                                {
        //                                    num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_y * GraphSetting.Y_SpaceValue + StartY, "###");
        //                                    num1 += 1;
        //                                    if (num != 0)
        //                                        Graphics.DrawString(num + CoorSetting.y_unit, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + 4, Center_.Y - i + 4);
        //                                }
        //                                // label position x
        //                                num1 = 0;
        //                                for (decimal i = start_x; i <= end_x; i += GraphSetting.Zoom * GraphSetting.X_Space * CoorSetting.number_space_x)
        //                                {
        //                                    num = MathPackage.Main.Approximate(num1 * CoorSetting.number_space_x * X_SpaceValue_ + StartX, "###");
        //                                    num1 += 1;
        //                                    if (num != 0)
        //                                        Graphics.DrawString(num + "π/2" + CoorSetting.x_unit, GraphSetting.Font, new SolidBrush(CoorSetting.number_color), Center_.X + i + 4, Center_.Y + 4);
        //                                }
        //                            }

        //                            break;
        //                        }
        //                }
        //            }
        //        }

        //        public void Coordinate_polar(Graphics g)
        //        {
        //            int StartRadius, EndRadius;


        //            decimal start_x, end_x;
        //            start_x = xChangeToCoor_(0);
        //            end_x = xChangeToCoor_(GraphSetting.Width);
        //            ;/* Cannot convert OnErrorResumeNextStatementSyntax, CONVERSION ERROR: Conversion for OnErrorResumeNextStatement not implemented, please report this issue in 'On Error Resume Next' at character 36256
        //   at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.MethodBodyVisitor.DefaultVisit(SyntaxNode node)
        //   at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitOnErrorResumeNextStatement(OnErrorResumeNextStatementSyntax node)
        //   at Microsoft.CodeAnalysis.VisualBasic.Syntax.OnErrorResumeNextStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
        //   at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
        //   at ICSharpCode.CodeConverter.CSharp.CommentConvertingMethodBodyVisitor.ConvertWithTrivia(SyntaxNode node)
        //   at ICSharpCode.CodeConverter.CSharp.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)

        //Input: 

        //            On Error Resume Next

        // */
        //            if (CoorSetting.DrawPolarLines)
        //            {
        //                for (decimal theta = Math.PI / CoorSetting.polar_line_space; theta <= Math.PI - Math.PI / CoorSetting.polar_line_space + (Math.PI / CoorSetting.polar_line_space / 1000); theta += Math.PI / CoorSetting.polar_line_space)
        //                    Graphics.DrawLine(CoorSetting.PolarLines_pen, new Point(0, yChangeToPixel_(start_x * Math.Tan(theta))), new Point(GraphSetting.Width, yChangeToPixel_(end_x * Math.Tan(theta))));
        //            }



        //            if (CoorSetting.DrawPolarCircles)
        //            {
        //                if (Center_.X >= 0 & Center_.X <= GraphSetting.Width)
        //                {
        //                    if (Center_.Y >= 0 & Center_.Y <= GraphSetting.Height)
        //                    {
        //                        StartRadius = CoorSetting.polar_circle_space;
        //                        EndRadius = Math.Max(Math.Max(Math.Max(Math.Pow((Math.Pow(X_Domain_[0], 2) + Math.Pow(Y_Domain_[0], 2)), 0.5), Math.Pow((Math.Pow(X_Domain_[1], 2) + Math.Pow(Y_Domain_[0], 2)), 0.5)), Math.Pow((Math.Pow(X_Domain_[0], 2) + Math.Pow(Y_Domain_[1], 2)), 0.5)), Math.Pow((Math.Pow(X_Domain_[1], 2) + Math.Pow(Y_Domain_[1], 2)), 0.5));
        //                    }
        //                    else if (Center_.Y < 0)
        //                    {
        //                        StartRadius = Math.Round(Y_Domain_[1] / (double)CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                        EndRadius = Math.Max(Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[0], 2)), 0.5), Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[1], 2)), 0.5));
        //                    }
        //                    else if (Center_.Y > GraphSetting.Height)
        //                    {
        //                        StartRadius = Math.Round(Y_Domain_[0] / (double)CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                        EndRadius = Math.Max(Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[0], 2)), 0.5), Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[1], 2)), 0.5));
        //                    }
        //                }
        //                else if (Center_.X < 0)
        //                {
        //                    if (Center_.Y >= 0 & Center_.Y <= GraphSetting.Height)
        //                    {
        //                        StartRadius = Math.Round(X_Domain_[0] / (double)CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                        EndRadius = Math.Round(Math.Max(Math.Pow((Math.Pow(X_Domain_[1], 2) + Math.Pow(Y_Domain_[0], 2)), 0.5), Math.Pow((Math.Pow(X_Domain_[1], 2) + Math.Pow(Y_Domain_[1], 2)), 0.5)) / CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                    }
        //                    else if (Center_.Y < 0)
        //                    {
        //                        StartRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[0], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                        EndRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[1], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                    }
        //                    else if (Center_.Y > GraphSetting.Height)
        //                    {
        //                        StartRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[0], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                        EndRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[1], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                    }
        //                }
        //                else if (Center_.X > GraphSetting.Width)
        //                {
        //                    if (Center_.Y >= 0 & Center_.Y <= GraphSetting.Height)
        //                    {
        //                        StartRadius = Math.Round(X_Domain_[1] / (double)CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                        EndRadius = Math.Round(Math.Max(Math.Pow((Math.Pow(X_Domain_[0], 2) + Math.Pow(Y_Domain_[0], 2)), 0.5), Math.Pow((Math.Pow(X_Domain_[0], 2) + Math.Pow(Y_Domain_[1], 2)), 0.5)) / CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                    }
        //                    else if (Center_.Y < 0)
        //                    {
        //                        StartRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[1], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                        EndRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[0], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                    }
        //                    else if (Center_.Y > GraphSetting.Height)
        //                    {
        //                        StartRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[1], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                        EndRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[0], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                    }
        //                }

        //                for (decimal i = StartRadius; i <= EndRadius; i += CoorSetting.polar_circle_space)
        //                    Graphics.DrawEllipse(CoorSetting.PolarCircles_pen, Center_.X - i * GraphSetting.X_Stretch, Center_.Y - i * GraphSetting.Y_Stretch, 2 * i * GraphSetting.X_Stretch, 2 * i * GraphSetting.Y_Stretch);

        //                if (CoorSetting.DrawPolarAngles)
        //                {
        //                    int i;
        //                    switch (MathPackage.Main.Lang)
        //                    {
        //                        case object _ when MathPackage.Main.Language.AR:
        //                            {
        //                                for (decimal theta = Math.PI / CoorSetting.polar_line_space; theta <= 2 * Math.PI - Math.PI / CoorSetting.polar_line_space + (Math.PI / CoorSetting.polar_line_space / 1000); theta += Math.PI / CoorSetting.polar_line_space)
        //                                {
        //                                    i += 1;
        //                                    if (string.Format("{0:0.0000}", Math.Abs(Math.Sin(theta))) != 1 && string.Format("{0:0.0000}", Math.Abs(Math.Cos(theta))) != 1)
        //                                        Graphics.DrawString(MathPackage.TextReformer.ToArNumbers(i) + "ط" + @"\" + MathPackage.TextReformer.ToArNumbers(CoorSetting.polar_line_space), GraphSetting.Font, new SolidBrush(GraphSetting.number_color), new Point(xChangeToPixel_(Math.Cos(theta) * (StartRadius + EndRadius) / 2), yChangeToPixel_(Math.Sin(theta) * (StartRadius + EndRadius) / 2)));
        //                                }

        //                                break;
        //                            }

        //                        case object _ when MathPackage.Main.Language.EN:
        //                            {
        //                                for (decimal theta = Math.PI / CoorSetting.polar_line_space; theta <= 2 * Math.PI - Math.PI / CoorSetting.polar_line_space + (Math.PI / CoorSetting.polar_line_space / 1000); theta += Math.PI / CoorSetting.polar_line_space)
        //                                {
        //                                    i += 1;
        //                                    if (string.Format("{0:0.0000}", Math.Abs(Math.Sin(theta))) != 1 && string.Format("{0:0.0000}", Math.Abs(Math.Cos(theta))) != 1)
        //                                        Graphics.DrawString(i + "π/" + CoorSetting.polar_line_space, GraphSetting.Font, new SolidBrush(GraphSetting.number_color), new Point(xChangeToPixel_(Math.Cos(theta) * (StartRadius + EndRadius) / 2), yChangeToPixel_(Math.Sin(theta) * (StartRadius + EndRadius) / 2)));
        //                                }

        //                                break;
        //                            }
        //                    }
        //                }
        //            }
        //            else if (CoorSetting.DrawPolarAngles)
        //            {
        //                if (Center_.X >= 0 & Center_.X <= GraphSetting.Width)
        //                {
        //                    if (Center_.Y >= 0 & Center_.Y <= GraphSetting.Height)
        //                    {
        //                        StartRadius = CoorSetting.polar_circle_space;
        //                        EndRadius = Math.Max(Math.Max(Math.Max(Math.Pow((Math.Pow(X_Domain_[0], 2) + Math.Pow(Y_Domain_[0], 2)), 0.5), Math.Pow((Math.Pow(X_Domain_[1], 2) + Math.Pow(Y_Domain_[0], 2)), 0.5)), Math.Pow((Math.Pow(X_Domain_[0], 2) + Math.Pow(Y_Domain_[1], 2)), 0.5)), Math.Pow((Math.Pow(X_Domain_[1], 2) + Math.Pow(Y_Domain_[1], 2)), 0.5));
        //                    }
        //                    else if (Center_.Y < 0)
        //                    {
        //                        StartRadius = Math.Round(Y_Domain_[1] / (double)CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                        EndRadius = Math.Max(Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[0], 2)), 0.5), Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[1], 2)), 0.5));
        //                    }
        //                    else if (Center_.Y > GraphSetting.Height)
        //                    {
        //                        StartRadius = Math.Round(Y_Domain_[0] / (double)CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                        EndRadius = Math.Max(Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[0], 2)), 0.5), Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[1], 2)), 0.5));
        //                    }
        //                }
        //                else if (Center_.X < 0)
        //                {
        //                    if (Center_.Y >= 0 & Center_.Y <= GraphSetting.Height)
        //                    {
        //                        StartRadius = Math.Round(X_Domain_[0] / (double)CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                        EndRadius = Math.Round(Math.Max(Math.Pow((Math.Pow(X_Domain_[1], 2) + Math.Pow(Y_Domain_[0], 2)), 0.5), Math.Pow((Math.Pow(X_Domain_[1], 2) + Math.Pow(Y_Domain_[1], 2)), 0.5)) / CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                    }
        //                    else if (Center_.Y < 0)
        //                    {
        //                        StartRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[0], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                        EndRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[1], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                    }
        //                    else if (Center_.Y > GraphSetting.Height)
        //                    {
        //                        StartRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[0], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                        EndRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[1], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                    }
        //                }
        //                else if (Center_.X > GraphSetting.Width)
        //                {
        //                    if (Center_.Y >= 0 & Center_.Y <= GraphSetting.Height)
        //                    {
        //                        StartRadius = Math.Round(X_Domain_[1] / (double)CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                        EndRadius = Math.Round(Math.Max(Math.Pow((Math.Pow(X_Domain_[0], 2) + Math.Pow(Y_Domain_[0], 2)), 0.5), Math.Pow((Math.Pow(X_Domain_[0], 2) + Math.Pow(Y_Domain_[1], 2)), 0.5)) / CoorSetting.polar_circle_space) * CoorSetting.polar_circle_space;
        //                    }
        //                    else if (Center_.Y < 0)
        //                    {
        //                        StartRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[1], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                        EndRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[0], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                    }
        //                    else if (Center_.Y > GraphSetting.Height)
        //                    {
        //                        StartRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[0], 2) + Math.Pow(X_Domain_[1], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                        EndRadius = Math.Round(Math.Pow((Math.Pow(Y_Domain_[1], 2) + Math.Pow(X_Domain_[0], 2)), 0.5) / (CoorSetting.polar_circle_space)) * (CoorSetting.polar_circle_space);
        //                    }
        //                }

        //                int i;
        //                switch (MathPackage.Main.Lang)
        //                {
        //                    case object _ when MathPackage.Main.Language.AR:
        //                        {
        //                            for (decimal theta = Math.PI / CoorSetting.polar_line_space; theta <= 2 * Math.PI - Math.PI / CoorSetting.polar_line_space + (Math.PI / CoorSetting.polar_line_space / 1000); theta += Math.PI / CoorSetting.polar_line_space)
        //                            {
        //                                i += 1;
        //                                if (string.Format("{0:0.0000}", Math.Abs(Math.Sin(theta))) != 1 && string.Format("{0:0.0000}", Math.Abs(Math.Cos(theta))) != 1)
        //                                    Graphics.DrawString(MathPackage.TextReformer.ToArNumbers(i) + "ط" + @"\" + MathPackage.TextReformer.ToArNumbers(CoorSetting.polar_line_space), GraphSetting.Font, new SolidBrush(GraphSetting.number_color), new Point(xChangeToPixel_(Math.Cos(theta) * (StartRadius + EndRadius) / 2), yChangeToPixel_(Math.Sin(theta) * (StartRadius + EndRadius) / 2)));
        //                            }

        //                            break;
        //                        }

        //                    case object _ when MathPackage.Main.Language.EN:
        //                        {
        //                            for (decimal theta = Math.PI / CoorSetting.polar_line_space; theta <= 2 * Math.PI - Math.PI / CoorSetting.polar_line_space + (Math.PI / CoorSetting.polar_line_space / 1000); theta += Math.PI / CoorSetting.polar_line_space)
        //                            {
        //                                i += 1;
        //                                if (string.Format("{0:0.0000}", Math.Abs(Math.Sin(theta))) != 1 && string.Format("{0:0.0000}", Math.Abs(Math.Cos(theta))) != 1)
        //                                    Graphics.DrawString(i + "π/" + CoorSetting.polar_line_space, GraphSetting.Font, new SolidBrush(GraphSetting.number_color), new Point(xChangeToPixel_(Math.Cos(theta) * (StartRadius + EndRadius) / 2), yChangeToPixel_(Math.Sin(theta) * (StartRadius + EndRadius) / 2)));
        //                            }

        //                            break;
        //                        }
        //                }
        //            }
        //        }

        public System.Xml.Linq.XElement GetAsXml()
        {
            System.Xml.Linq.XElement element = new System.Xml.Linq.XElement("Coordinates");
            object settings = CoorSetting.GetAsXml();
            if (settings == null)
            {
                return null;
            }
            element.Add(CoorSetting.GetAsXml());
            return element;
        }

    }

    public class CoorSetting
    {
        private GraphSetting GraphSetting;
        public CoorSetting(GraphSetting GraphSetting_)
        {
            GraphSetting = GraphSetting_;
        }

        private CoorType Type_ = CoorType.Custom;
        public CoorType Type
        {
            get
            {
                return Type_;
            }
            set
            {
                if (value == CoorType.Radian && Type_ == CoorType.Custom)
                {
                    GraphSetting.X_Space *= Math.PI / 2;
                    GraphSetting.X_SpaceValue *= Math.PI / 2;
                }
                else if (value == CoorType.Custom && Type_ == CoorType.Radian)
                {
                    GraphSetting.X_Space /= Math.PI / 2;
                    GraphSetting.X_SpaceValue /= Math.PI / 2;
                }
                Type_ = value;
            }
        }
        public enum CoorType
        {
            Custom,
            Radian
        }
        public Color BackColor = Color.White;

        public string X_unit = "";
        public string Y_unit = "";

        public Font Number_font = new Font("Segoe II", 12.0f);
        public Color Number_color = Color.Black;


        public bool DrawDecimal = true;
        public bool DrawCoor = true;
        public bool DrawAxises = true;
        public bool DrawNumbers = true;
        public bool DrawPolarCircles = false;
        public bool DrawPolarLines = false;
        public bool DrawPolarAngles = false;

        bool autoRefiningOnZoomimg = true;
        public bool AutoRefiningOnZoomimg
        {
            get => autoRefiningOnZoomimg;
            set
            {
                autoRefiningOnZoomimg = value;
                GraphSetting.Centerate();
            }
        }


        public Pen Coor_pen = new Pen(Color.Silver, 1);
        public Pen Decimal_Pen = new Pen(Color.Gainsboro, 1);
        public Pen Axises_pen = new Pen(Color.Black, 2);
        public Pen PolarCircles_pen = new Pen(Color.Gray, 1);
        public Pen PolarLines_pen = new Pen(Color.Gray, 1);

        public int Decimal_space_y = 5;
        public int Decimal_space_x = 5;
        public int Number_space_x = 1;
        public int Number_space_y = 1;
        public double Polar_circle_space = 1;
        public double Polar_line_space = 6;

        public System.Xml.Linq.XElement GetAsXml()
        {
            string settings = "";

            if (Type != CoorType.Custom) settings += $"<Type Value=\"{Type.ToString()}\"/>";
            if (AutoRefiningOnZoomimg != true) settings += $"<AutoRefiningOnZoomimg Value=\"{AutoRefiningOnZoomimg.ToString()}\"/>";
            if (BackColor != Color.White) settings += $"<BackColor Value=\"{MainFunctions.GetColorAsString(BackColor)}\"/>";
            if (Number_color != Color.Black) settings += $"<Number_color Value=\"{MainFunctions.GetColorAsString(Number_color)}\"/>";
            if (Number_font.Name.ToString() != "Tahoma" && Number_font.Size != 10) settings += $"<Number_font Value=\"{MainFunctions.GetFontAsString(Number_font)}\"/>";
            if (!string.IsNullOrEmpty(X_unit)) settings += $"<X_unit Value=\"{X_unit.ToString()}\"/>";
            if (!string.IsNullOrEmpty(Y_unit)) settings += $"<y_unit Value=\"{Y_unit.ToString()}\"/>";
            if (Number_space_x != 1) settings += $"<Number_space_x Value=\"{Number_space_x.ToString()}\"/>";
            if (Number_space_y != 1) settings += $"<Number_space_y Value=\"{Number_space_y.ToString()}\"/>";
            if (Polar_circle_space != 1) settings += $"<Polar_circle_space Value=\"{Polar_circle_space.ToString()}\"/>";
            if (Polar_line_space != 6) settings += $"<Polar_line_space Value=\"{Polar_line_space.ToString()}\"/>";
            if (Decimal_space_x != 5) settings += $"<Decimal_space_x Value=\"{Decimal_space_x.ToString()}\"/>";
            if (Decimal_space_y != 5) settings += $"<Decimal_space_y Value=\"{Decimal_space_y.ToString()}\"/>";

            if (DrawDecimal != true) settings += $"<DrawDecimal Value=\"{DrawDecimal.ToString()}\"/>";
            if (DrawCoor != true) settings += $"<DrawCoor Value=\"{DrawCoor.ToString()}\"/>";
            if (DrawAxises != true) settings += $"<DrawAxises Value=\"{DrawAxises.ToString()}\"/>";
            if (DrawNumbers != true) settings += $"<DrawNumbers Value=\"{DrawNumbers.ToString()}\"/>";
            if (DrawPolarCircles != false) settings += $"<DrawPolarCircles Value=\"{DrawPolarCircles.ToString()}\"/>";
            if (DrawPolarLines != false) settings += $"<DrawPolarLines Value=\"{DrawPolarLines.ToString()}\"/>";
            if (DrawPolarAngles != false) settings += $"<DrawPolarAngles Value=\"{DrawPolarAngles.ToString()}\"/>";

            if (Coor_pen.Color != Color.Silver && Coor_pen.Width != 1) settings += $"<Coor_pen Value=\"{MainFunctions.GetPenAsString(Coor_pen)}\"/>";
            if (Decimal_Pen.Color != Color.Gainsboro && Decimal_Pen.Width != 1) settings += $"<Decimal_Pen Value=\"{MainFunctions.GetPenAsString(Decimal_Pen)}\"/>";
            if (Axises_pen.Color != Color.Black && Axises_pen.Width != 2) settings += $"<Axises_pen Value=\"{MainFunctions.GetPenAsString(Axises_pen)}\"/>";
            if (PolarCircles_pen.Color != Color.Gray && PolarCircles_pen.Width != 1) settings += $"<PolarCircles_pen Value=\"{MainFunctions.GetPenAsString(PolarCircles_pen)}\"/>";
            if (PolarLines_pen.Color != Color.Gray && PolarLines_pen.Width != 1) settings += $"<PolarLines_pen Value=\"{MainFunctions.GetPenAsString(PolarLines_pen)}\"/>";

            if (string.IsNullOrEmpty(settings)) return null;

            return System.Xml.Linq.XElement.Parse($"<CoorSetting>{settings}</CoorSetting>");

        }

    }
}
