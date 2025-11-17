using System;
using System.Collections.Generic;
using Loyc.Syntax;
using Loyc;
using System.Linq;
using System.Drawing;
using Graphing.Objects;
using MathPackage;
using number = System.Double;	// Change this line to make a calculator for a different data type 

namespace Graphing
{

    public class GraphSetting
    {

        public GraphSetting(Sketch sketch)
        {
            Center = new PointF(Width / 2, Height / 2);
            CalculationSetting = new CalculationSetting(Main.AngleType.Radian, this);
            Sketch = sketch;
            SliderTimer.Tick += timerTick;

            CalculationSetting.Vars[0].Name = sy_x;
            CalculationSetting.Vars[1].Name = sy_y;

            //For updating X and Y Strexh value.
            X_Stretch = (X_Space / X_SpaceValue);
            Y_Stretch = (Y_Space / Y_SpaceValue);
        }

        #region calc

        public Symbol sy_x = (Symbol)"x", sy_y = (Symbol)"y";
        public string var => sy_x.ToString();

        public CalculationSetting CalculationSetting;

        #endregion

        #region GraphMainSettings

        private PointF center;
        public PointF Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
                ChangeDomain();
            }
        }

        public CalcRange XRange { get; set; }
        public CalcRange YRange { get; set; }

        /// <summary>
        ///     ''' The x length unit as number of pixels ::::: "1" (x_unit) = "X_Stretch" (Pixel)
        ///     ''' </summary>
        ///     ''' <returns></returns>
        public double X_Stretch { get; private set; }

        /// <summary>
        ///     ''' The y length unit as number of pixels ::::: 1 y_unit = Y_Stretch Pixel
        ///     ''' </summary>
        ///     ''' <returns></returns>
        public double Y_Stretch { get; private set; }

        /// <summary>
        ///     ''' the Width of the drawing bitmap
        ///     ''' </summary>
        ///     ''' <returns></returns>
        protected int width = 500;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                ChangeDomain();
            }
        }

        /// <summary>
        ///     ''' the height of the drawing bitmap
        ///     ''' </summary>
        ///     ''' <returns></returns>
        protected int height = 500;
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                ChangeDomain();
            }
        }

        protected double x_Space = 80;
        public double X_Space
        {
            get
            {
                return x_Space;
            }
            set
            {
                if (value < 10)
                    throw new Exception("You Con Not Zoom More Than This.");
                x_Space = value;
                X_Stretch = (X_Space / X_SpaceValue);
                ChangeDomain();
            }
        }

        protected double x_spaceModifier = 1;
        public double X_spaceModifier
        {
            get => x_spaceModifier;
            set
            {
                x_spaceModifier = value;
                ChangeDomain();
            }
        }
        
        protected double y_spaceModifier = 1;
        public double Y_spaceModifier
        {
            get => y_spaceModifier;
            set
            {
                y_spaceModifier = value;
                ChangeDomain();
            }
        }
        
        protected double y_space = 80;
        public double Y_Space
        {
            get
            {
                return y_space;
            }
            set
            {
                if (value < 10)
                    throw new Exception("You Con Not Zoom More Than This.");
                y_space = value;
                Y_Stretch = (Y_Space / Y_SpaceValue);
                ChangeDomain();
            }
        }

        /// <summary>
        ///     ''' </summary>
        protected double x_SpaceValue = 2;
        public double X_SpaceValue
        {
            get
            {
                return x_SpaceValue;
            }
            set
            {
                x_SpaceValue = value;
                ChangeDomain();
            }
        }

        /// <summary>
        ///     ''' </summary>
        protected double y_SpaceValue = 2;
        public double Y_SpaceValue
        {
            get
            {
                return y_SpaceValue;
            }
            set
            {
                y_SpaceValue = value;
                ChangeDomain();
            }
        }

        protected double xAngle = Math.PI / 6;
        public double XAngle
        {
            get
            {
                return xAngle;
            }
            set
            {
                xAngle = value;
                ChangeDomain();
            }
        }

        protected double yAngle = Math.PI / 3;
        public double YAngle
        {
            get
            {
                return yAngle;
            }
            set
            {
                yAngle = value;
                ChangeDomain();
            }
        }

        public float xChangeToPixel(double xCoorValue, double yCoorValue)
        {
            return (float)(Center.X + (xCoorValue * X_Stretch) * Math.Cos(xAngle) + (yCoorValue * Y_Stretch) * Math.Cos(yAngle));
        }

        public float yChangeToPixel(double xCoorValue, double yCoorValue)
        {
            return (float)(Center.Y - ((yCoorValue * Y_Stretch) * Math.Sin(yAngle) + (xCoorValue * X_Stretch) * Math.Sin(xAngle)));
        }

        public double xChangeToCoor(float xPixelValue, float yPixelValue)
        {
            xPixelValue = xPixelValue - center.X;
            yPixelValue = center.Y - yPixelValue;
            return ((Math.Cos(YAngle) * yPixelValue - Math.Sin(YAngle) * xPixelValue) / Math.Sin(XAngle - YAngle)) / X_Stretch;
        }

        public double yChangeToCoor(float xPixelValue, float yPixelValue)
        {
            xPixelValue = xPixelValue - center.X;
            yPixelValue = center.Y - yPixelValue;
            return ((Math.Cos(XAngle) * yPixelValue - Math.Sin(XAngle) * xPixelValue) / Math.Sin(YAngle - XAngle)) / Y_Stretch;
        }

        private void ChangeDomain()
        {
            XRange = new CalcRange(xChangeToCoor(0, Math.Tan(yAngle) > 0 ? 0 : Height), xChangeToCoor(Width, Math.Tan(yAngle) < 0 ? 0 : Height), Width);
            YRange = new CalcRange(yChangeToCoor(Math.Tan(xAngle) > 0 ? Width : 0, Height), yChangeToCoor(Math.Tan(xAngle) < 0 ? Width : 0, 0), Height);
            X_Stretch = (X_Space * x_spaceModifier / X_SpaceValue);
            Y_Stretch = (Y_Space * y_spaceModifier / Y_SpaceValue);
        }

        public void Centerate()
        {
            Center = new PointF(Width/2, Height/2);
        }

        #endregion

        #region Zoom

        ZoomingState X_ZoomingState = ZoomingState.v2;
        ZoomingState Y_ZoomingState = ZoomingState.v2;
        enum ZoomingState
        {
            v1,
            v2,
            v5,
        }

        void TakeMeasuresForZoomIn_X(double V1, double V2)
        {
            double ration = V2 / V1;
            X_SpaceValue = X_SpaceValue * ration;
            X_Space = X_Space * ration;
        }
        void TakeMeasuresForZoomIn_Y(double V1, double V2)
        {
            double ration = V2 / V1;
            Y_SpaceValue = Y_SpaceValue * ration;
            Y_Space = Y_Space * ration;
        }

        public void ZoomIn_Auto()
        {
            #region X
            if (X_Space < 180) X_Space += 10;
            /// You have reached the boundary
            /// if 5 it will be 2 
            /// if 2 it will be 1
            /// if 1 it will be 5
            /// and you will multiply be a value (10^n) to make 0.5 , 2000 , 0.00001
            else if (X_Space >= 180)
            {
                if (X_ZoomingState == ZoomingState.v5)
                {
                    X_ZoomingState = ZoomingState.v2;
                    TakeMeasuresForZoomIn_X(5, 2);
                }
                else if (X_ZoomingState == ZoomingState.v2)
                {
                    X_ZoomingState = ZoomingState.v1;
                    TakeMeasuresForZoomIn_X(2, 1);
                }
                else if (X_ZoomingState == ZoomingState.v1)
                {
                    X_ZoomingState = ZoomingState.v5;
                    TakeMeasuresForZoomIn_X(1, 0.5);
                }
            }
            #endregion

            #region Y
            if (Y_Space < 180) Y_Space += 10;
            /// You have reached the boundary
            /// if 5 it will be 2 
            /// if 2 it will be 1
            /// if 1 it will be 5
            /// and you will multiply be a value (10^n) to make 0.5 , 2000 , 0.00001
            else if (Y_Space >= 180)
            {
                if (Y_ZoomingState == ZoomingState.v5)
                {
                    Y_ZoomingState = ZoomingState.v2;
                    TakeMeasuresForZoomIn_Y(5, 2);
                }
                else if (Y_ZoomingState == ZoomingState.v2)
                {
                    Y_ZoomingState = ZoomingState.v1;
                    TakeMeasuresForZoomIn_Y(2, 1);
                }
                else if (Y_ZoomingState == ZoomingState.v1)
                {
                    Y_ZoomingState = ZoomingState.v5;
                    TakeMeasuresForZoomIn_Y(1, 0.5);
                }
            }
            #endregion
        }
        public void ZoomOut_Auto()
        {

            #region X
            if (X_Space > 60) X_Space -= 10;
            /// You have reached the boundary
            /// if 5 it will be 2 
            /// if 2 it will be 1
            /// if 1 it will be 5
            /// and you will multiply be a value (10^n) to make 0.5 , 2000 , 0.00001
            else if (X_Space <= 60)
            {
                if (X_ZoomingState == ZoomingState.v1)
                {
                    X_ZoomingState = ZoomingState.v2;
                    TakeMeasuresForZoomIn_X(1, 2);
                }
                else if (X_ZoomingState == ZoomingState.v2)
                {
                    X_ZoomingState = ZoomingState.v5;
                    TakeMeasuresForZoomIn_X(2, 5);
                }
                else if (X_ZoomingState == ZoomingState.v5)
                {
                    X_ZoomingState = ZoomingState.v1;
                    TakeMeasuresForZoomIn_X(5, 10);
                }
            }
            #endregion

            #region Y
            if (Y_Space > 60) Y_Space -= 10;
            /// You have reached the boundary
            /// if 5 it will be 2 
            /// if 2 it will be 1
            /// if 1 it will be 5
            /// and you will multiply be a value (10^n) to make 0.5 , 2000 , 0.00001
            else if (Y_Space <= 60)
            {
                if (Y_ZoomingState == ZoomingState.v1)
                {
                    Y_ZoomingState = ZoomingState.v2;
                    TakeMeasuresForZoomIn_Y(1, 2);
                }
                else if (Y_ZoomingState == ZoomingState.v2)
                {
                    Y_ZoomingState = ZoomingState.v5;
                    TakeMeasuresForZoomIn_Y(2, 5);
                }
                else if (Y_ZoomingState == ZoomingState.v5)
                {
                    Y_ZoomingState = ZoomingState.v1;
                    TakeMeasuresForZoomIn_Y(5, 10);
                }
            }
            #endregion
        }

        public void ZoomIn_()
        {
            #region X
            X_Space += 0.1 * X_Space;
            #endregion
          
            #region Y
            Y_Space += 0.1* Y_Space;
            #endregion
        }
        public void ZoomOut_()
        {
            #region X
            if (X_Space - 0.1 * X_Space > 20)
                 X_Space -= 0.1 * X_Space;

            #endregion

            #region Y
            if (Y_Space - 0.1 * Y_Space > 20)
                Y_Space -= 0.1 * Y_Space;
            #endregion
        }

        public void ZoomIn(PointF CenterOfZoomimg, bool UpdateDrawing = true)
        {
            PointF before = new PointF((float)xChangeToCoor(CenterOfZoomimg.X, CenterOfZoomimg.Y), (float)yChangeToCoor(CenterOfZoomimg.X, CenterOfZoomimg.Y));

            if (Sketch.Coordinates.CoorSetting.AutoRefiningOnZoomimg)
                ZoomIn_Auto();
            else
                ZoomIn_();

            PointF delta = new PointF(xChangeToPixel(before.X, before.Y) - CenterOfZoomimg.X, yChangeToPixel(before.X, before.Y) - CenterOfZoomimg.Y);
            Center = new PointF(
                Center.X - delta.X,
                Center.Y - delta.Y
                );

            if (UpdateDrawing) Sketch.UpdateAndDraw();
        }
        public void ZoomOut(PointF CenterOfZoomimg, bool UpdateDrawing = true)
        {
            PointF before = new PointF((float)xChangeToCoor(CenterOfZoomimg.X, CenterOfZoomimg.Y), (float)yChangeToCoor(CenterOfZoomimg.X, CenterOfZoomimg.Y));

            if (Sketch.Coordinates.CoorSetting.AutoRefiningOnZoomimg)
                ZoomOut_Auto();
            else
                ZoomOut_();

            PointF delta = new PointF(xChangeToPixel(before.X, before.Y) - CenterOfZoomimg.X, yChangeToPixel(before.X, before.Y) - CenterOfZoomimg.Y);
            Center = new PointF(
                Center.X - delta.X,
                Center.Y - delta.Y
                );


            if (UpdateDrawing) Sketch.UpdateAndDraw();
        }


        #endregion

        #region SomeVariables

        public Font Font = new Font("Tahoma", 10.0f);
        public Color FontColor = Color.Black;
        public bool snap_int;
        public bool snap_coor = true;
        public bool PenIsStatic = false;
        public MathPackage.Main.Language Lang = MainFunctions.GetLanguage();
        public bool IsDelete;
        public State MyState = State.Ready;
        public enum State
        {
            Loading,
            Ready,
            Drawing,
            Clearing,
            Saving

        }

        public Sketch Sketch;

        public Dictionary<string, GraphObject> GetObjects()
        {
            Dictionary<string, GraphObject> GraphObjects = new Dictionary<string, GraphObject>();
            foreach (GraphObject obj in Sketch.Objects.ToArray())
            {
                GraphObjects.Add(obj.Name == null ? "" : obj.Name, obj);
            }
            return GraphObjects;
        }

        #endregion

        #region "Timer"

        public System.Windows.Forms.Timer SliderTimer = new System.Windows.Forms.Timer();

        protected bool SliderTimerOn_ = false;
        protected bool SliderTimerOn
        {
            get
            {
                return SliderTimerOn_;
            }
            set
            {
                SliderTimerOn_ = value;
                if (value)
                    SliderTimer.Start();
                else
                    SliderTimer.Stop();
            }
        }
        protected int TimerInterval_;
        protected int TimerInterval
        {
            get
            {
                return TimerInterval_;
            }
            set
            {
                TimerInterval_ = value;
                if (value > SliderTimer.Interval && value > 0)
                    SliderTimer.Interval = value;
            }
        }
        protected void GetTimerInterval()
        {
            foreach (var slider in Sketch.Sliders.ToArray())
            {
                if (slider.Timer.Interval > SliderTimer.Interval)
                    SliderTimer.Interval = slider.Timer.Interval;
            }
        }
        protected bool IsTimerActiveElse(Objects.Slider s)
        {
            foreach (var slider in Sketch.Sliders.ToArray())
            {
                if (slider.Timer.Enabled && slider != s)
                {
                    return true;
                }
            }
            return false;
        }
        protected void timerTick(System.Object sender, System.EventArgs e)
        {
            Sketch.UpdateAndDraw();
            Sketch.SketchControl.Draw();
            Sketch.SketchControl.FlushMemory();
        }

        /// <param name="Slider">The slider that its timer has changes</param>
        public void UpdateSliderTimerState(Objects.Slider Slider)
        {
            if (!Slider.Timer.Enabled)
            {
                if (!IsTimerActiveElse(Slider))
                {
                    SliderTimerOn = false;
                }
            }
            else
            {
                SliderTimerOn = true;
            }
            GetTimerInterval();
        }

        #endregion

        #region "Names"

        public readonly string[] CustomNames = new string[] {
            "A","B", "C","D","E","F","G","H","I", "J" ,"K", "L","M", "N", "O", "P", "Q",
            "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h",
            "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
          };

        //  Get the current auto-selected name. This will not be added into the list.
        public string SelectName()
        {
            foreach (string name_ in CustomNames)
            {
                if (!IsNameUsed(name_))
                {
                    return name_;
                }
            }
            throw new Exception(":(  There is not avaliable name.  ):");
        }

        public bool IsNameUsed(string name, List<string> ExceptNames = null)
        {
            object obj = GetObjByName(name);
            if (obj != null)
            {
                if (ExceptNames != null)
                {
                    if (!ExceptNames.Contains(name))
                    {
                        return true;
                    }
                }
                else
                    return true;
            }
            return false;
        }

        public void RemoveByName(string name, bool RemoveControl)
        {

        }

        public object GetObjByName(string Name_)
        {
            /// GraphObjects /// GraphObjects /// GraphObjects
            Dictionary<string, GraphObject> GraphObjects = GetObjects();
            for (int i = 0; i < GraphObjects.Count; i++)
            {
                if (GraphObjects.ElementAt(i).Key != null && GraphObjects.ElementAt(i).Key == Name_)
                {
                    return GraphObjects.ElementAt(i).Value;
                }
            }

            /// Lists /// Lists /// Lists /// Lists /// Lists
            for (int i = 0; i < Sketch.GraphObjectsLists.Count; i++)
            {
                if (Sketch.GraphObjectsLists[i].Name == Name_)
                {
                    return Sketch.GraphObjectsLists[i];
                }
            }
            for (int i = 0; i < Sketch.NumbersLists.Count; i++)
            {
                if (Sketch.NumbersLists[i].Name == Name_)
                {
                    return Sketch.NumbersLists[i];
                }
            }

            /// SLiders /// SLiders /// SLiders /// SLiders 
            List<Slider> sliders = Sketch.Sliders.ToList();
            foreach (Slider slider in sliders)
            {
                if (slider.Name == Name_)
                {
                    return slider;
                }
            }

            ///   vars  ///   vars  ///   vars  ///   vars
            for (int i = 0; i < CalculationSetting.Vars.Count; i++)
            {
                if (CalculationSetting.Vars[i].Name == (Symbol)Name_)
                {
                    return CalculationSetting.Vars[i];
                }
            }

            /// Func /// Func /// Func /// Func /// Func
            for (int i = 0; i < CalculationSetting.Funcs.Count; i++)
            {
                if (CalculationSetting.Funcs[i].Name == (Symbol)Name_)
                {
                    return CalculationSetting.Funcs[i];
                }
            }

            return null;
        }

        public void ChangeVarKey(Symbol name, Symbol CurrentName)
        {
            bool exist = IsNameUsed(name.Name, new List<string>(new string[] { CurrentName.Name }));
            if (exist)
            {
                throw new Exception($"This name \"{name.Name}\" has already been used.");
            }
            for (int i = 0; i < CalculationSetting.Vars.Count; i++)
            {
                if (CalculationSetting.Vars[i].Name == CurrentName)
                {
                    CalculationSetting.Vars[i].Name = name;
                    continue;
                }
            }
        }

        public void ChangeFuncKey(Symbol name, Symbol CurrentName)
        {
            bool exist = IsNameUsed(name.Name, new List<string>(new string[] { CurrentName.Name }));
            if (exist)
            {
                throw new Exception($"This name \"{name.Name}\" has already been used.");
            }
            for (int i = 0; i < CalculationSetting.Funcs.Count; i++)
            {
                if (CalculationSetting.Funcs[i].Name == CurrentName)
                {
                    CalculationSetting.Funcs[i].Name = name;
                    continue;
                }
            }
        }

        #endregion

        #region Save and Get

        public System.Xml.Linq.XElement GetAllAsXml()
        {
            MyState = State.Saving;

            string elements = "";

            #region Settings

            string str = "";

            if (sy_x != (Symbol)"x") str += $"<sy_x Value=\"{sy_x.ToString()}\"/>";
            if (sy_y != (Symbol)"y") str += $"<sy_y Value=\"{sy_y.ToString()}\"/>";
            if (Lang != Main.Language.EN) str += $"<Lang Value=\"{((int)Lang).ToString()}\"/>";
            if (FontColor != Color.Black) str += $"<FontColor Value=\"{MainFunctions.GetColorAsString(FontColor)}\"/>";
            if (Font.Name.ToString() != "Thoma" && Font.Size != 10) str += $"<Font Value=\"{MainFunctions.GetFontAsString(Font)}\"/>";
            if (CalculationSetting.AngleType != Main.AngleType.Radian) str += $"<AngleType Value=\"{((int)CalculationSetting.AngleType).ToString()}\"/>";
            str += $"<Center Value=\"{ "(" + Center.X + ", " + Center.Y + ")" }\"/>";
            if (X_Space != 1) str += $"<X_Space Value=\"{X_Space}\"/>";
            if (Y_Space != 1) str += $"<Y_Space Value=\"{Y_Space}\"/>";
            if (X_SpaceValue != 1) str += $"<X_SpaceValue Value=\"{X_SpaceValue}\"/>";
            if (Y_SpaceValue != 1) str += $"<Y_SpaceValue Value=\"{Y_SpaceValue}\"/>";
            if (snap_coor != true) str += $"<snap_coor Value=\"{snap_coor}\"/>";
            if (snap_int != false) str += $"<snap_int Value=\"{snap_int}\"/>";
            if (PenIsStatic != false) str += $"<PenIsStatic Value=\"{PenIsStatic}\"/>";

            if (!string.IsNullOrEmpty(str))
                elements += $"<Settings>{str}</Settings>";

            #endregion

            #region Coordinates

            System.Xml.Linq.XElement str_ = Sketch.Coordinates.GetAsXml();
            if (str_ != null && str_.Elements().Count() > 0)
                elements += str_;

            #endregion

            #region objects

            string objects = "";

            foreach (MathPackage.Operations.Func _func in CalculationSetting.Funcs)
                objects += _func.GetAsXml();

            List<string> sliders = new List<string>();
            foreach (Slider slider in Sketch.Sliders.ToArray())
            {
                objects += slider.GetAsXml();
                sliders.Add(slider.Name);
            }
            List<string> RestrictedNames = MainFunctions.RestrictedNames();
            for (int i = 0; i < CalculationSetting.Vars.Count; i++)
            {
                if (CalculationSetting.Vars[i].Name.Name != sy_x.Name && CalculationSetting.Vars[i].Name.Name != sy_y.Name && !sliders.Contains(CalculationSetting.Vars[i].Name.Name) && !RestrictedNames.Contains(CalculationSetting.Vars[i].Name.Name))
                {
                    objects += CalculationSetting.Vars.ElementAt(i).GetAsXml();
                }
            }

            foreach (GraphObject obj in Sketch.Objects.ToArray())
                objects += obj.GetAsXml();

            foreach (DinamicPen pen in Sketch.Pens.DinamicPenArray)
                objects += pen.GetAsXml();

            foreach (StaticPen pen in Sketch.Pens.StaticPenArray)
                objects += pen.GetAsXml();

            elements += $"<Objects>{objects}</Objects>";

            #endregion

            MyState = State.Ready;

            return System.Xml.Linq.XElement.Parse($"<Drawings Name=\"{Sketch.Name}\">{elements}</Drawings>");

        }

        public void GetDrawingFromXml(System.Xml.Linq.XElement xml)
        {
            MyState = State.Loading;

            foreach (System.Xml.Linq.XAttribute attr in xml.Attributes())
            {
                if (attr.Name.ToString() == "Name")
                    Sketch.Name = xml.Attribute("Name").Value;
            }
            foreach (System.Xml.Linq.XElement element in xml.Nodes())
            {
                if (element.Name.ToString() == "Settings")
                {
                    foreach (System.Xml.Linq.XElement element_ in element.Nodes())
                    {
                        switch (element_.Name.ToString())
                        {
                            case "AngleType":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        CalculationSetting.AngleType = (MathPackage.Main.AngleType)(int.Parse(attr.Value));
                                    }
                                }
                                break;
                            case "FontColor":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        FontColor = MainFunctions.GetColorFromString(attr.Value, CalculationSetting);
                                    }
                                }
                                break;
                            case "Font":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        Font = MainFunctions.GetFontFromString(attr.Value);
                                    }
                                }
                                break;
                            case "Center":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        LNode center = MainFunctions.ParseExprs(attr.Value);
                                        if (center.Calls(CodeSymbols.Tuple) && center.Args.Count == 2)
                                        {
                                            Center = new PointF((float)MathPackage.Main.CalculateLNode(center.Args[0], CalculationSetting), (float)MathPackage.Main.CalculateLNode(center.Args[1], CalculationSetting));
                                        }
                                        else
                                            throw new Exception($"The value \"{ attr.Value }\" for the center is not valid.");
                                    }
                                }
                                break;
                            case "X_Space":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        X_Space = number.Parse(attr.Value);
                                    }
                                }
                                break;
                            case "Y_Space":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        Y_Space = number.Parse(attr.Value);
                                    }
                                }
                                break;
                            case "X_SpaceValue":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        X_SpaceValue = number.Parse(attr.Value);
                                    }
                                }
                                break;
                            case "Y_SpaceValue":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        Y_SpaceValue = number.Parse(attr.Value);
                                    }
                                }
                                break;
                            case "snap_coor":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        snap_coor = GetBool(attr.Value);
                                    }
                                }
                                break;
                            case "snap_int":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        snap_int = GetBool(attr.Value);
                                    }
                                }
                                break;
                            case "PenIsStatic":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        PenIsStatic = GetBool(attr.Value);
                                    }
                                }
                                break;
                            case "sy_x":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        sy_x = (Loyc.Symbol)attr.Value;
                                    }
                                }
                                break;
                            case "sy_y":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        sy_y = (Loyc.Symbol)attr.Value;
                                    }
                                }
                                break;
                            case "Lang":
                                foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                {
                                    if (attr.Name == "Value")
                                    {
                                        switch ((MathPackage.Main.Language)(int.Parse(attr.Value)))
                                        {
                                            case MathPackage.Main.Language.AR:
                                                {
                                                    MainFunctions.SetLanguage(MathPackage.Main.Language.AR);
                                                    break;
                                                }
                                            case MathPackage.Main.Language.EN:
                                                {
                                                    MainFunctions.SetLanguage(MathPackage.Main.Language.EN);
                                                    break;
                                                }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                else if (element.Name.ToString() == "Coordinates")
                {
                    foreach (System.Xml.Linq.XElement element_ in element.Nodes())
                    {
                        switch (element_.Name.ToString())
                        {
                            case "CoorSetting":
                                Get_coor_setting(element_);
                                break;
                        }
                    }
                }
                else if (element.Name.ToString() == "Objects")
                {

                    #region Getting the objects
                    List<System.Xml.Linq.XNode> nodes = new List<System.Xml.Linq.XNode>(element.Nodes());
                    System.Xml.Linq.XElement element_;

                    /// Putting the variables at first.
                    for (int i = 0; i < nodes.Count(); i++)
                    {
                        if (nodes[i] is System.Xml.Linq.XElement)
                        {
                            element_ = (System.Xml.Linq.XElement)nodes[i];
                            switch (element_.Name.ToString())
                            {
                                case "Variable":
                                    {
                                        bool IsSlider = false;
                                        foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                        {
                                            if (attr.Name == "Slider")
                                                IsSlider = true;
                                        }
                                        if (IsSlider)
                                        {
                                            Dictionary<string, string> sliderValues = new Dictionary<string, string>();
                                            sliderValues.Add("Name", element_.Attribute("Name").Value);
                                            bool ContainsValue = false;
                                            foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                            {
                                                if (attr.Name == "Value")
                                                { sliderValues.Add("Value", MathPackage.Main.CalculateString(element_.Attribute("Value").Value, CalculationSetting).ToString()); ContainsValue = true; }
                                                else if (attr.Name == "Slider")
                                                {
                                                    LNode a = MainFunctions.ParseExprs(element_.Attribute("Slider").Value);
                                                    if (a.Calls((Symbol)"'{}"))
                                                    {
                                                        // Getting the values of the slider
                                                        for (int ii = 0; ii < a.Args.Count; ii++)
                                                        {
                                                            if (a.Args[ii].Calls(CodeSymbols.Assign))
                                                            {
                                                                string value = "";
                                                                if (a.Args[ii].Args[1].IsId)
                                                                {
                                                                    value = a.Args[ii].Args[1].ToString();
                                                                    value = value.Remove(value.Length - 1, 1);
                                                                }
                                                                else
                                                                    value = MathPackage.Main.CalculateLNode(a.Args[ii].Args[1], CalculationSetting).ToString();
                                                                if (a.Args[ii].Args[0].IsId)
                                                                {
                                                                    sliderValues.Add(a.Args[ii].Args[0].Name.Name, value);
                                                                }
                                                                else
                                                                    throw new Exception($"Slider can not have this value.\n{element_.Attribute("Slider").Value}");
                                                            }
                                                            else
                                                            {
                                                                string value = "";
                                                                value = a.Args[ii].ToString();
                                                                value = value.Remove(value.Length - 1, 1);
                                                                switch (ii)
                                                                {
                                                                    case 0:
                                                                        sliderValues.Add("Start", MathPackage.Main.CalculateLNode(a.Args[ii], CalculationSetting).ToString());
                                                                        break;
                                                                    case 1:
                                                                        sliderValues.Add("End", MathPackage.Main.CalculateLNode(a.Args[ii], CalculationSetting).ToString());
                                                                        break;
                                                                    case 2:
                                                                        sliderValues.Add("Increament", MathPackage.Main.CalculateLNode(a.Args[ii], CalculationSetting).ToString());
                                                                        break;
                                                                    case 3:
                                                                        sliderValues.Add("Oscillation", MathPackage.Main.CalculateLNode(a.Args[ii], CalculationSetting).ToString());
                                                                        break;
                                                                    case 4:
                                                                        sliderValues.Add("Speed", MathPackage.Main.CalculateLNode(a.Args[ii], CalculationSetting).ToString());
                                                                        break;
                                                                    default:
                                                                        throw new Exception($"Your argumments for the slider is not valid.\n{element_.Attribute("Slider").Value}");
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                        throw new Exception($"Slider can not have this value.\n{element_.Attribute("Slider").Value}");
                                                }
                                            }
                                            if (!ContainsValue)
                                            {
                                                sliderValues.Add("Value", "0");
                                            }
                                            AddSlider(sliderValues);
                                        }
                                        else
                                        {
                                            // < Variable Name = "a" Value = "1.8" />
                                            Variable var = new Variable((Symbol)element_.Attribute("Name").Value, new MathPackage.Operations.Constant(0));
                                            foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                            {
                                                if (attr.Name == "Value")
                                                    var.Value = MathPackage.Transformer.GetNodeFromString(element_.Attribute("Value").Value);
                                            }
                                            CalculationSetting.Vars.Add(var);
                                            Sketch.SketchControl.AddGraphControl(var);
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    for (int i = 0; i < nodes.Count(); i++)
                    {
                        if (nodes[i] is System.Xml.Linq.XElement)
                        {
                            element_ = (System.Xml.Linq.XElement)nodes[i];
                            LNode points;
                            switch (element_.Name.ToString())
                            {
                                case "Func":
                                    {  // < Variable Name = "a" Value = "1.8" />
                                        MathPackage.Operations.Func func = new MathPackage.Operations.Func((Symbol)element_.Attribute("Name").Value, null, MathPackage.Transformer.GetNodeFromString(element_.Attribute("Process").Value));
                                        foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
                                        {
                                            if (attr.Name == "Args")
                                            {
                                                // Add the arguments here
                                                LNode args = MainFunctions.ParseExprs(attr.Value);
                                                List<Symbol> argsList = new List<Symbol>();
                                                if (args.Calls((Symbol)"'{}"))
                                                {
                                                    foreach (LNode arg in args.Args)
                                                    {
                                                        if (arg.IsId)
                                                        {
                                                            argsList.Add(arg.Name);
                                                        }
                                                        else
                                                            throw new Exception("Sorry, there was a problem on loading?");
                                                    }
                                                    func.Args = argsList;
                                                }
                                                else
                                                    throw new Exception($"Properity \"Args\" has invalid value \"{attr.Value}\"");
                                            }
                                        }
                                        CalculationSetting.Funcs.Add(func);
                                        Sketch.SketchControl.AddGraphControl(func);

                                    }
                                    break;
                                case "XFunction":
                                    {
                                        XFunction f = new XFunction(this);
                                        AddGraphObjectAttributes(element_, f);
                                        foreach (System.Xml.Linq.XElement value in element_.Nodes())
                                        {
                                            if (value.Name == "Pen")
                                            {
                                                f.Pen = MainFunctions.GetPenFromString(value.Attribute("Value").Value, CalculationSetting);
                                            }
                                            else if (value.Name == "Expression")
                                            {
                                                LNode expr = MainFunctions.ParseExprs(value.Attribute("Value").Value);
                                                if (!MainFunctions.MathExpression(expr))
                                                    throw new Exception($"Properity Expression with value \"{ value.Attribute("Value").Value }\" is not valid.");

                                                f.Expression = MathPackage.Transformer.GetNodeFromLoycNode(expr);
                                            }
                                        }
                                        Sketch.SketchControl.AddGraphControl(f);
                                        Sketch.SketchControl.Sketch.Objects.Add(f);
                                    }
                                    break;
                                case "XYFunction":
                                    {
                                        XYFunction f = new XYFunction(this);
                                        AddGraphObjectAttributes(element_, f);
                                        foreach (System.Xml.Linq.XElement value in element_.Nodes())
                                        {
                                            if (value.Name == "Pen")
                                            {
                                                f.Pen = MainFunctions.GetPenFromString(value.Attribute("Value").Value, CalculationSetting);
                                            }
                                            else if (value.Name == "Expression")
                                            {
                                                LNode expr = MainFunctions.ParseExprs(value.Attribute("Value").Value);
                                                if (!MainFunctions.MathExpression(expr))
                                                    throw new Exception($"Properity Expression with value \"{ value.Attribute("Value").Value }\" is not valid.");

                                                f.Expression = MathPackage.Transformer.GetNodeFromLoycNode(expr);
                                            }
                                        }
                                        Sketch.SketchControl.AddGraphControl(f);
                                        Sketch.SketchControl.Sketch.Objects.Add(f);
                                    }
                                    break;
                                case "ParametricFunc":
                                    {
                                        ParametricFunc f = new ParametricFunc(this);
                                        AddGraphObjectAttributes(element_, f);
                                        LNode expr;
                                        foreach (System.Xml.Linq.XElement value in element_.Nodes())
                                        {
                                            if (value.Name == "Pen")
                                            {
                                                f.Pen = MainFunctions.GetPenFromString(value.Attribute("Value").Value, CalculationSetting);
                                            }
                                            else if (value.Name == "Step")
                                            {
                                                expr = MainFunctions.ParseExprs(value.Attribute("Value").Value);
                                                if (!MainFunctions.MathExpression(expr))
                                                    throw new Exception($"Properity Expression with value \"{ value.Attribute("Value").Value }\" is not valid.");
                                                f.Step = MathPackage.Transformer.GetNodeFromLoycNode(expr);
                                            }
                                            else if (value.Name == "Parameter")
                                            {
                                                expr = MainFunctions.ParseExprs(element_.Element("Parameter").Attribute("Value").Value);
                                                if (!expr.IsId)
                                                    throw new Exception($"Properity Expression with value \"{ expr.ToString().Substring(0, expr.ToString().Length - 1) }\" is not valid.");
                                                f.Parameter = expr.Name;
                                            }
                                            else if (value.Name == "Start")
                                            {
                                                expr = MainFunctions.ParseExprs(element_.Element("Start").Attribute("Value").Value);
                                                if (!MainFunctions.MathExpression(expr))
                                                    throw new Exception($"Properity Expression with value \"{ expr.ToString().Substring(0, expr.ToString().Length - 1) }\" is not valid.");
                                                f.Start = MathPackage.Transformer.GetNodeFromLoycNode(expr);
                                            }
                                            else if (value.Name == "End")
                                            {
                                                expr = MainFunctions.ParseExprs(element_.Element("End").Attribute("Value").Value);
                                                if (!MainFunctions.MathExpression(expr))
                                                    throw new Exception($"Properity Expression with value \"{ expr.ToString().Substring(0, expr.ToString().Length - 1) }\" is not valid.");
                                                f.End = MathPackage.Transformer.GetNodeFromLoycNode(expr);
                                            }
                                        }
                                        {
                                            expr = MainFunctions.ParseExprs(element_.Element("x_Expression").Attribute("Value").Value);
                                            if (!MainFunctions.MathExpression(expr))
                                                throw new Exception($"Properity Expression with value \"{ expr.ToString().Substring(0, expr.ToString().Length - 1) }\" is not valid.");
                                            f.x_Expression = MathPackage.Transformer.GetNodeFromLoycNode(expr);
                                            expr = MainFunctions.ParseExprs(element_.Element("y_Expression").Attribute("Value").Value);
                                            if (!MainFunctions.MathExpression(expr))
                                                throw new Exception($"Properity Expression with value \"{ expr.ToString().Substring(0, expr.ToString().Length - 1) }\" is not valid.");
                                            f.y_Expression = MathPackage.Transformer.GetNodeFromLoycNode(expr);
                                        }
                                        Sketch.SketchControl.AddGraphControl(f);
                                        Sketch.SketchControl.Sketch.Objects.Add(f);
                                    }
                                    break;
                                case "Point":
                                    {
                                        Points p = new Points(this);
                                        AddGraphObjectAttributes(element_, p);
                                        p.X_Value = MathPackage.Transformer.GetNodeFromString(element_.Element("x").Attribute("Value").Value);
                                        p.Y_Value = MathPackage.Transformer.GetNodeFromString(element_.Element("y").Attribute("Value").Value);
                                        foreach (System.Xml.Linq.XElement value in element_.Nodes())
                                        {
                                            if (value.Name == "Color")
                                            {
                                                p.Pen.Color = MainFunctions.GetColorFromString(value.Attribute("Value").Value, CalculationSetting);
                                            }
                                            else if (value.Name == "Shape")
                                            {
                                                switch (value.Attribute("Value").Value)
                                                {
                                                    case "Solid":
                                                        p.Shape = Points.PointShape.SolidCircle;
                                                        break;
                                                    case "Shallow":
                                                        p.Shape = Points.PointShape.ShallowCircle;
                                                        break;
                                                    case "Square":
                                                        p.Shape = Points.PointShape.Square;
                                                        break;
                                                    case "Cross":
                                                        p.Shape = Points.PointShape.Cross;
                                                        break;
                                                }
                                            }
                                            else if (value.Name == "Size")
                                            {
                                                p.Size = int.Parse(value.Attribute("Value").Value);
                                            }
                                        }
                                        Sketch.SketchControl.AddGraphControl(p);
                                        Sketch.Objects.Add(p);
                                    }
                                    break;
                                default:
                                    {
                                        if (IsPD(element_.Name.ToString()))
                                        {

                                            points = MainFunctions.ParseExprs(element_.Element("Points").Attribute("Value").Value);
                                            switch (element_.Name.ToString())
                                            {
                                                case "Circle":
                                                    if (points.Args.Count == 3)
                                                    {
                                                        ThreePCircle obj = new ThreePCircle(this);
                                                        AddGraphObjectAttributes(element_, obj);
                                                        obj.Pen = MainFunctions.GetPenFromString(element_.Element("Pen").Attribute("Value").Value, CalculationSetting);
                                                        Sketch.SketchControl.AddGraphControl(obj);
                                                        Sketch.Objects.Add(obj);
                                                    }
                                                    else if (points.Args.Count == 2)
                                                    {
                                                        TwoPCircle obj = new TwoPCircle(this);
                                                        AddGraphObjectAttributes(element_, obj);
                                                        obj.Pen = MainFunctions.GetPenFromString(element_.Element("Pen").Attribute("Value").Value, CalculationSetting);
                                                        Sketch.SketchControl.AddGraphControl(obj);
                                                        Sketch.Objects.Add(obj);
                                                    }
                                                    else throw new Exception($"Your object  is not supported. \n {element_.ToString()}");
                                                    break;
                                                case "Circle2":
                                                    if (points.Args.Count == 2)
                                                    {
                                                        TwoPCircle2 obj = new TwoPCircle2(this);
                                                        AddGraphObjectAttributes(element_, obj);
                                                        obj.Pen = MainFunctions.GetPenFromString(element_.Element("Pen").Attribute("Value").Value, CalculationSetting);
                                                        Sketch.SketchControl.AddGraphControl(obj);
                                                        Sketch.Objects.Add(obj);
                                                    }
                                                    else throw new Exception($"Your object  is not supported. \n {element_.ToString()}");
                                                    break;
                                                case "SemiCircle":
                                                    if (points.Args.Count == 2)
                                                    {
                                                        TwoPSemiCircle obj = new TwoPSemiCircle(this);
                                                        AddGraphObjectAttributes(element_, obj);
                                                        obj.Pen = MainFunctions.GetPenFromString(element_.Element("Pen").Attribute("Value").Value, CalculationSetting);
                                                        Sketch.SketchControl.AddGraphControl(obj);
                                                        Sketch.Objects.Add(obj);
                                                    }
                                                    else throw new Exception($"Your object  is not supported. \n {element_.ToString()}");
                                                    break;
                                                case "SemiCircle2":
                                                    if (points.Args.Count == 2)
                                                    {
                                                        TwoPSemiCircle2 obj = new TwoPSemiCircle2(this);
                                                        AddGraphObjectAttributes(element_, obj);
                                                        obj.Pen = MainFunctions.GetPenFromString(element_.Element("Pen").Attribute("Value").Value, CalculationSetting);
                                                        Sketch.SketchControl.AddGraphControl(obj);
                                                        Sketch.Objects.Add(obj);
                                                    }
                                                    else throw new Exception($"Your object  is not supported. \n {element_.ToString()}");
                                                    break;
                                                case "Line":
                                                    if (points.Args.Count == 2)
                                                    {
                                                        Line obj = new Line(this);
                                                        AddGraphObjectAttributes(element_, obj);
                                                        obj.Pen = MainFunctions.GetPenFromString(element_.Element("Pen").Attribute("Value").Value, CalculationSetting);
                                                        Sketch.SketchControl.AddGraphControl(obj);
                                                        Sketch.Objects.Add(obj);
                                                    }
                                                    else throw new Exception($"Your object  is not supported. \n {element_.ToString()}");
                                                    break;
                                                case "Angle":
                                                    if (points.Args.Count == 3)
                                                    {
                                                        Angle obj = new Angle(this);
                                                        AddGraphObjectAttributes(element_, obj);
                                                        obj.Pen = MainFunctions.GetPenFromString(element_.Element("Pen").Attribute("Value").Value, CalculationSetting);
                                                        Sketch.SketchControl.AddGraphControl(obj);
                                                        Sketch.Objects.Add(obj);
                                                    }
                                                    else throw new Exception($"Your object  is not supported. \n {element_.ToString()}");
                                                    break;
                                                case "Distance":
                                                    if (points.Args.Count == 2)
                                                    {
                                                        Distance obj = new Distance(this);
                                                        AddGraphObjectAttributes(element_, obj);
                                                        obj.Pen = MainFunctions.GetPenFromString(element_.Element("Pen").Attribute("Value").Value, CalculationSetting);
                                                        Sketch.SketchControl.AddGraphControl(obj);
                                                        Sketch.Objects.Add(obj);
                                                    }
                                                    else throw new Exception($"Your object  is not supported. \n {element_.ToString()}");
                                                    break;
                                                case "Polygone":
                                                    if (points.Args.Count > 2)
                                                    {
                                                        Polygone obj = new Polygone(this);
                                                        AddGraphObjectAttributes(element_, obj);
                                                        obj.Pen = MainFunctions.GetPenFromString(element_.Element("Pen").Attribute("Value").Value, CalculationSetting);
                                                        Sketch.SketchControl.AddGraphControl(obj);
                                                        Sketch.Objects.Add(obj);
                                                    }
                                                    else throw new Exception($"Your object  is not supported. \n {element_.ToString()}");
                                                    break;
                                                default:
                                                    throw new Exception($"Your object  is not supported. \n {element_.ToString()}");

                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                    }

                    #endregion

                    #region adding pointsdependant to points and otherwise
                    for (int i = 0; i < nodes.Count(); i++)
                    {
                        if (nodes[i] is System.Xml.Linq.XElement)
                        {
                            element_ = (System.Xml.Linq.XElement)nodes[i];
                            if (IsPD(element_.Name.ToString()))
                            {
                                // Getting PointsDependant by name from Sketch
                                PointsDependant pd = null;
                                foreach (GraphObject pd_ in Sketch.Objects.ToArray())
                                {
                                    if (pd_.Name == element_.Attribute("Name").Value)
                                    {
                                        pd = (PointsDependant)pd_;
                                    }
                                }
                                if (pd == null)
                                    throw new Exception("Sorry, there was a problem on loading?");
                                // Add the points here
                                LNode points = MainFunctions.ParseExprs(element_.Element("Points").Attribute("Value").Value);
                                if (points.Calls((Symbol)"'{}"))
                                {
                                    foreach (LNode point in points.Args)
                                    {
                                        if (point.IsId)
                                        {
                                            Points p = (Points)GetObjByName(point.Name.Name);
                                            p.Dependants.Add(pd);
                                            pd.Points.Add(p);
                                        }
                                        else if (point.Calls(CodeSymbols.Tuple))
                                        {
                                            pd.Points.Add(MainFunctions.GetPointFromTuple(point, this));
                                        }
                                        else
                                            throw new Exception("Sorry, there was a problem on loading?");
                                    }
                                    pd.Control.Script.Text = pd.ToString();
                                }
                                else
                                    throw new Exception($"Properity \"Points\" has invalid value");
                            }
                        }
                    }
                    #endregion

                }
            }

            MyState = State.Ready;
        }
        /// <param name="name">the type name e.g.:   "Circle" ,Circle2, Angle,.....  </param>
        bool IsPD(string name)
        {
            if (MainFunctions.PDsTypes().Contains(name))
            {
                return true;
            }
            return false;
        }
        Point pointSliderPosition = new Point(0, 0);
        void AddSlider(Dictionary<string, string> values)
        {
            Slider slider = new Slider(values["Name"], this);
            for (int i = 0; i < values.Count; i++)
            {
                switch (values.ElementAt(i).Key)
                {
                    case "Start":
                        slider.Interval_start = (double.Parse(values["Start"]));
                        break;
                    case "End":
                        slider.Interval_end = (double.Parse(values["End"]));
                        break;
                    case "Increament":
                        slider.Increament = (double.Parse(values["Increament"]));
                        break;
                    case "Oscillation":
                        switch (values["Oscillation"])
                        {
                            case "UpDown":
                                slider.OscillationType_ = Slider.OscillationType.UpDown;
                                break;
                            case "UpOnce":
                                slider.OscillationType_ = Slider.OscillationType.UpOnce;
                                break;
                            case "DownOnce":
                                slider.OscillationType_ = Slider.OscillationType.DownOnce;
                                break;
                            case "Up":
                                slider.OscillationType_ = Slider.OscillationType.Up;
                                break;
                            case "Down":
                                slider.OscillationType_ = Slider.OscillationType.Down;
                                break;
                            default:
                                throw new Exception($"The attribute {values.ElementAt(i).Key} is not avaliable in Slider");
                        }
                        break;
                    case "Speed":
                        slider.Timer.Interval = (int.Parse(values["Speed"]));
                        break;
                    case "Name":
                        break;
                    case "Value":
                        break;
                    default:
                        throw new Exception($"The attribute {values.ElementAt(i).Key} is not avaliable in Slider");
                }
            }
            slider.ModifyValues();
            slider.SetValue(double.Parse(values["Value"]));
            Sketch.Sliders.Add(slider);
            GetSliderPosition(slider.SliderControl);
            Sketch.SketchControl.GraphPanel.Controls.Add(slider.SliderControl);
        }
        void GetSliderPosition(System.Windows.Forms.Control control)
        {
            control.Location = pointSliderPosition;
            if (pointSliderPosition.X + control.Width > Width)
            {
                if (pointSliderPosition.Y + control.Height > Height)
                {
                    pointSliderPosition.Y = 0;
                    pointSliderPosition.X = 0;
                }
                else
                {
                    pointSliderPosition.Y += control.Height;
                    pointSliderPosition.X = 0;
                }
            }
            else
                pointSliderPosition.X += control.Width;
        }
        void AddGraphObjectAttributes(System.Xml.Linq.XElement element_, GraphObject obj)
        {
            bool NameFound = false;
            foreach (System.Xml.Linq.XAttribute attr in element_.Attributes())
            {
                if (attr.Name == "Name")
                {
                    obj.SetName(attr.Value);
                    NameFound = true;
                }
                else if (attr.Name == "Visible")
                {
                    obj.Visible = GetBool(attr.Value);
                }
            }
            if (!NameFound)
            {
                string name = SelectName();
                obj.SetName(name);
                element_.SetAttributeValue("Name", name);
            }
        }
        public void Get_coor_setting(System.Xml.Linq.XElement xml)
        {
            foreach (System.Xml.Linq.XElement element in xml.Nodes())
            {
                switch (element.Name.ToString())
                {
                    case "Number_color":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Number_color = MainFunctions.GetColorFromString(attr.Value, CalculationSetting);
                            }
                        }
                        break;
                    case "Number_font":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Number_font = MainFunctions.GetFontFromString(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "AutoRefiningOnZoomimg":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.AutoRefiningOnZoomimg = GetBool(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "BackColor":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.BackColor = MainFunctions.GetColorFromString(element.Attribute("Value").Value, CalculationSetting);
                            }
                        }
                        break;
                    case "Polar_circle_space":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Polar_circle_space = number.Parse(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "Polar_line_space":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Polar_line_space = number.Parse(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "Decimal_space_x":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Decimal_space_x = int.Parse(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "Decimal_space_y":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Decimal_space_y = int.Parse(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "Number_space_x":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Number_space_x = int.Parse(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "Number_space_y":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Number_space_y = int.Parse(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "Type":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Type = (Graphing.Objects.CoorSetting.CoorType)int.Parse(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "Coor_pen":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Coor_pen = MainFunctions.GetPenFromString(element.Attribute("Value").Value, CalculationSetting);
                            }
                        }
                        break;
                    case "Decimal_Pen":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Decimal_Pen = MainFunctions.GetPenFromString(element.Attribute("Value").Value, CalculationSetting);
                            }
                        }
                        break;
                    case "Axises_pen":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Axises_pen = MainFunctions.GetPenFromString(element.Attribute("Value").Value, CalculationSetting);
                            }
                        }
                        break;
                    case "PolarLines_pen":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.PolarLines_pen = MainFunctions.GetPenFromString(element.Attribute("Value").Value, CalculationSetting);
                            }
                        }
                        break;
                    case "PolarCircles_pen":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.PolarCircles_pen = MainFunctions.GetPenFromString(element.Attribute("Value").Value, CalculationSetting);
                            }
                        }
                        break;
                    case "X_unit":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.X_unit = element.Attribute("Value").Value;
                            }
                        }
                        break;
                    case "Y_unit":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.Y_unit = element.Attribute("Value").Value;
                            }
                        }
                        break;
                    case "DrawNumbers":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.DrawNumbers = GetBool(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "DrawCoor":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.DrawCoor = GetBool(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "DrawDecimal":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.DrawDecimal = GetBool(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "DrawAxises":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.DrawAxises = GetBool(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "DrawPolarAngles":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.DrawPolarAngles = GetBool(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "DrawPolarCircles":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.DrawPolarCircles = GetBool(element.Attribute("Value").Value);
                            }
                        }
                        break;
                    case "DrawPolarLines":
                        foreach (System.Xml.Linq.XAttribute attr in element.Attributes())
                        {
                            if (attr.Name == "Value")
                            {
                                Sketch.Coordinates.CoorSetting.DrawPolarLines = GetBool(element.Attribute("Value").Value);
                            }
                        }
                        break;
                }
            }
        }
        private static bool GetBool(string str)
        {
            if (str.ToLower() == "true")
                return true;
            else
                return false;
        }

        #endregion

    }
}
