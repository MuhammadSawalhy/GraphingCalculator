using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loyc.Syntax;
using Loyc;

namespace MathPackage
{
    public class CalculationSetting
    {

        public CalculationSetting(Main.AngleType angleType = Main.AngleType.Radian, Graphing.GraphSetting GraphSetting_ = null)
        {
            GraphSetting = GraphSetting_;
            AngleType = angleType;

            List<Graphing.Variable> vars = new List<Graphing.Variable>();
            vars.Add(new Graphing.Variable((Symbol)"x", new Operations.Constant(double.NaN)));
            vars.Add(new Graphing.Variable((Symbol)"y", new Operations.Constant(Math.PI)));
            vars.Add(new Graphing.Variable((Symbol)"nth", new Operations.Constant(double.NaN)));
            vars.Add(new Graphing.Variable((Symbol)"pi", new Operations.Constant(Math.PI)));
            vars.Add(new Graphing.Variable((Symbol)"e", new Operations.Constant(Math.E)));
            vars.Add(new Graphing.Variable((Symbol)"phi", new Operations.Constant(double.NaN)));
            Vars = vars;
        }
        /// <summary>
        /// the pre-added vars, {0:x, 1:y, 2:nth, 3:pi, 4:E, 5:phi, ...}
        /// </summary>
        public List<Graphing.Variable> Vars = new List<Graphing.Variable>();
        public List<Operations.Func> Funcs = new List<MathPackage.Operations.Func>();

        public MathPackage.Main.AngleType AngleType;
        public Graphing.GraphSetting GraphSetting;
        
    }
}
