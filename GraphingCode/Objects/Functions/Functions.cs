using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using Loyc.Syntax;
namespace Graphing.Objects
{
   public abstract class Function : GraphObject
    {

        public virtual MathPackage.Node Expression { get; set; }

        public bool IsSelected = false;

        public override string Get_Type => "Functions";

        public Function(GraphSetting GraphSetting_, Controls.GraphControl Control = null) : base(GraphSetting_,Control)
        {
            if (MainFunctions.IsColorDark(GraphSetting_.Sketch.Coordinates.CoorSetting.BackColor)) { Pen = new Pen(MainFunctions.RandomLightColor(), 2); } else { Pen = new Pen(MainFunctions.RandomDarkColor(), 2); }
        }

        public override string DiscriptionString() => null;
        public override string ToString()
        {
            throw new NotImplementedException();
        }

       
    }
}
