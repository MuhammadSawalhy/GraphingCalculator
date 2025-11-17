using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using Graphing.Objects;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Graphing.Controls
{
    public partial class SketchControl : UserControl
    {

        bool loaded = false;
        public SketchControl(Sketch sketch)
        {

            // Add any initialization after the InitializeComponent() call.

            /// This wil also set the GraphSetting {Property} value to the one inside this Sketch.
            Sketch = sketch;
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            MainBitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            SubBitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);

            InitializeComponent();
            GraphPanel.MouseWheel += GraphPanel_MouseWheel;
            loaded = true;

        }

        #region "Varaibles"
      
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
        public void FlushMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                    Process[] myProcesses = Process.GetProcessesByName("ApplicationName");
                    //Dim ProcessInfo As Process
                    foreach (Process myProcess in myProcesses)
                    {
                        SetProcessWorkingSetSize(myProcess.Handle, -1, -1);
                    }
                }

            }
            catch { }
        }

        public Bitmap Bitmap;
        public Bitmap SubBitmap;
        public Bitmap MainBitmap;
        public Points SelectedPoint;
        public Function SelectedFunc;
        public PointsDependant SelectedPD;
        private PointsDependant pointDependant;
        private Pen_ pen_;
        private Pen pens_pen = new Pen(Color.FromArgb(62, 62, 66), 2);
        Point DragPoint;
        
        /// <summary>
        /// The value of this <see cref="Graphing.GraphSetting"> will be set when you set the value of <see cref="Sketch">
        /// </summary>
        public GraphSetting GraphSetting;

        Sketch sketch_;

        public Sketch Sketch
        {
            get
            {
                return sketch_;
            }
            set
            {
                sketch_ = value;
                GraphSetting = sketch_.GraphSetting;
            }
        }

        public void Draw()
        {
            Create_Graphics(sketch_.Bitmap);
        }

        #endregion

        public bool Is_snap_to_int_x(int Location_x)
        {
            if (GraphSetting.snap_int)
            {
                if (MathPackage.Main.Approximate((Location_x - GraphSetting.Center.X) / (double)GraphSetting.X_Stretch, "9", "0", "###") % 1 == 0)
                    return true;
            }
            return false;
        }

        public bool Is_snap_to_int_y(int Location_y)
        {
            if (GraphSetting.snap_int)
            {
                if (MathPackage.Main.Approximate((GraphSetting.Center.Y - Location_y) / (double)GraphSetting.Y_Stretch, "9", "0", "###") % 1 == 0)
                    return true;
            }
            return false;
        }

        private void GraphPanel_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            for(int i = 0; i <= 0; i++)
            {
                if (e.Delta > 0)
                    GraphSetting.ZoomIn(e.Location);
                else
                    GraphSetting.ZoomOut(e.Location);

                Draw();
            }
            FlushMemory();
        }

        private void Create_Graphics(Bitmap Bitmap_)
        {
            GraphPanel.CreateGraphics().DrawImage(Bitmap_, 0, 0);
            MainBitmap = Bitmap_;
        }

        private void GraphPanel_Paint(object sender, PaintEventArgs e)
        {
            try
            {
            GraphPanel.CreateGraphics().DrawImage(MainBitmap, 0, 0);
            }
            catch
            {

            }
       }

        public void Remove_MouseUP_Events()
        {
            GraphPanel.MouseUp -= MouseUp_PD;
            GraphPanel.MouseUp -= MouseUp_Pen;
            GraphPanel.MouseUp -= MouseUp_Delete;
            GraphPanel.MouseUp -= MouseUp_DragCoor;
            GraphPanel.MouseUp -= MouseUp_DragPoint;
        }

        public void Remove_MouseDown_Events()
        {
            GraphPanel.MouseDown -= MouseDown_;
            GraphPanel.MouseDown -= MouseDown_CreatePD;
            GraphPanel.MouseDown -= MouseDown_Pen;
            GraphPanel.MouseDown -= MouseDown_Delete;
            GraphPanel.MouseDown -= MouseDown_Slider;
            GraphPanel.MouseDown -= MouseDown_AddPoints;
        }

        public void Remove_MouseMove_Events()
        {
            GraphPanel.MouseMove -= MouseMove_;
            BW.RunWorkerCompleted -= BW_RunWorkerCompleted;
            BW.DoWork -= BW_Delete;
            BW.DoWork -= BW_DragCoor;
            BW.DoWork -= BW_DragPoint;
            BW.DoWork -= BW_Pen;
            BW.DoWork -= BW_SetPD;
        }

        public void Balance(bool AddMouseDownEvent)
        {
            Remove_MouseDown_Events();
            Remove_MouseUP_Events();
            Remove_MouseMove_Events();
            GraphPanel.Cursor = Cursors.Arrow;
            if (AddMouseDownEvent)
                GraphPanel.MouseDown += MouseDown_;
            GraphSetting.IsDelete = false;
        }


        #region "Selecting"
        public Points GetSelectedPoint(Point point)
        {
            foreach (GraphObject point_ in sketch_.Objects.ToArray())
            {
                if (point_.Visible && point_ is Points)
                {
                    if (point.X >= ((Points)point_).Location.X - ((Points)point_).Size && point.X <= ((Points)point_).Location.X + ((Points)point_).Size)
                    {
                        if (point.Y >= ((Points)point_).Location.Y - ((Points)point_).Size && point.Y <= ((Points)point_).Location.Y + ((Points)point_).Size)
                            return ((Points)point_);
                    }
                }
            }
            return null;
        }

        public Function GetSelectedFunc(Point point)
        {
            foreach (GraphObject func in sketch_.Objects.ToArray())
            {
                if (func.Visible && func.Bitmap.GetPixel(point.X, point.Y).A > 0 && func is Function)
                    return (Function)func;
            }
            return null;
        }

        public AuxiliaryPoints GetSelectedAP(Point point)
        {
            foreach (AuxiliaryPoints point_ in sketch_.AuxiliaryPoints.ToArray())
            {
                if (point.X >= point_.Point.X && point.X <= point_.Point.X + point_.Size)
                {
                    if (point.Y >= point_.Point.Y && point.Y <= point_.Point.Y + point_.Size)
                        return point_;
                }
            }
            return null;
        }

        public PointsDependant GetSelectedPD(Point point)
        {
            foreach (GraphObject pd in sketch_.Objects.ToArray())
            {
                if (pd.Visible && pd.Bitmap.GetPixel(point.X, point.Y).A > 0 && pd is PointsDependant)
                    return (PointsDependant)pd;
            }
            return null;
        }

        public void SelectPoint(ref Points Point)
        {
            Point.Is_Selected = true;
            sketch_.UpdateAndDraw();
            Draw();
        }

        public void SelectFunc(Function Func)
        {
            GraphPanel.Cursor = Cursors.Arrow;
            Func.IsSelected = !Func.IsSelected;
            sketch_.UpdateAndDraw();
            Draw();
        }

        public void SelectAP(AuxiliaryPoints AP)
        {
            GraphPanel.Cursor = Cursors.Arrow;
            //Creating and drawing the text of this AuxiliaryPoints

            using (Graphics g = Graphics.FromImage(MainBitmap))
            {
                using (Objects.Text text = new Objects.Text(AP.ToString(), GraphSetting.Font, Color.Black, Color.White, new Point()))
                {
                    SizeF size = text.BoxSize();
                    text.Location = new Point((int)(AP.Point.X - size.Width / 2), (int)(AP.Point.Y - size.Height - 5));
                    text.DrawTo(g);
                    Create_Graphics(MainBitmap);
                }
            }
        }
        #endregion

        public void AddPointToPen(Point point)
        {
            if (PenIsStatic.Checked)
                pen_.Points.Add(point);
            else
                pen_.Points.Add(new PointF((float)(point.X - GraphSetting.Center.X), (float)(point.Y - GraphSetting.Center.Y)));
        }

        private bool NeedRefresh;

        #region "MouseDown"
        Point CurrentPoint;

        private void MouseDown_(object sender, MouseEventArgs e)
        {
            CurrentPoint = e.Location;
            if (!BW.IsBusy)
            {
                Sub_MouseDown_(BW, new RunWorkerCompletedEventArgs(null, null, false));
            }
            else
            {
                Remove_MouseMove_Events();
                BW.RunWorkerCompleted += Sub_MouseDown_;
            }
        }

        private bool SliderTimerEnabeled = false;
        private void Sub_MouseDown_(System.Object sender, RunWorkerCompletedEventArgs e)
        {
            BW.RunWorkerCompleted -= Sub_MouseDown_;
            GraphPanel.Cursor = Cursors.Hand;
            SelectedPoint = GetSelectedPoint(CurrentPoint);
            if (SelectedPoint != null)
            {
                SelectPoint(ref SelectedPoint);
                GraphPanel.MouseUp += MouseUp_DragPoint;
                BW.RunWorkerCompleted += BW_RunWorkerCompleted;
                GraphPanel.MouseMove += MouseMove_;
                BW.DoWork += BW_DragPoint;
                return;
            }
            AuxiliaryPoints AP = GetSelectedAP(CurrentPoint);
            if (AP != null)
            {
                SelectAP(AP);
                return;
            }
            SelectedFunc = GetSelectedFunc(CurrentPoint);
            if (SelectedFunc != null)
            {
                
                if (SelectedFunc is XYFunction)
                {
                    if (((XYFunction)SelectedFunc).Func_Type == XYFunction.FuncType.Equation)
                    { SelectFunc(SelectedFunc); return; }

                } else { SelectFunc(SelectedFunc); return;}
            }
            DragPoint = CurrentPoint;
            sketch_.DrawExceptCoor();

            SliderTimerEnabeled = GraphSetting.SliderTimer.Enabled;
            GraphSetting.SliderTimer.Enabled = false;
            GraphPanel.MouseMove += MouseMove_;
            BW.RunWorkerCompleted += BW_RunWorkerCompleted;
            BW.DoWork += BW_DragCoor;
            GraphPanel.MouseUp += MouseUp_DragCoor;
        }
        //''''''''''''''''

        private void MouseDown_CreatePD(object sender, MouseEventArgs e)
        {
            CurrentPoint = e.Location;
            if (!BW.IsBusy)
            {
                Sub_MouseDown_CreatePD(BW, new RunWorkerCompletedEventArgs(null, null, false));
            }
            else
            {
                Remove_MouseMove_Events();
                BW.RunWorkerCompleted += Sub_MouseDown_CreatePD;
            }
        }
        private void Sub_MouseDown_CreatePD(System.Object sender, RunWorkerCompletedEventArgs e)
        {
            BW.RunWorkerCompleted -= Sub_MouseDown_CreatePD;
            SubBitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            using (Graphics Graphics = Graphics.FromImage(SubBitmap))
            {
                Graphics.SmoothingMode = MainFunctions.Smoothing;
                Graphics.CompositingQuality = MainFunctions.Composition;
                Graphics.DrawImage(Bitmap, 0, 0);
                SelectedPoint = GetSelectedPoint(CurrentPoint);

                // Add new point
                if (SelectedPoint != null)
                {
                    if (pointDependant.Points.Count == 0 || (SelectedPoint.Name != pointDependant.Points.Last().Name && SelectedPoint.Name != null))
                        pointDependant.Points.Add(SelectedPoint);
                }
                else
                {
                    AddPoint(CurrentPoint);
                    pointDependant.Points.Add((Points)sketch_.Objects.ToArray().Last());
                }

                //Drawing the points
                pointDependant.Points.Last().Is_Selected = true;
                if (pointDependant.Points.Count >= pointDependant.PointNumberAtLeast)
                {
                    pointDependant.Draw();
                    pointDependant.DrawTo(Graphics);
                    for (int i = 0; i < pointDependant.Points.Count; i++)
                    {
                        pointDependant.Points[i].Draw();
                        pointDependant.Points[i].DrawTo(Graphics);
                    }
                }
                else
                {
                    for (int i = 0; i < pointDependant.Points.Count; i++)
                    {
                        pointDependant.Points[i].Draw();
                        pointDependant.Points[i].DrawTo(Graphics);
                    }
                }

                Create_Graphics(SubBitmap);

                if (pointDependant.Points.Count == pointDependant.PointNumberAtMost)
                {
                    GraphPanel.MouseUp += MouseUp_PD;
                    return;
                }
                if (pointDependant.PointNumberAtMost == 0)
                {
                    GraphPanel.MouseUp += MouseUp_PD;
                    GraphPanel.MouseMove += MouseMove_;
                    BW.DoWork += BW_SetPD;
                    BW.RunWorkerCompleted += BW_RunWorkerCompleted;
                    return;
                }
                if (pointDependant.Points.Count >= pointDependant.PointNumberAtLeast - 1)
                {
                    GraphPanel.MouseMove += MouseMove_;
                    BW.DoWork += BW_SetPD;
                    BW.RunWorkerCompleted += BW_RunWorkerCompleted;
                }

            }
        }
        //''''''''''''''''

        private void MouseDown_Pen(object sender, MouseEventArgs e)
        {
            CurrentPoint = e.Location;
            if (!BW.IsBusy)
            {
                Sub_MouseDown_Pen(BW, new RunWorkerCompletedEventArgs(null, null, false));
            }
            else
            {
                Remove_MouseMove_Events();
                BW.RunWorkerCompleted += Sub_MouseDown_Pen;
            }
        }
        private void Sub_MouseDown_Pen(System.Object sender, RunWorkerCompletedEventArgs e)
        {
            BW.RunWorkerCompleted -= Sub_MouseDown_Pen;
            switch (PenIsStatic.Checked)
            {
                case true:
                    {
                        pen_ = new StaticPen(GraphSetting);
                        break;
                    }

                case false:
                    {
                        pen_ = new DinamicPen(GraphSetting);
                        break;
                    }
            }
            pen_.Pen = pens_pen;
            AddPointToPen(CurrentPoint);
            GraphPanel.Cursor = new Cursor(Properties.Resources.Pen_icon_active.Handle);
            GraphPanel.MouseMove += MouseMove_;
            BW.DoWork += BW_Pen;
            GraphPanel.MouseUp += MouseUp_Pen;
        }
        //''''''''''''''''

        private void MouseDown_Slider(object sender, MouseEventArgs e)
        {
            GraphPanel.MouseDown -= MouseDown_Slider;

            CurrentPoint = e.Location;
            if (!BW.IsBusy)
            {
                Sub_MouseDown_Slider(BW, new RunWorkerCompletedEventArgs(null, null, false));
            }
            else
            {
                Remove_MouseMove_Events();
                BW.RunWorkerCompleted += Sub_MouseDown_Slider;
            }

        }
        private void Sub_MouseDown_Slider(System.Object sender, RunWorkerCompletedEventArgs e)
        {
            BW.RunWorkerCompleted -= Sub_MouseDown_Slider;
            // '

            Point p;
            if (CurrentPoint.X + 176 > GraphPanel.Width)
            {
                if (CurrentPoint.Y + 62 > GraphPanel.Height)
                    p = new Point(GraphPanel.Width - 176, GraphPanel.Height - 62);
                else
                    p = new Point(GraphPanel.Width - 176, CurrentPoint.Y);
            }
            else if (CurrentPoint.Y + 62 > GraphPanel.Height)
                p = new Point(CurrentPoint.X, GraphPanel.Height - 62);
            else
                p = CurrentPoint;
            Slider_Form form = new Slider_Form(p, GraphSetting);
            form.ShowDialog();

            // '

            GraphPanel.MouseDown += MouseDown_;

        }
        //''''''''''''''''

        private void MouseDown_Delete(object sender, MouseEventArgs e)
        {
            CurrentPoint = e.Location;
            if (!BW.IsBusy)
            {
                Sub_MouseDown_Delete(BW, new RunWorkerCompletedEventArgs(null, null, false));
            }
            else
            {
                Remove_MouseMove_Events();
                BW.RunWorkerCompleted += Sub_MouseDown_Delete;
            }
        }
        private void Sub_MouseDown_Delete(System.Object sender, RunWorkerCompletedEventArgs e)
        {
            BW.RunWorkerCompleted -= Sub_MouseDown_Delete;
            GraphPanel.MouseMove += MouseMove_;
            BW.DoWork += BW_Delete;

            GraphPanel.MouseUp += MouseUp_Delete;

            GraphPanel.Cursor = new Cursor(Properties.Resources.Erase_icon_active.Handle);

            Delete(CurrentPoint);

        }
        //''''''''''''''''

        private void MouseDown_AddPoints(object sender, MouseEventArgs e)
        {
            CurrentPoint = e.Location;
            if (!BW.IsBusy)
            {
                Sub_MouseDown_AddPoints(BW, new RunWorkerCompletedEventArgs(null, null, false));
            }
            else
            {
                Remove_MouseMove_Events();
                BW.RunWorkerCompleted += Sub_MouseDown_AddPoints;
            }
        }
        private void Sub_MouseDown_AddPoints(System.Object sender, RunWorkerCompletedEventArgs e)
        {
            BW.RunWorkerCompleted -= Sub_MouseDown_AddPoints;
            SelectedPoint = GetSelectedPoint(CurrentPoint);
            if (SelectedPoint == null)
            {
                AddPoint(CurrentPoint);
                sketch_.Objects.ToArray().Last().Draw();
                sketch_.Draw();
                Draw();
            }
            else
            {
                SelectPoint(ref SelectedPoint);
                SelectedPoint.Is_Selected = false;
            }
        }
        //''''''''''''''''

        public bool IsPointInside(Point point)
        {
            if (point.X < 0 || point.Y < 0)
                return false;
            if (point.X > GraphSetting.Width || point.Y > GraphSetting.Height)
                return false;

            return true;
        }

        public void Delete(Point Point)
        {
            // 
            // Select objects And delete the select
            // 

            if (IsPointInside(Point))
            {
                foreach (GraphObject obj in sketch_.Objects.ToArray())
                {
                    if (obj.Bitmap.GetPixel(Point.X, Point.Y).A > 0)
                    {
                        if (obj.Visible)
                        {
                            obj.Delete(true);
                            sketch_.UpdateAndDraw();
                            Draw();
                            return;
                        }
                    }
                }

                foreach (DinamicPen pen in sketch_.Pens.DinamicPenArray)
                {
                    if (pen.Bitmap.GetPixel(Point.X, Point.Y).A > 0)
                    {
                        if (pen.Visible)
                        {
                            pen.Delete();
                            sketch_.UpdateAndDraw();
                            Draw();
                            return;
                        }
                    }
                }

                Bitmap b;
                Graphics g;
                foreach (StaticPen pen in sketch_.Pens.StaticPenArray)
                {
                    if (pen.Visible)
                    {
                        b = new Bitmap(GraphSetting.Width, GraphSetting.Height);
                        g = Graphics.FromImage(b);
                        pen.DrawTo(g);

                        if (b.GetPixel(Point.X, Point.Y).A > 0)
                        {
                            pen.Delete();
                            sketch_.UpdateAndDraw();
                            Draw();
                            return;
                        }

                    }

                }
            }
        }

        public void PutPointIntoCurve()
        {
        }

        public void SetPointCoor(Points p, Point location_)
        {
            if (GraphSetting.snap_int)
            {
                if (Is_snap_to_int_x(location_.X))
                    p.X_Value = new MathPackage.Operations.Constant(Math.Round(GraphSetting.xChangeToCoor(location_.X, location_.Y)));
                else if (GraphSetting.snap_coor)
                    p.X_Value = new MathPackage.Operations.Constant(MathPackage.Main.Approximate((location_.X - GraphSetting.Center.X) / (double)(GraphSetting.X_Space), "9", "0", "###") * (GraphSetting.X_Space) / (double)GraphSetting.X_Stretch);
                else
                    p.X_Value = new MathPackage.Operations.Constant(GraphSetting.xChangeToCoor(location_.X, location_.Y));
                if (Is_snap_to_int_y(location_.Y))
                    p.Y_Value = new MathPackage.Operations.Constant(Math.Round(GraphSetting.yChangeToCoor(location_.X, location_.Y)));
                else if (GraphSetting.snap_coor)
                    p.Y_Value = new MathPackage.Operations.Constant(MathPackage.Main.Approximate((GraphSetting.Center.Y - location_.Y) / (double)(GraphSetting.Y_Space), "9", "0", "###") * (GraphSetting.Y_Space) / (double)GraphSetting.Y_Stretch);
                else
                    p.Y_Value = new MathPackage.Operations.Constant(GraphSetting.yChangeToCoor(location_.X, location_.Y));
            }
            else if (GraphSetting.snap_coor)
            {
                p.X_Value = new MathPackage.Operations.Constant(MathPackage.Main.Approximate((location_.X - GraphSetting.Center.X) / (double)(GraphSetting.X_Space), "9", "0", "###") * (GraphSetting.X_Space) / (double)GraphSetting.X_Stretch);
                p.Y_Value = new MathPackage.Operations.Constant(MathPackage.Main.Approximate((GraphSetting.Center.Y - location_.Y) / (double)(GraphSetting.Y_Space), "9", "0", "###") * (GraphSetting.Y_Space) / (double)GraphSetting.Y_Stretch);
            }
            else
            {
                p.X_Value = new MathPackage.Operations.Constant(GraphSetting.xChangeToCoor(location_.X, location_.Y));
                p.Y_Value = new MathPackage.Operations.Constant(GraphSetting.yChangeToCoor(location_.X, location_.Y));
            }
        }

        public void AddPoint(Point Location_)
        {
            Points p = new Points(GraphSetting);
            SetPointCoor(p, Location_);
            p.SetName(GraphSetting.SelectName());
            sketch_.Objects.Add(p);
            AddGraphControl(p);
        }

        #endregion

        #region "MouseMove"

        private Point BW_Argument;

        private void MouseMove_(object sender, MouseEventArgs e)
        {
            BW_Argument = e.Location;
            if (BW.IsBusy)
                NeedRefresh = true;
            else
                BW.RunWorkerAsync(e.Location);
        }

        private void BW_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            NeedRefresh = false;
            if (NeedRefresh)
                BW.RunWorkerAsync(BW_Argument);
            FlushMemory();
        }

        private void BW_DragPoint(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SetPointCoor(SelectedPoint, (Point)e.Argument);
            sketch_.UpdateAndDraw();
            Draw();
        }

        private void BW_DragCoor(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);

            using (Graphics Graphics = Graphics.FromImage(Bitmap))
            {
                Graphics.SmoothingMode = MainFunctions.Smoothing;
                Graphics.CompositingQuality = MainFunctions.Composition;
                sketch_.Coordinates.Draw(new PointF((GraphSetting.Center.X + (((Point)e.Argument).X - DragPoint.X)), (GraphSetting.Center.Y + (((Point)e.Argument).Y - DragPoint.Y))));
                sketch_.Coordinates.DrawTo(Graphics);
                Graphics.DrawImage(sketch_.Bitmap2, new Point(((Point)e.Argument).X - DragPoint.X, ((Point)e.Argument).Y - DragPoint.Y));
                Graphics.DrawImage(sketch_.Pens.StaticPenBitmap, 0, 0);

                Create_Graphics(Bitmap);

                Bitmap.Dispose();
            }
        }

        private PointsDependant pd;

        private void BW_SetPD(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SubBitmap = new Bitmap(Bitmap.Width, Bitmap.Height);
            using (Graphics Graphics = Graphics.FromImage(SubBitmap))
            {
                Graphics.SmoothingMode = MainFunctions.Smoothing;
                Graphics.CompositingQuality = MainFunctions.Composition;
                Graphics.DrawImage(Bitmap, 0, 0);
                pd.Points.Clear();
                pd.Points.AddRange(pointDependant.Points.ToArray());

                SelectedPoint = GetSelectedPoint((Point)e.Argument);
                if (SelectedPoint != null)
                {
                    SelectedPoint.Is_Selected = true;
                    pd.Points.Add(SelectedPoint);
                    pd.Draw();
                    pd.DrawTo(Graphics);

                    for (int i = 0; i < pd.Points.Count; i++)
                    {
                        pd.Points[i].Draw();
                        pd.Points[i].DrawTo(Graphics);
                    }
                    if (!pointDependant.Points.Contains(SelectedPoint))
                    {
                        SelectedPoint.Is_Selected = false;
                    }
                }
                else
                {
                    Points p = new Points(GraphSetting);
                    SetPointCoor(p, (Point)e.Argument);
                    p.Is_Selected = true;
                    pd.Points.Add(p);
                    pd.Draw();
                    pd.DrawTo(Graphics);

                    for (int i = 0; i < pd.Points.Count; i++)
                    {
                        pd.Points[i].Draw();
                        pd.Points[i].DrawTo(Graphics);
                    }
                }

                Create_Graphics(SubBitmap);
                SubBitmap.Dispose();
            }
        }
        private void BW_Pen(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SubBitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            using (Graphics Graphics = Graphics.FromImage(SubBitmap))
            {
                Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                Graphics.DrawImage(Bitmap, 0, 0);
                AddPointToPen((Point)e.Argument);

                pen_.Draw();
                pen_.DrawTo(Graphics);

                Create_Graphics(SubBitmap);
            }
        }
        private void BW_Delete(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            // 
            // Select objects And delete the select
            // 
            Delete((Point)e.Argument);
        }

        #endregion

        #region "MouseUp"

        private void MouseUp_PD(object sender, MouseEventArgs e)
        {
            BW_Argument = e.Location;
            NeedRefresh = false;

            GraphPanel.MouseUp -= MouseUp_PD;
            BW.RunWorkerCompleted -= BW_RunWorkerCompleted;

            if (BW.IsBusy)
            {
                BW.RunWorkerCompleted += BW_PD_;
            }
            else
                BW_PD_(BW, new RunWorkerCompletedEventArgs(null, null, false));
        }
        private void BW_PD_(System.Object sender, RunWorkerCompletedEventArgs e)
        {
            BW.RunWorkerCompleted -= BW_PD_;

            if (pointDependant.Type == PointsDependant.PointDependantType.Polygone)
            {

                if (pointDependant.Points.Last().Name == pointDependant.Points.First().Name && pointDependant.Points.Count > 1)
                {
                    pointDependant.SetName(GraphSetting.SelectName());
                    AddGraphControl(pointDependant, pointDependant.ToString());
                    pointDependant.Points.RemoveAt(pointDependant.Points.Count - 1);
                    foreach (Points p in pointDependant.Points)
                    {
                        p.Is_Selected = false;
                        p.Dependants.Add(pointDependant);
                    }
                    sketch_.Objects.Add(pointDependant);
                    sketch_.UpdateAndDraw();
                    Draw();

                    GraphPanel.Cursor = Cursors.Arrow;

                    BW.DoWork -= BW_SetPD;
                    GraphPanel.MouseMove -= MouseMove_;
                    GraphPanel.MouseDown -= MouseDown_CreatePD;
                    GraphPanel.MouseDown += MouseDown_;

                    return;

                }
            }
            else
            {
                BW.DoWork -= BW_SetPD;
                GraphPanel.MouseMove -= MouseMove_;
                GraphPanel.MouseDown -= MouseDown_CreatePD;
                GraphPanel.MouseDown += MouseDown_;

                pointDependant.SetName(GraphSetting.SelectName());
                AddGraphControl(pointDependant, pointDependant.ToString());
                foreach (Points p in pointDependant.Points)
                {
                    p.Is_Selected = false;
                    p.Dependants.Add(pointDependant);
                }
                sketch_.Objects.Add(pointDependant);
                sketch_.UpdateAndDraw();
                Draw();

                GraphPanel.Cursor = Cursors.Arrow;

                return;
            }
        }

        private void MouseUp_DragPoint(object sender, MouseEventArgs e)
        {
            GraphPanel.MouseMove -= MouseMove_;
            GraphPanel.MouseUp -= MouseUp_DragPoint;
            BW.DoWork -= BW_DragPoint;
            BW.RunWorkerCompleted -= BW_RunWorkerCompleted;
            BW_Argument = e.Location;
            NeedRefresh = false;
            if (BW.IsBusy)
            {
                BW.RunWorkerCompleted += BW_DragPoint_;
            }
            else
                BW_DragPoint_(BW, new RunWorkerCompletedEventArgs(null, null, false));
        }
        private void BW_DragPoint_(System.Object sender, RunWorkerCompletedEventArgs e)
        {
            BW.RunWorkerCompleted -= BW_DragPoint_;
            GraphPanel.Cursor = Cursors.Arrow;
            SelectedPoint.Is_Selected = false;
            SelectedPoint.UpdateScriptText();
            sketch_.UpdateAndDraw();
            Draw();
        }

        private void MouseUp_Pen(object sender, MouseEventArgs e)
        {
            BW.DoWork -= BW_Pen;
            GraphPanel.MouseUp -= MouseUp_Pen;
            GraphPanel.MouseMove -= MouseMove_;
            BW.RunWorkerCompleted -= BW_RunWorkerCompleted;
            BW_Argument = e.Location;
            NeedRefresh = false;
            if (BW.IsBusy)
            {
                BW.RunWorkerCompleted += BW_Pen_;
            }
            else
                BW_Pen_(BW, new RunWorkerCompletedEventArgs(null, null, false));
        }
        private void BW_Pen_(System.Object sender, RunWorkerCompletedEventArgs e)
        {
            BW.RunWorkerCompleted -= BW_Pen_;

            GraphPanel.Cursor = new Cursor(Properties.Resources.Pen_icon.Handle);
            switch (PenIsStatic.Checked)
            {
                case true:
                    {
                        if (pen_.Points.Count > 1)
                        {
                            sketch_.Pens.AddStaticPen((StaticPen)pen_);
                            sketch_.Draw();
                        }
                        else if (pen_.Points.Count == 1)
                        {
                            sketch_.Pens.AddStaticPen((StaticPen)pen_);
                            sketch_.Draw();
                        }
                        break;
                    }
                case false:
                    {
                        if (pen_.Points.Count > 1)
                        {
                            sketch_.Pens.DinamicPenArray.Add((DinamicPen)pen_);
                            sketch_.Draw();
                        }
                        else if (pen_.Points.Count == 1)
                        {
                            sketch_.Pens.DinamicPenArray.Add((DinamicPen)pen_);
                            sketch_.Draw();
                        }
                        break;
                    }
            }

            Draw();
        }

        private void MouseUp_Delete(object sender, MouseEventArgs e)
        {
            GraphPanel.MouseMove -= MouseMove_;
            GraphPanel.MouseUp -= MouseUp_Delete;
            BW.RunWorkerCompleted -= BW_RunWorkerCompleted;
            BW.DoWork -= BW_Delete;
            BW_Argument = e.Location;
            NeedRefresh = false;
            GraphPanel.Cursor = new Cursor(Properties.Resources.Erase_icon.Handle);
        }

        private void MouseUp_DragCoor(object sender, MouseEventArgs e)
        {
            GraphPanel.MouseMove -= MouseMove_;
            GraphPanel.MouseUp -= MouseUp_DragCoor;
            BW.DoWork -= BW_DragCoor;
            BW_Argument = e.Location;
            NeedRefresh = false;
            if (BW.IsBusy)
            {
                BW.RunWorkerCompleted -= BW_RunWorkerCompleted;
                BW.RunWorkerCompleted += BW_DragCoor_;
            }
            else
                BW_DragCoor_(BW, new RunWorkerCompletedEventArgs(null, null, false));
        }
        public void BW_DragCoor_(System.Object sender, RunWorkerCompletedEventArgs e)
        {
            BW.RunWorkerCompleted -= BW_DragCoor_;
            GraphPanel.Cursor = Cursors.Arrow;
            GraphSetting.Center = new PointF((GraphSetting.Center.X + (BW_Argument.X - DragPoint.X)), (GraphSetting.Center.Y + (BW_Argument.Y - DragPoint.Y)));
            GraphSetting.SliderTimer.Enabled = SliderTimerEnabeled;
            sketch_.UpdateAndDraw();
            Draw();
        }

        #endregion

        #region "Tools Code"

        private void CenterButton_Click(object sender, EventArgs e)
        {
            Balance(false);
            GraphPanel.MouseDown += MouseDown_;
            GraphSetting.Centerate();
            sketch_.UpdateAndDraw();
            Draw();
        }

        private void PointerButton_Click(object sender, EventArgs e)
        {
            Balance(true);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Balance(true);
            string str = "";
            switch (MainFunctions.GetLanguage())
            {
                case MathPackage.Main.Language.AR:
                    {
                        str = "ءأنت متأكد من خذف جميع الرسومات؟";
                        break;
                    }

                case MathPackage.Main.Language.EN:
                    {
                        str = "Are you sure of deleting all the drawings?";
                        break;
                    }
            }

            switch (Interaction.MsgBox(str, MsgBoxStyle.YesNo, ""))
            {
                case MsgBoxResult.Yes:
                    {
                        sketch_.DeleteAll();
                        Draw();
                        break;
                    }
            }
        }

        private void PenButton_Click(object sender, EventArgs e)
        {
            Balance(false);
            Bitmap = MainBitmap;
            GraphPanel.Cursor = new Cursor(Properties.Resources.Pen_icon.Handle);
            GraphPanel.MouseDown += MouseDown_Pen;
        }

        private void SliderButton_Click(object sender, EventArgs e)
        {
            Balance(false);
            GraphPanel.MouseDown += MouseDown_Slider;
        }

        private void EraseButton_Click(object sender, EventArgs e)
        {
            Balance(false);
            GraphSetting.IsDelete = true;
            GraphPanel.Cursor = new Cursor(Properties.Resources.Erase_icon.Handle);
            GraphPanel.MouseDown += MouseDown_Delete;
        }

        private void PenEdit_Click(object sender, EventArgs e)
        {
            pens_pen = MainFunctions.AdjustPen(pens_pen);
        }

        private void Show_Hide_Tools_Panel_Click(object sender, EventArgs e)
        {
            ToolsPanel.BringToFront();
        }
        private void Show_Hide_Objects_Panel_Click(object sender, EventArgs e)
        {
            ObjectsPanel.BringToFront();
        }

        private void _2PSemiCircleButton_Click(object sender, EventArgs e)
        {
            pointDependant = new TwoPSemiCircle(GraphSetting);
            pd = new TwoPSemiCircle(GraphSetting);
            PrepareToPd();
        }

        private void _2PCircleButton_Click(object sender, EventArgs e)
        {
            pointDependant = new TwoPCircle2(GraphSetting);
            pd = new TwoPCircle2(GraphSetting);
            PrepareToPd();
        }

        private void bunifuFlatButton11_Click(object sender, EventArgs e)
        {
            pointDependant = new TwoPCircle(GraphSetting);
            pd = new TwoPCircle(GraphSetting);
            PrepareToPd();
        }

        private void _3PCircleButton_Click(object sender, EventArgs e)
        {
            pointDependant = new ThreePCircle(GraphSetting);
            pd = new ThreePCircle(GraphSetting);
            PrepareToPd();
        }

        private void LengthButton_Click(object sender, EventArgs e)
        {
            pointDependant = new Distance(GraphSetting);
            pd = new Distance(GraphSetting);
            PrepareToPd();
        }

        private void AngleButton_Click(object sender, EventArgs e)
        {
            pointDependant = new Angle(GraphSetting);
            pd = new Angle(GraphSetting);
            PrepareToPd();
        }
        private void LineBtn_Click(object sender, EventArgs e)
        {
            pointDependant = new Line(GraphSetting);
            pd = new Line(GraphSetting);
            PrepareToPd();
        }
        private void PolygoneBtn_Click(object sender, EventArgs e)
        {
            pointDependant = new Polygone(GraphSetting);
            pd = new Polygone(GraphSetting);
            PrepareToPd();
        }
        public void PrepareToPd()
        {
            Bitmap = MainBitmap;
            Balance(false);
            GraphPanel.Cursor = new Cursor(Properties.Resources.Pin.Handle);
            GraphPanel.MouseDown += MouseDown_CreatePD;
        }

        private Bitmap GraphicsState1;
        private void PenEdit_Paint(object sender, PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            G.DrawImage(GraphicsState1, 0, 0);
            G.DrawLine(pens_pen, new Point(PenEdit.Width / 8, PenEdit.Height / 2), new Point(PenEdit.Width - PenEdit.Width / 8, PenEdit.Height / 2));
        }
        private void GraphPanel_Resize(object sender, EventArgs e)
        {
            if (loaded && GraphPanel.Width != 0 && GraphPanel.Height != 0)
            {
                GraphSetting.Width = GraphPanel.Width;
                GraphSetting.Height = GraphPanel.Height;
                sketch_.UpdateAndDraw();
                Draw();
            }
        }

        private void AddPointBtn_Click(object sender, EventArgs e)
        {
            Balance(false);
            GraphPanel.Cursor = new Cursor(Properties.Resources.Pin.Handle);
            GraphPanel.MouseDown += MouseDown_AddPoints;
        }
        private void GraphControl_Load(object sender, EventArgs e)
        {
            GraphicsState1 = new Bitmap(PenEdit.Width, PenEdit.Height);
            PenEdit.DrawToBitmap(GraphicsState1, new Rectangle(0, 0, PenEdit.Width, PenEdit.Height));
        }
        private void Show_Hide_Right_Panel_Click(object sender, EventArgs e)
        {
            if (RightPanel.Width == 12)
            {
                RightPanel_.Visible = true;
                RightPanel.Width = 250;
            }
            else
            {
                RightPanel_.Visible = false;
                RightPanel.Width = 12;
            }
        }

        #endregion

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            ToolsPanel1.BringToFront();
        }

        private void bunifuFlatButton7_Click(object sender, EventArgs e)
        {
            ToolsPanel2.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddGraphControl();
        }

        #region AddGraphControl
            /// <summary>
            /// AddGraphControl
            /// </summary>
            public void AddGraphControl()
                {
                    GraphControl gc = new GraphControl(GraphSetting);
                    ObjectsPanel.Controls.Add(gc);
                    gc.Dock = DockStyle.Top;
                    gc.BringToFront();
                    AddObjBtn.BringToFront();
                }
            public void AddGraphControl(GraphObject g = null, string script = "")
            {
                GraphControl gc = new GraphControl(g, GraphSetting, string.IsNullOrEmpty(script) ? g.ToString() : script);
                ObjectsPanel.Controls.Add(gc);
                g.Control = gc;
                gc.BringToFront();
                gc.Dock = DockStyle.Top;
                AddObjBtn.BringToFront();
            }
            public void AddGraphControl(MathPackage.Operations.Func g = null, string script = "")
            {
                GraphControl gc = new GraphControl(g, GraphSetting, string.IsNullOrEmpty(script) ? g.ToString() : script);
                ObjectsPanel.Controls.Add(gc);
                gc.Dock = DockStyle.Top;
                gc.BringToFront();
                AddObjBtn.BringToFront();
            }
            public void AddGraphControl(Variable g = null, string script = "")
            {
                GraphControl gc = new GraphControl(g, GraphSetting,
                    string.IsNullOrEmpty(script) ? g.ToString() : script
                    );
                ObjectsPanel.Controls.Add(gc);
                gc.Dock = DockStyle.Top;
                gc.BringToFront();
                AddObjBtn.BringToFront();
            }
        #endregion

        private void ZoomIn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                GraphSetting.ZoomIn(new PointF(GraphSetting.Width / 2, GraphSetting.Height/2));
                Draw();
            }
            FlushMemory();
        }

        private void ZoomOut_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                GraphSetting.ZoomOut(new PointF(GraphSetting.Width / 2, GraphSetting.Height / 2));
                Draw();
            }
            FlushMemory();
        }

    }
}
