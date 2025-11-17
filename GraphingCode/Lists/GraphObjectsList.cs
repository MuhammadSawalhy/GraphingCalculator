using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using Graphing.Objects;

namespace Graphing.Lists
{
    public class GraphObjectsList : GList<Objects.GraphObject>
    {

        public GraphObjectsList(string name, Loyc.Syntax.LNode script, List<Objects.GraphObject> items, GraphSetting graphSetting, Controls.GraphControl graphControl = null) : base(name, script, items, graphSetting, graphControl) { }
        /// <summary>
        /// This will be like this : { " a*x^2 " } = whereas > a is <see cref="NumbersList"/>.
        /// This will generate a temporary variable ( TempVar ) called "nth" refering to the index
        /// </summary>
        public GraphObjectsList(string name, Loyc.Syntax.LNode script, GraphObject Object, GraphSetting graphSetting, Controls.GraphControl graphControl = null) : base(name, script, null, graphSetting, graphControl)
        {
            GraphObject = Object;
        }
        
        public override LType ListType => LType.GraphObjectsList;

        public GraphObject GraphObject;

        public override int Count
        {
            get {
                if(Items == null)
                {
                    int nth = 0;
                    double testForFunc;
                    do
                    {
                        GraphSetting.CalculationSetting.Vars[0].Value = new MathPackage.Operations.Constant(nth);
                        try
                        {
                            switch (GraphObject)
                            {
                                case XFunction _:
                                    testForFunc = ((XFunction)GraphObject).Get(0);
                                    break;
                                case XYFunction _:
                                    testForFunc = ((XYFunction)GraphObject).Get(0, 0);
                                    break;
                                case ParametricFunc _:
                                    testForFunc = ((ParametricFunc)GraphObject).Get_X(0);
                                    testForFunc = ((ParametricFunc)GraphObject).Get_Y(0);
                                    break;
                                default:
                                    GraphObject.Draw();
                                    break;
                            }
                        }
                        catch
                        {
                            goto Line;
                        }
                        nth += 1;
                    }
                    while (true);
                    Line:;
                    return nth;
                }
                else
                {
                    return Items.Count;
                }
            }
        }
        
        #region GraphObject Properties

        public bool Visible = true;
        protected Pen pen_;
        public Pen Pen
        {
            get
            {
                return pen_;
            }
            set
            {
                pen_ = value;
                Items.ForEach(item => {
                    item.Pen = value;
                });
            }
        }

        public Bitmap Bitmap;

        public void Draw()
        {
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            using (Graphics g = Graphics.FromImage(Bitmap))
            {
                g.SmoothingMode = MainFunctions.Smoothing;
                g.CompositingQuality = MainFunctions.Composition;
                if(Items == null)
                {
                    int nth = 0;
                    do
                    {

                        GraphSetting.CalculationSetting.Vars[0].Value = new MathPackage.Operations.Constant(nth);

                        try
                        {
                            GraphObject.Draw(true);
                            GraphObject.DrawTo(g);
                        }
                        catch
                        {
                            goto Line;
                        }
                        nth += 1;
                    }
                    while (true);
                    Line:;

                }
                else
                {
                    foreach (Objects.GraphObject item in Items)
                    {
                        item.Draw();
                        item.DrawTo(g);
                    }
                }
            }
        }

        public void DrawTo(Graphics g)
        {
            g.DrawImage(Bitmap, 0, 0);
        }

        public override GraphObject GetItem(int index)
        {
            if (Items == null)
            {
                return GraphObject;
            }
            else
            {
                return Items[index];
            }
        }

        #endregion

    }

}

