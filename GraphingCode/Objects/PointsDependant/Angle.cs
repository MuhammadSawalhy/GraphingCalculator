using System;
using System.Collections.Generic;
using System.Drawing;

namespace Graphing.Objects
{
    public class Angle : PointsDependant
    {
        public override PointDependantType Type { get; } = PointDependantType.Angle;
        public override string Get_Type => "Angle";
        public override int PointNumberAtLeast { get; } = 3;
        public override int PointNumberAtMost { get; } = 3;
        public MathPackage.Main.AngleType angletype = MathPackage.Main.AngleType.Degree;

        public Angle(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
        }

        public void RightAngle(Graphics g, double start_angle, double end_angle, Point circle_center, int Length)
        {
            List<Point> points = new List<Point>();
            points.Add(circle_center);
            points.Add(new Point((int)(circle_center.X + Length * Math.Cos(start_angle)), (int)(circle_center.Y - Length * Math.Sin(start_angle))));
            points.Add(new Point((int)(circle_center.X + Length * Math.Pow(2, 0.5) * Math.Cos(end_angle - (Math.PI / 4))), (int)(circle_center.Y - Length * Math.Pow(2, 0.5) * Math.Sin(end_angle - (Math.PI / 4)))));
            points.Add(new Point((int)(circle_center.X + Length * Math.Cos(end_angle)), (int)(circle_center.Y - Length * Math.Sin(end_angle))));

            if (points.Count > 1)
                Graphics.FillClosedCurve(new SolidBrush(Color.FromArgb(150, Pen.Color.R, Pen.Color.G, Pen.Color.B)), points.ToArray(), System.Drawing.Drawing2D.FillMode.Alternate, 0);
        }

        private string Text = "";

        public override void Draw(bool throwError = false)
        {
            try
            {

                Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
                Graphics = Graphics.FromImage(Bitmap);
                Graphics.SmoothingMode = MainFunctions.Smoothing;
                Graphics.CompositingQuality = MainFunctions.Composition;

                PointF[] location_ = new PointF[3];
                location_[0] = Points[0].Location;
                location_[1] = Points[1].Location;
                location_[2] = Points[2].Location;
                double[] sloop;
                try
                {
                    sloop = new double[] { MainFunctions.GetSloop(Points[1], Points[0]), MainFunctions.GetSloop(Points[1], Points[2]) };
                }
                catch
                {
                    return;
                }

                Graphics.DrawLine(Pen, location_[1], location_[0]);
                Graphics.DrawLine(Pen, location_[2], location_[1]);

                // drawing the arc of the angle.
                double angle = (sloop[1] - sloop[0]);

                if (angle < 0)
                    angle += 2 * Math.PI;


                if (MathPackage.Main.Approximate(angle, "###") == MathPackage.Main.Approximate(Math.PI / 2, "###"))
                {
                    Text = "π/2";
                    RightAngle(Graphics, sloop[0], sloop[1], new Point((int)location_[1].X, (int)location_[1].Y), 15);
                    return;
                }
                else
                {
                    MainFunctions.Fill_arc(Graphics, Pen, sloop[0], angle, new Point((int)location_[1].X, (int)location_[1].Y), 15, (int)(15 * GraphSetting.Y_Stretch / GraphSetting.X_Stretch), false);
                }


                ///Drawing the angel as string.
                Text Text_ = new Text()
                {
                    Font = GraphSetting.Font,
                    Color = Color.Black,
                    BackColor = Color.FromArgb(100, 255, 255, 255),
                    Location = new Point((int)location_[1].X + 5, (int)location_[1].Y + 5)
                };

                switch (angletype)
                {
                    case MathPackage.Main.AngleType.Radian:
                        {
                            Text = MathPackage.Main.Approximate(angle, "99", "00", "###").ToString();
                            switch (MainFunctions.GetLanguage())
                            {
                                case MathPackage.Main.Language.AR:
                                    {
                                        Text_.Value = MathPackage.Transformer.ToArNumbers(Text);
                                        Text_.DrawTo(Graphics);
                                        break;
                                    }

                                case MathPackage.Main.Language.EN:
                                    {
                                        Text_.Value = Text;
                                        Text_.DrawTo(Graphics);
                                        break;
                                    }
                            }
                            break;
                        }

                    case MathPackage.Main.AngleType.Grads:
                        {
                            angle = (angle * 200 / Math.PI);
                            Text = MathPackage.Main.Approximate(angle, "99", "00", "###").ToString();
                            switch (MainFunctions.GetLanguage())
                            {
                                case MathPackage.Main.Language.AR:
                                    {
                                        Text_.Value = MathPackage.Transformer.ToArNumbers(Text);
                                        Text_.DrawTo(Graphics);
                                        break;
                                    }

                                case MathPackage.Main.Language.EN:
                                    {
                                        Text_.Value = Text;
                                        Text_.DrawTo(Graphics);
                                        break;
                                    }
                            }
                            break;
                        }

                    case MathPackage.Main.AngleType.Degree:
                        {
                            angle = (angle * 180 / Math.PI);
                            Text = MathPackage.Transformer.AngleDegrees(angle.ToString(), MainFunctions.GetLanguage());
                            Text_.Value = Text;
                            Text_.DrawTo(Graphics);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                if (throwError)
                    throw ex;

                ShowError(ex.Message);
            }

        }

        public override string DiscriptionString()
        {
            return Text;
        }


    }
}
