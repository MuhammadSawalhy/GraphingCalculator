using System;
using System.Drawing;
using System.Windows.Forms;
using Graphing.Objects;
using System.Collections.Generic;
using Graphing.Lists;
namespace Graphing.Controls
{
    public partial class GraphControl : UserControl
    {

        object GraphObject_ = null;
        public object GraphObject
        {
            get
            {
                return GraphObject_;
            }
            set
            {
                if (value == null)
                {
                    GraphObject_ = null;
                    NullifyObject(true);
                }
                else
                {
                    GraphObject_ = value;
                    if (value is GraphObject)
                    {
                        NullifyObject(false);
                        VisibleCheckBox.Checked = ((GraphObject)value).Visible;
                    }
                    else
                        NullifyObject(true);
                    setName();
                }
            }
        }

        public string Error;
        public void ShowError(string message)
        {
            VisibleCheckBox.Enabled = false;
            EditBtn.Enabled = false;
            ErrorBtn.Visible = true;
            Error = message;
            BackColor = System.Drawing.Color.Black;
        }


        public GraphSetting GraphSetting;

        private bool isloaded;

        void NullifyObject(bool nullify)
        {
            VisibleCheckBox.Enabled = !nullify;
            EditBtn.Enabled = !nullify;
        }

        public GraphControl(GraphSetting GraphSetting_)
        {
            InitializeComponent();
            GraphSetting = GraphSetting_;
            isloaded = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"> you can pass {Variable, Func, GraphObject}</param>
        /// <param name="script"></param>
        public GraphControl(object g, GraphSetting graphSetting, string script)
        {
            InitializeComponent();
            isloaded = true;
            if (g != null)
            {
                GraphObject = g;
                GraphSetting = graphSetting;
                Script.Text = script;
            }
            else
            {
                NullifyObject(true);
            }
        }

        void AdjustObject(object g)
        {
            // if the name is null or empty then we didn't assgin a name for it before.
            // so we will put the name for GraphObject reference if the name has been assigned before.
            switch (g)
            {

                case Points _:
                    {
                        if (GraphObject != null && GraphObject is Points)
                        {
                            if (((Points)g).Name != null && ((Points)g).Name != "")
                            {
                                if (GraphSetting.IsNameUsed(((Points)g).Name, new List<string>(new string[] { ((Points)GraphObject).Name })))
                                {
                                    throw new Exception("This name has already been used.");
                                }
                                ((Points)GraphObject).Name = ((Points)g).Name;
                            }
                            ((Points)GraphObject).Pen = ((Points)g).Pen;
                            ((Points)GraphObject).Size = ((Points)g).Size;
                            ((Points)GraphObject).Visible = true;

                            ((Points)GraphObject).x_variabled = false;
                            ((Points)GraphObject).y_variabled = false;
                            ((Points)GraphObject).x_static = false;
                            ((Points)GraphObject).y_static = false;
                            ((Points)GraphObject).X_Value = ((Points)g).X_Value;
                            ((Points)GraphObject).Y_Value = ((Points)g).Y_Value;
                            GraphObject = ((Points)GraphObject);
                        }
                        else
                            ComplementaryAdjustments((GraphObject)g);
                    }
                    break;
                case XFunction _:
                    {
                        if (GraphObject != null && GraphObject is XFunction)
                        {
                            // we won't change the pen
                            if (((XFunction)g).Name != null && ((XFunction)g).Name != "")
                            {
                                if (GraphSetting.IsNameUsed(((XFunction)g).Name, new List<string>(new string[] { ((XFunction)GraphObject).Name })))
                                {
                                    throw new Exception($"This name \"{((XFunction)g).Name}\" has already been used.");
                                }
                                ((XFunction)GraphObject).Name = ((XFunction)g).Name;
                            }
                            ((XFunction)GraphObject).Expression = ((XFunction)g).Expression;
                            ((XFunction)GraphObject).Visible = true;
                            GraphObject = ((XFunction)GraphObject);
                        }
                        else
                            ComplementaryAdjustments((GraphObject)g);
                    }
                    break;
                case XYFunction _:
                    {
                        if (GraphObject != null && GraphObject is XYFunction)
                        {
                            // we won't change the pen
                            if (((Function)g).Name != null && ((Function)g).Name != "")
                            {
                                if (GraphSetting.IsNameUsed(((Function)g).Name, new List<string>(new string[] { ((Function)GraphObject).Name })))
                                {
                                    throw new Exception($"This name \"{((Function)g).Name}\" has already been used.");
                                }
                                ((Function)GraphObject).Name = ((Function)g).Name;
                            }
                            ((Function)GraphObject).Expression = ((Function)g).Expression;
                            ((Function)GraphObject).Visible = true;
                            GraphObject = ((Function)GraphObject);
                        }
                        else
                            ComplementaryAdjustments((GraphObject)g);
                    }
                    break;
                case ParametricFunc _:
                    {
                        if (GraphObject != null && GraphObject is ParametricFunc)
                        {
                            // we won't change the pen
                            if (((ParametricFunc)g).Name != null && ((ParametricFunc)g).Name != "")
                            {
                                ((ParametricFunc)GraphObject).SetName(((ParametricFunc)g).Name);
                            }
                            ((ParametricFunc)GraphObject).Parameter = ((ParametricFunc)g).Parameter;
                            ((ParametricFunc)GraphObject).x_Expression = ((ParametricFunc)g).x_Expression;
                            ((ParametricFunc)GraphObject).y_Expression = ((ParametricFunc)g).y_Expression;
                            ((ParametricFunc)GraphObject).Start = ((ParametricFunc)g).Start;
                            ((ParametricFunc)GraphObject).End = ((ParametricFunc)g).End;
                            ((ParametricFunc)GraphObject).Step = ((ParametricFunc)g).Step;
                            ((ParametricFunc)GraphObject).Visible = true;

                            GraphObject = ((ParametricFunc)GraphObject);
                        }
                        else
                            ComplementaryAdjustments((GraphObject)g);
                    }
                    break;
                case PointsDependant _:
                    {
                        if (GraphObject != null && GraphObject is PointsDependant)
                        {
                            Pen pen = ((PointsDependant)GraphObject).Pen;
                            bool a = false;
                            if (((PointsDependant)g).Name != null && ((PointsDependant)g).Name != ((PointsDependant)GraphObject).Name)
                            {
                                ((PointsDependant)g).SetName(((PointsDependant)g).Name);
                                a = true;
                            }
                            GraphSetting.RemoveByName(((PointsDependant)GraphObject).Name, false);
                            if (!a)
                            {
                                ((PointsDependant)g).SetName(((PointsDependant)GraphObject).Name);
                            }
                            // I want to keep the pen
                            ((PointsDependant)g).Pen = pen;
                            ((PointsDependant)g).Control = this;
                            ((PointsDependant)GraphObject).Visible = true;
                            GraphSetting.Sketch.AddObject((PointsDependant)g, false);
                            GraphObject = (PointsDependant)g;
                        }
                        else
                            ComplementaryAdjustments((GraphObject)g);
                    }
                    break;
                case Variable _:
                    {
                        if (GraphObject != null && GraphObject is Variable)
                        {
                            if (((Variable)g).Name == null && ((Variable)g).Name.Name == "")
                            {
                                throw new Exception("The variable must have a name");
                            }
                            GraphSetting.ChangeVarKey(((Variable)g).Name, ((Variable)GraphObject).Name);
                            ((Variable)GraphObject).Value = ((Variable)g).Value;
                            GraphObject = (Variable)g;
                        }
                        else
                            ComplementaryAdjustments((Variable)g);
                    }
                    break;
                case MathPackage.Operations.Func _:
                    {
                        if (GraphObject != null && GraphObject is MathPackage.Operations.Func)
                        {
                            if (((MathPackage.Operations.Func)g).Name == null && ((MathPackage.Operations.Func)g).Name.Name == "")
                            {
                                throw new Exception("The variable must have a name");
                            }

                            GraphSetting.ChangeFuncKey(((MathPackage.Operations.Func)g).Name, ((MathPackage.Operations.Func)GraphObject).Name);

                            for (int i = 0; i < GraphSetting.CalculationSetting.Funcs.Count; i++)
                            {
                                if (GraphSetting.CalculationSetting.Funcs[i].Name == ((MathPackage.Operations.Func)GraphObject).Name)
                                {
                                    GraphSetting.CalculationSetting.Funcs[i].Args = ((MathPackage.Operations.Func)g).Args;
                                    GraphSetting.CalculationSetting.Funcs[i].Process = ((MathPackage.Operations.Func)g).Process;
                                }
                            }
                            GraphObject = (MathPackage.Operations.Func)g;
                        }
                        else
                            ComplementaryAdjustments((MathPackage.Operations.Func)g);
                    }
                    break;
                case GraphObjectsList _:
                    {
                        if (GraphObject is GraphObjectsList)
                        {
                            ((GraphObjectsList)GraphObject).Items = ((GraphObjectsList)g).Items;
                            ((GraphObjectsList)GraphObject).Script = ((GraphObjectsList)g).Script;
                            ((GraphObjectsList)GraphObject).Pen = ((GraphObjectsList)g).Pen;

                            GraphObject = (GraphObjectsList)g;
                        }
                        else
                            ComplementaryAdjustments((GraphObjectsList)g);
                    }
                    break;
                case NumbersList _:
                    {
                        if (GraphObject is NumbersList)
                        {
                            ((NumbersList)GraphObject).Items = ((NumbersList)g).Items;
                            ((NumbersList)GraphObject).Script = ((NumbersList)g).Script;

                            GraphObject = (NumbersList)g;
                        }
                        else
                            ComplementaryAdjustments((NumbersList)g);
                    }
                    break;
            }
        }

        #region ComplementaryAdjustments 

        bool ChangeType_MessegeWarning(object from, object to)
        {
            return MessageBox.Show($"You are going to change the object type from {from} to {to}, complete?", "Wraning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        void ComplementaryAdjustments(GraphObject g)
        {
            if (GraphObject != null && GraphObject is MathPackage.Operations.Func)
            {
                // warnning when you change the object type!
                if (MessageBox.Show("You are going to change the object type, complete?", "Wraning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    GraphSetting.CalculationSetting.Funcs.Remove((MathPackage.Operations.Func)GraphObject);
                    if (g.Name != null && g.Name != "")
                    {
                        g.SetName(g.Name);
                    }
                    GraphSetting.Sketch.Objects.Add(g);
                    GraphObject = g;
                }
            }
            else if (GraphObject != null && GraphObject is Variable)
            {
                // warnning when you change the object type!
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    GraphSetting.CalculationSetting.Vars.Remove((Variable)GraphObject);
                    if (g.Name != null && g.Name != "")
                    {
                        g.SetName(g.Name);
                    }
                    GraphSetting.Sketch.Objects.Add(g);
                    GraphObject = g;
                }
            }
            else if (GraphObject is GraphObjectsList)
            {
                // warnning when you change the object type!
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    GraphSetting.Sketch.GraphObjectsLists.Remove(((GraphObjectsList)GraphObject));
                    if (g.Name != null && g.Name != "")
                    {
                        g.SetName(g.Name);
                    }
                    GraphSetting.Sketch.Objects.Add(g);
                    GraphObject = g;
                }
            }
            else if (GraphObject is NumbersList)
            {
                // warnning when you change the object type!
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    GraphSetting.Sketch.NumbersLists.Remove(((NumbersList)GraphObject));
                    if (g.Name != null && g.Name != "")
                    {
                        g.SetName(g.Name);
                    }
                    GraphSetting.Sketch.Objects.Add(g);
                    GraphObject = g;
                }
            }
            else if (GraphObject is GraphObject)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    if ((g).Name != null && (g).Name != "")
                    {
                        (g).SetName((g).Name);
                    }
                    // Delete the current object
                    if (GraphObject_ is Points)
                        ((Points)GraphObject_).Delete(false);
                    else
                        ((GraphObject)GraphObject_).Delete(false);
                    // Add the new object
                    GraphSetting.Sketch.Objects.Add(g);
                }
            }
            else
            {
                throw new Exception("The object hasn't been assigned before.");
            }
        }
        void ComplementaryAdjustments(Variable g)
        {                    
            if (GraphObject != null && GraphObject is MathPackage.Operations.Func)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    if (((Variable)g).Name == null && ((Variable)g).Name.Name == "")
                    {
                        throw new Exception("The variable must have a name");
                    }
                    bool b = GraphSetting.IsNameUsed(((Variable)g).Name.Name, new List<String>(new string[] { ((MathPackage.Operations.Func)GraphObject).Name.Name }));
                    if (b)
                    {
                        throw new Exception($"This name \"{((Variable)g).Name.Name}\" has already been used.");
                    }
                    GraphSetting.CalculationSetting.Funcs.Remove((MathPackage.Operations.Func)GraphObject);
                    ((Variable)g).Control = this;
                    GraphSetting.CalculationSetting.Vars.Add(g);
                    GraphObject = (Variable)g;
                }
            }
            else if (GraphObject != null && GraphObject is GraphObjectsList)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(((Variable)g).Name.Name);
                    if (GraphSetting.IsNameUsed(((Variable)g).Name.Name, new List<String>(new string[] { ((GraphObjectsList)GraphObject).Name })))
                        throw new Exception($"This name \"{((Variable)g).Name.Name}\" has already been used.");
                    #endregion

                    GraphSetting.Sketch.GraphObjectsLists.Remove((GraphObjectsList)GraphObject);

                    ((Variable)g).Control = this;
                    GraphSetting.CalculationSetting.Vars.Add((Variable)g);
                    GraphObject = (Variable)g;
                }
            }
            else if (GraphObject != null && GraphObject is NumbersList)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(((Variable)g).Name.Name);
                    if (GraphSetting.IsNameUsed(((Variable)g).Name.Name, new List<String>(new string[] { ((NumbersList)GraphObject).Name })))
                        throw new Exception($"This name \"{((Variable)g).Name.Name}\" has already been used.");
                    #endregion

                    GraphSetting.Sketch.GraphObjectsLists.Remove((GraphObjectsList)GraphObject);

                    ((Variable)g).Control = this;
                    GraphSetting.CalculationSetting.Vars.Add((Variable)g);
                    GraphObject = (Variable)g;
                }
            }
            else if (GraphObject is GraphObject)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    if (((Variable)g).Name == null && ((Variable)g).Name.Name == "")
                    {
                        throw new Exception("The variable must have a name");
                    }

                    if (g.Name.Name != ((GraphObject)GraphObject_).Name && GraphSetting.IsNameUsed(g.Name.Name))
                    {
                        throw new Exception($"This name \"{g.Name.Name}\" has already been used.");
                    }

                    // Delete the current object
                    if (GraphObject_ is Points)
                        ((Points)GraphObject_).Delete(false);
                    else
                        ((GraphObject)GraphObject_).Delete(false);

                    (g).Control = this;

                    GraphSetting.CalculationSetting.Vars.Add((Variable)g);

                    GraphObject = (Variable)g;
                }
            }
            else
            {
                throw new Exception("The object hasn't been assigned before.");
            }
        }
        void ComplementaryAdjustments(MathPackage.Operations.Func g)
        {
            if (GraphObject != null && GraphObject is Variable)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(((MathPackage.Operations.Func)g).Name.Name);
                    if (GraphSetting.IsNameUsed(((MathPackage.Operations.Func)g).Name.Name, new List<String>(new string[] { ((Variable)GraphObject).Name.Name })))
                        throw new Exception($"This name \"{((MathPackage.Operations.Func)g).Name.Name}\" has already been used.");
                    #endregion

                    GraphSetting.CalculationSetting.Vars.Remove((Variable)GraphObject);

                    GraphSetting.CalculationSetting.Funcs.Add((MathPackage.Operations.Func)g);
                    GraphObject = (MathPackage.Operations.Func)g;
                }
            }
            else if (GraphObject != null && GraphObject is GraphObjectsList)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(((MathPackage.Operations.Func)g).Name.Name);
                    if (GraphSetting.IsNameUsed(((MathPackage.Operations.Func)g).Name.Name, new List<String>(new string[] { ((GraphObjectsList)GraphObject).Name })))
                        throw new Exception($"This name \"{((MathPackage.Operations.Func)g).Name.Name}\" has already been used.");
                    #endregion

                    GraphSetting.Sketch.GraphObjectsLists.Remove((GraphObjectsList)GraphObject);

                    GraphSetting.CalculationSetting.Funcs.Add((MathPackage.Operations.Func)g);
                    GraphObject = (MathPackage.Operations.Func)g;
                }
            }
            else if (GraphObject != null && GraphObject is NumbersList)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(((MathPackage.Operations.Func)g).Name.Name);
                    if (GraphSetting.IsNameUsed(((MathPackage.Operations.Func)g).Name.Name, new List<String>(new string[] { ((GraphObjectsList)GraphObject).Name })))
                        throw new Exception($"This name \"{((MathPackage.Operations.Func)g).Name.Name}\" has already been used.");
                    #endregion

                    GraphSetting.Sketch.GraphObjectsLists.Remove((GraphObjectsList)GraphObject);

                    GraphSetting.CalculationSetting.Funcs.Add((MathPackage.Operations.Func)g);
                    GraphObject = (MathPackage.Operations.Func)g;
                }
            }
            else if (GraphObject is GraphObject)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    if (GraphSetting.IsNameUsed(g.Name.Name, new List<string>(new[] { g.Name.Name })))
                    {
                        throw new Exception($"This name \"{g.Name}\" has already been used.");
                    }
                    // Delete the current object
                    if (GraphObject_ is Points)
                        ((Points)GraphObject_).Delete(false);
                    else
                        ((GraphObject)GraphObject_).Delete(false);

                    GraphSetting.CalculationSetting.Funcs.Add((MathPackage.Operations.Func)g);
                    GraphObject = (MathPackage.Operations.Func)g;
                }
            }
            else
            {
                throw new Exception("The object hasn't been assigned before.");
            }
        }
        void ComplementaryAdjustments(GraphObjectsList g)
        {
            if (GraphObject != null && GraphObject is Variable)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(g.Name);
                    if (GraphSetting.IsNameUsed(g.Name, new List<String>(new string[] { ((Variable)GraphObject).Name.Name })))
                        throw new Exception($"This name \"{(g).Name}\" has already been used.");
                    #endregion

                    GraphSetting.CalculationSetting.Vars.Remove((Variable)GraphObject);

                    g.Control = this;
                    GraphSetting.Sketch.GraphObjectsLists.Add(g);
                    GraphObject = g;
                }
            }
            else if (GraphObject != null && GraphObject is MathPackage.Operations.Func)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(g.Name);
                    if (GraphSetting.IsNameUsed(g.Name, new List<String>(new string[] { ((MathPackage.Operations.Func)GraphObject).Name.Name })))
                        throw new Exception($"This name \"{(g).Name}\" has already been used.");
                    #endregion

                    GraphSetting.CalculationSetting.Funcs.Remove((MathPackage.Operations.Func)GraphObject);

                    g.Control = this;
                    GraphSetting.Sketch.GraphObjectsLists.Add(g);
                    GraphObject = g;
                }
            }
            else if (GraphObject != null && GraphObject is NumbersList)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(g.Name);
                    if (GraphSetting.IsNameUsed(g.Name, new List<String>(new string[] { ((NumbersList)GraphObject).Name })))
                        throw new Exception($"This name \"{(g).Name}\" has already been used.");
                    #endregion

                    GraphSetting.Sketch.NumbersLists.Remove((NumbersList)GraphObject);

                    g.Control = this;
                    GraphSetting.Sketch.GraphObjectsLists.Add(g);
                    GraphObject = g;
                }
            }
            else if (GraphObject is GraphObject)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(g.Name);
                    if (GraphSetting.IsNameUsed(g.Name, new List<String>(new string[] { ((GraphObject)GraphObject).Name })))
                        throw new Exception($"This name \"{(g).Name}\" has already been used.");
                    #endregion

                    // Delete the current object
                    if (GraphObject_ is Points)
                        ((Points)GraphObject_).Delete(false);
                    else
                        ((GraphObject)GraphObject_).Delete(false);

                    g.Control = this;
                    GraphSetting.Sketch.GraphObjectsLists.Add(g);
                    GraphObject = g;
                }
            }
            else
            {
                throw new Exception("The object hasn't been assigned before.");
            }
        }
        void ComplementaryAdjustments(NumbersList g)
        {
            if (GraphObject != null && GraphObject is Variable)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(g.Name);
                    if (GraphSetting.IsNameUsed(g.Name, new List<String>(new string[] { ((Variable)GraphObject).Name.Name })))
                        throw new Exception($"This name \"{(g).Name}\" has already been used.");
                    #endregion

                    GraphSetting.CalculationSetting.Vars.Remove((Variable)GraphObject);

                    g.Control = this;
                    GraphSetting.Sketch.NumbersLists.Add(g);
                    GraphObject = g;
                }
            }
            else if (GraphObject != null && GraphObject is MathPackage.Operations.Func)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(g.Name);
                    if (GraphSetting.IsNameUsed(g.Name, new List<String>(new string[] { ((MathPackage.Operations.Func)GraphObject).Name.Name })))
                        throw new Exception($"This name \"{(g).Name}\" has already been used.");
                    #endregion

                    GraphSetting.CalculationSetting.Funcs.Remove((MathPackage.Operations.Func)GraphObject);

                    g.Control = this;
                    GraphSetting.Sketch.NumbersLists.Add(g);
                    GraphObject = g;
                }
            }
            else if (GraphObject != null && GraphObject is GraphObjectsList)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(g.Name);
                    if (GraphSetting.IsNameUsed(g.Name, new List<String>(new string[] { ((GraphObjectsList)GraphObject).Name })))
                        throw new Exception($"This name \"{(g).Name}\" has already been used.");
                    #endregion

                    GraphSetting.Sketch.GraphObjectsLists.Remove((GraphObjectsList)GraphObject);

                    g.Control = this;
                    GraphSetting.Sketch.NumbersLists.Add(g);
                    GraphObject = g;
                }
            }
            else if (GraphObject is GraphObject)
            {
                if (ChangeType_MessegeWarning(GraphObject, g))
                {
                    #region Checking for the name
                    MainFunctions.CheckName(g.Name);
                    if (GraphSetting.IsNameUsed(g.Name, new List<String>(new string[] { ((GraphObject)GraphObject).Name })))
                        throw new Exception($"This name \"{(g).Name}\" has already been used.");
                    #endregion

                    // Delete the current object
                    if (GraphObject_ is Points)
                        ((Points)GraphObject_).Delete(false);
                    else
                        ((GraphObject)GraphObject_).Delete(false);

                    g.Control = this;
                    GraphSetting.Sketch.NumbersLists.Add(g);
                    GraphObject = g;
                }
            }
            else
            {
                throw new Exception("The object hasn't been assigned before.");
            }
        }
       


        #endregion

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isloaded)
            {
                ((GraphObject)GraphObject).Visible = VisibleCheckBox.Checked;
                GraphSetting.Sketch.UpdateAndDraw();
                GraphSetting.Sketch.SketchControl.Draw();
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            switch (((GraphObject)GraphObject).Get_Type)
            {

                case "Points":
                    {
                        PointForm f = new PointForm((Objects.Points)GraphObject);
                        f.ShowDialog();
                        ((GraphObject)GraphObject).GraphSetting.Sketch.UpdateAndDraw();
                        ((GraphObject)GraphObject).GraphSetting.Sketch.SketchControl.Draw();
                        break;
                    }
                case "Texts":
                    {
                        break;
                    }
                //for editting the pen
                default:
                    {
                        MainFunctions.AdjustPen(((GraphObject)GraphObject).Pen);
                        GraphSetting.Sketch.UpdateAndDraw();
                        GraphSetting.Sketch.SketchControl.Draw();
                        break;
                    }
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {

            if (GraphObject != null && GraphObject is GraphObject)
            {
                ((GraphObject)GraphObject).Delete(true);
                GraphSetting.Sketch.UpdateAndDraw();
                GraphSetting.Sketch.SketchControl.Draw();
            }
            else
            {
                if (GraphObject is Variable)
                {
                    GraphSetting.CalculationSetting.Vars.Remove((Variable)GraphObject);
                }
                else if (GraphObject is MathPackage.Operations.Func)
                {
                    GraphSetting.CalculationSetting.Funcs.Remove(((MathPackage.Operations.Func)GraphObject));
                }
                GraphSetting.Sketch.UpdateAndDraw();
                GraphSetting.Sketch.SketchControl.Draw();
                Parent.Controls.Remove(this);
            }
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            object g = null;
            try
            {
                g = MainFunctions.GetObject(Script.Text, GraphSetting, GraphObject_ == null, GraphObject_ == null);
                if (GraphObject_ == null)
                {
                    string s = g.ToString();
                    if (g is GraphObject)
                    {
                        ((GraphObject)g).Control = this;
                        if (!string.IsNullOrEmpty(((GraphObject)g).Name))
                        {
                            ((GraphObject)g).SetName(((GraphObject)g).Name);
                        }
                        GraphSetting.Sketch.AddObject(((GraphObject)g), false);
                        GraphObject = g;
                    }
                    else if (g is MathPackage.Operations.Func)
                    {
                        MathPackage.Operations.Func f = (MathPackage.Operations.Func)g;
                        bool exist = GraphSetting.IsNameUsed(f.Name.Name);
                        if (exist)
                        {
                            throw new Exception("The name has already been used before.");
                        }
                        GraphSetting.CalculationSetting.Funcs.Add(f);
                        GraphObject = g;
                    }
                    else if (g is Variable)
                    {
                        if (((Variable)g).Name == null)
                        {
                            throw new Exception("The variable must have a name");
                        }
                        ((Variable)g).Control = this;
                        GraphSetting.CalculationSetting.Vars.Add((Variable)g);
                        GraphObject = g;
                    }
                    else if(g is GraphObjectsList)
                    {
                        if (!string.IsNullOrEmpty(((GraphObjectsList)g).Name))
                        {
                            ((GraphObjectsList)g).SetName(((GraphObjectsList)g).Name);
                        }
                        ((GraphObjectsList)g).Control = this;
                        GraphSetting.Sketch.GraphObjectsLists.Add((GraphObjectsList)g);
                        GraphObject = g;
                    }
                    else if (g is NumbersList)
                    {
                        if (!string.IsNullOrEmpty(((NumbersList)g).Name))
                        {
                            ((NumbersList)g).SetName(((NumbersList)g).Name);
                        }
                        ((NumbersList)g).Control = this;
                        GraphSetting.Sketch.NumbersLists.Add((NumbersList)g);
                        GraphObject = g;
                    }
                }
                else
                {
                    AdjustObject(g);
                }
                ErrorBtn.Visible = false;
                BackColor = System.Drawing.Color.FromArgb(70, 78, 87);
            }
            catch (Exception ex)
            {
                // error
                if (GraphObject is GraphObject)
                    ((GraphObject)GraphObject).ShowError(ex.Message);
                else
                    ShowError(ex.Message);
            }
            GraphSetting.Sketch.UpdateAndDraw();
            GraphSetting.Sketch.SketchControl.Draw();
        }

        void setName()
        {
            switch (GraphObject)
            {
                case GraphObject _:
                    NameLab.Text = string.IsNullOrEmpty(((GraphObject)GraphObject).Name)? "" : ((GraphObject)GraphObject).Name;
                    break;
                case Variable _:
                    NameLab.Text = string.IsNullOrEmpty(((Variable)GraphObject).Name.Name) ? "" : ((Variable)GraphObject).Name.Name;
                    break;
                case MathPackage.Operations.Func _:
                    NameLab.Text = string.IsNullOrEmpty(((MathPackage.Operations.Func)GraphObject).Name.Name) ? "" : ((MathPackage.Operations.Func)GraphObject).Name.Name;
                    break;
                case GraphObjectsList _:
                    NameLab.Text = string.IsNullOrEmpty(((GraphObjectsList)GraphObject).Name) ? "" : ((GraphObjectsList)GraphObject).Name;
                    break;
                case NumbersList _:
                    NameLab.Text = string.IsNullOrEmpty(((NumbersList)GraphObject).Name) ? "" : ((NumbersList)GraphObject).Name;
                    break;
            }
        }

        private void ErrorBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Error);
        }

    }
}
