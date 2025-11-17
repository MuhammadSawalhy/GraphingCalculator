using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Graphing.Objects.Arrays;
using Loyc.Syntax;

using Graphing.Lists;
using System.Threading.Tasks;

namespace Graphing.Objects
{
    public class Sketch
    {
        public string Name;

        // Arrays
        public ObjectsArray Objects = new ObjectsArray();
        public SliderArray Sliders = new SliderArray();
        public PenArray Pens;
        public AuxiliaryPointsArray AuxiliaryPoints = new AuxiliaryPointsArray();

        // Lists
        public List<GraphObjectsList> GraphObjectsLists = new List<GraphObjectsList>();
        public List<NumbersList> NumbersLists = new List<NumbersList>();

        public Bitmap Bitmap;
        public Bitmap Bitmap2;
        public Coordinates Coordinates;

        public Panel Panel = new Panel();

        public GraphSetting GraphSetting;
        public Controls.SketchControl SketchControl;

        public Sketch()
        {
            GraphSetting = new GraphSetting(this);
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Bitmap2 = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Coordinates = new Coordinates(GraphSetting);
            Pens = new PenArray(GraphSetting);
            SketchControl = new Controls.SketchControl(this);
        }
        public Sketch(GraphSetting graphSetting)
        {
            GraphSetting = graphSetting;
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Bitmap2 = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Coordinates = new Coordinates(GraphSetting);
            Pens = new PenArray(GraphSetting);
            SketchControl = new Controls.SketchControl(this);
        }

        public void AddObject(string script, bool AddControl)
        {
            object g = null;
            try
            {
                g = MainFunctions.GetObject(script, GraphSetting, true, true);
                if (g is GraphObject)
                {
                    AddObject((GraphObject)g, AddControl, script);
                }
                else
                {
                    if (g is Variable)
                    {
                        GraphSetting.CalculationSetting.Vars.Add((Variable)g);
                    }
                    else if (g is MathPackage.Operations.Func)
                    {
                        GraphSetting.CalculationSetting.Funcs.Add((MathPackage.Operations.Func)g);
                    }
                    else if (g is GraphObjectsList)
                    {
                        GraphSetting.Sketch.GraphObjectsLists.Add((GraphObjectsList)g);
                    }
                    else if (g is NumbersList)
                    {
                        GraphSetting.Sketch.NumbersLists.Add((NumbersList)g);
                    }
                }
            }
            catch (Exception ex)
            {
                // here will be an error message
                MessageBox.Show(ex.Message);
            }
        }

        public void AddObject(GraphObject g, bool AddControl, string script = "")
        {
            if (AddControl)
            {
                SketchControl.AddGraphControl(g, script);
            }
            Objects.Add(g);
        }

        public void Delete(GraphObject g, bool RemoveControl)
        {
            switch (g)
            {
                case Points _:
                    Objects.Delete((Points)g, RemoveControl);
                    break;
                case XFunction _:
                    Objects.Delete((XFunction)g, RemoveControl);
                    break;
                case PointsDependant _:
                    Objects.Delete((PointsDependant)g, RemoveControl);
                    break;
            }
        }

        /// <summary>
        ///         ''' This Drawing Will be drawn to Bitmap
        ///         ''' </summary>
        public void UpdateAndDraw()
        {
            if (GraphSetting.MyState == GraphSetting.State.Ready)
            {
                Update();
                Draw();
            }
        }

        public void Update()
        {
            if (GraphSetting.MyState == GraphSetting.State.Ready)
            {
                AuxiliaryPoints.Clear();
                Coordinates.Draw();
                foreach (GraphObject obj in Objects.ToList())
                    if (obj.Visible)
                    {
                        obj.Draw();
                    }
                for (int i = 0; i < GraphObjectsLists.Count; i++)
                    if (GraphObjectsLists.ElementAt(i).Visible)
                    {
                        GraphObjectsLists.ElementAt(i).Draw();
                    }
                foreach (DinamicPen pen in Pens.DinamicPenArray)
                    if (pen.Visible)
                    {
                        pen.Draw();
                    }
            }
        }

        /// <summary>
        ///         ''' This Drawing Will be drawn to Bitmap
        ///         ''' </summary>
        public void Draw()
        {

            Bitmap.Dispose();
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            using (Graphics Graphics = Graphics.FromImage(Bitmap))
            {
                Graphics.SmoothingMode = MainFunctions.Smoothing;
                Graphics.CompositingQuality = MainFunctions.Composition;

                Coordinates.DrawTo(Graphics);

                foreach (GraphObject obj in Objects.ToList())
                    if (obj.Visible)
                    {
                        obj.DrawTo(Graphics);
                    }
                for (int i = 0; i < GraphObjectsLists.Count; i++)
                    if (GraphObjectsLists.ElementAt(i).Visible)
                    {
                        GraphObjectsLists.ElementAt(i).DrawTo(Graphics);
                    }

                foreach (DinamicPen pen in Pens.DinamicPenArray)
                    if (pen.Visible)
                    {
                        pen.DrawTo(Graphics);
                    }
                foreach (AuxiliaryPoints point in AuxiliaryPoints.ToList())
                    point.DrawTo(Graphics);

                Graphics.DrawImage(Pens.StaticPenBitmap, 0, 0);
            }
        }
        /// <summary>
        ///         ''' This Drawing Will be drawn to Bitmap2
        ///         ''' </summary>
        ///         ''' <param name="Func"></param>
        public void DrawExcept(GraphObject[] objs)
        {
            Bitmap2.Dispose();
            Bitmap2 = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            using (Graphics Graphics = Graphics.FromImage(Bitmap2))
            {
                Graphics.SmoothingMode = MainFunctions.Smoothing;
                Graphics.CompositingQuality = MainFunctions.Composition;

                Coordinates.DrawTo(Graphics);

                List<GraphObject> objects = new List<GraphObject>(Objects.ToArray());

                foreach (GraphObject obj in objs)
                    objects.Remove(obj);


                foreach (GraphObject obj in Objects.ToArray())
                    if (obj.Visible)
                        obj.DrawTo(Graphics);

                for (int i = 0; i < GraphObjectsLists.Count; i++)
                    if (GraphObjectsLists.ElementAt(i).Visible)
                        GraphObjectsLists.ElementAt(i).DrawTo(Graphics);

                foreach (DinamicPen pen in Pens.DinamicPenArray)
                    if (pen.Visible)
                        pen.DrawTo(Graphics);

                foreach (AuxiliaryPoints point in AuxiliaryPoints.ToList())
                    point.DrawTo(Graphics);


                Graphics.DrawImage(Pens.StaticPenBitmap, 0, 0);
            }
        }
        /// <summary>
        ///         ''' This Drawing Will be drawn to Bitmap2
        ///         ''' </summary>
        public void DrawExcept(Points[] points, bool removePDs)
        {
            Bitmap2.Dispose();
            Bitmap2 = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            using (Graphics Graphics = Graphics.FromImage(Bitmap2))
            {
                Graphics.SmoothingMode = MainFunctions.Smoothing;
                Graphics.CompositingQuality = MainFunctions.Composition;

                Coordinates.DrawTo(Graphics);

                List<GraphObject> objects = new List<GraphObject>();
                objects.AddRange(Objects.ToArray());

                //removing the points...
                foreach (Points point in points)
                {
                    if (removePDs)
                    {
                        foreach (PointsDependant pd in point.Dependants)
                        {
                            if (objects.Contains(pd))
                                objects.Remove(pd);
                        }
                    }
                    objects.Remove(point);
                }

                foreach (GraphObject obj in Objects.ToArray())
                    if (obj.Visible)
                        obj.DrawTo(Graphics);

                for (int i = 0; i < GraphObjectsLists.Count; i++)
                    if (GraphObjectsLists.ElementAt(i).Visible)
                        GraphObjectsLists.ElementAt(i).DrawTo(Graphics);

                foreach (DinamicPen pen in Pens.DinamicPenArray)
                    if (pen.Visible)
                        pen.DrawTo(Graphics);

                foreach (AuxiliaryPoints point in AuxiliaryPoints.ToList())
                    point.DrawTo(Graphics);


                Graphics.DrawImage(Pens.StaticPenBitmap, 0, 0);
            }
        }

        public void DrawExceptCoor()
        {
            Bitmap2.Dispose();
            Bitmap2 = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            using (Graphics Graphics = Graphics.FromImage(Bitmap2))
            {
                Graphics.SmoothingMode = MainFunctions.Smoothing;
                Graphics.CompositingQuality = MainFunctions.Composition;

                foreach (GraphObject obj in Objects.ToArray())
                    if (obj.Visible)
                        obj.DrawTo(Graphics);

                for (int i = 0; i < GraphObjectsLists.Count; i++)
                    if (GraphObjectsLists.ElementAt(i).Visible)
                        GraphObjectsLists.ElementAt(i).DrawTo(Graphics);

                foreach (DinamicPen pen in Pens.DinamicPenArray)
                    if (pen.Visible)
                        pen.DrawTo(Graphics);

                foreach (AuxiliaryPoints point in AuxiliaryPoints.ToList())
                    point.DrawTo(Graphics);

                Graphics.DrawImage(Pens.StaticPenBitmap, 0, 0);
            }
        }

        public void DeleteAll()
        {
            Objects.Clear();
            Pens.DinamicPenArray.Clear();
            Pens.StaticPenArray.Clear();
            Draw();
        }


    }
}
