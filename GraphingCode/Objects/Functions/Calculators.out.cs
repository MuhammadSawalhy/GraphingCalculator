// Generated from Calculators.ecs by LeMP custom tool. LeMP version: 2.5.2.0
// Note: you can give command-line arguments to the tool via 'Custom Tool Namespace':
// --no-out-header       Suppress this message
// --verbose             Allow verbose messages (shown by VS as 'warnings')
// --timeout=X           Abort processing thread after X seconds (default: 10)
// --macros=FileName.dll Load macros from FileName.dll, path relative to this file 
// Use #importMacros to use macros in a given namespace, e.g. #importMacros(Loyc.LLPG);
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loyc;
using Loyc.Syntax;
using Loyc.Collections;
using Graphing;
using number = System.Double;	// Change this line to make a calculator for a different data type 
public class CalcRange
{
    public number Lo;
    public number Hi;
    public int StepsCount;
    // Generate a constructor and three public fields
    public CalcRange(number lo, number hi, int pxCount)
    {
        Lo = lo;
        Hi = hi;
        StepsCount = pxCount;
        StepSize = (Hi - Lo) / Math.Max(StepsCount - 1, 1)/20;
    }
    public number StepSize;

    public number ValueToPx(number value) => (value - Lo) / (Hi - Lo) * StepsCount;
    public number PxToValue(int px) => (number)px / StepsCount * (Hi - Lo) + Lo;
    public number PxToDelta(int px) => (number)px / StepsCount * (Hi - Lo);
    public CalcRange DraggedBy(int dPx) =>
    new CalcRange(Lo - PxToDelta(dPx), Hi - PxToDelta(dPx), StepsCount);
    public CalcRange ZoomedBy(number ratio)
    {
        double mid = (Hi + Lo) / 2, halfSpan = (Hi - Lo) * ratio / 2;
        return new CalcRange(mid - halfSpan, mid + halfSpan, StepsCount);
    }
}

// "alt class" generates an entire class hierarchy with base class CalculatorCore and 
// read-only fields. Each "alternative" (derived class) is marked with the word "alt".
public abstract class CalculatorCore
{

    public static GraphSetting GraphSetting;

    // Base class constructor and fields
    public CalculatorCore(LNode Expr, GraphSetting GraphSetting_)
    {
        GraphSetting = GraphSetting_;
        this.Expr = Expr;
    }

    public LNode Expr { get; private set; }
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public LNode Item1
    {
        get
        {
            return Expr;
        }
    }

    public object Results { get; protected set; }

    public abstract number? GetValueAt(int x, int y);

    public static CalculatorCore New(LNode expr, GraphSetting GraphSetting_)
    {
        // Find out if the expression uses the variable "y" (or is an equation with '=' or '==')
        // As an (unnecessary) side effect, this throws if an unreferenced var is used
        bool isEquation = expr.Calls(CodeSymbols.Assign, 2) || expr.Calls(CodeSymbols.Eq, 2), usesY = false;
        if (!isEquation)
        {
            LNode zero = LNode.Literal((double)0);
            Func<Symbol, double> lookup = null;
            lookup = name => name == (Symbol)"x" || (usesY |= name == (Symbol)"y") ? 0 : MainFunctions.Eval(GraphSetting.Vars[name], lookup);
            MainFunctions.Eval(expr, lookup);
        }
        if (isEquation || usesY)
            return null;
        //return new Calculator3D(expr, GraphSetting_);
        else
            return new Calculator2D(expr, GraphSetting_);
    }

    // Parse the list of variables provided in the GUI
    public static Dictionary<Symbol, LNode> ParseVarList(IEnumerable<LNode> varList)
    {
        var vars = new Dictionary<Symbol, LNode>();
        foreach (LNode assignment in varList)
        {
            {
                LNode expr, @var;
                if (assignment.Calls(CodeSymbols.Assign, 2) && (@var = assignment.Args[0]) != null && (expr = assignment.Args[1]) != null)
                {
                    if (!@var.IsId)
                        throw new ArgumentException("Left-hand side of '=' must be a variable name: {0}".Localized(@var));

                    // For efficiency, try to evaluate the expression in advance
                    try { expr = LNode.Literal(MainFunctions.Eval(expr,GraphSetting.Vars)); } catch { }   // it won't work if expression uses X or Y
                    vars.Add(@var.Name, expr);
                }
                else
                    throw new ArgumentException("Expected assignment expression: {0}".Localized(assignment));
            };
        }
        return vars;
    }
 
}

// Derived class for 2D graphing calculator
public class Calculator2D : CalculatorCore
{
    static readonly Symbol sy_x = (Symbol)"x";
    public Calculator2D(LNode Expr, GraphSetting GraphSetting_)
         : base(Expr, GraphSetting_) { }

    public override number? GetValueAt(int x, int _)
    {
        var tmp_14 = (uint)x;
        var r = ((number[])Results);
        return
        tmp_14 < (uint)r.Length ? r[x] : (number?)null;
    }

}

// Derived class for pseudo-3D and "equation" calculator
public class Calculator3D : CalculatorCore
{
//    static readonly Symbol sy_x = (Symbol)"x", sy_y = (Symbol)"y";
    public Calculator3D(LNode Expr, GraphSetting GraphSetting_)
         : base(Expr, GraphSetting_)
    {

    }
    //    public CalcRange YRange { get; private set; }
    //    //public override CalculatorCore WithExpr(LNode newValue)
    //    //{
    //    //    return new Calculator3D(newValue, Vars, XRange, YRange);
    //    //}
    //    //public override CalculatorCore WithVars(Dictionary<Symbol, LNode> newValue)
    //    //{
    //    //    return new Calculator3D(Expr, newValue, XRange, YRange);
    //    //}
    //    //public override CalculatorCore WithXRange(CalcRange newValue)
    //    //{
    //    //    return new Calculator3D(Expr, Vars, newValue, YRange);
    //    //}
    //    //public Calculator3D WithYRange(CalcRange newValue)
    //    //{
    //    //    return new Calculator3D(Expr, Vars, XRange, newValue);
    //    //}

    //    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    //    public CalcRange Item4
    //    {
    //        get
    //        {
    //            return YRange;
    //        }
    //    }
    public bool EquationMode { get; private set; }

    //    public number[,] RunCore(LNode expr, bool difMode)
    //    {
    //        var results = new number
    //        [YRange.StepsCount + (difMode ? 1 : 0), GraphSetting.XRange.StepsCount + (difMode ? 1 : 0)];
    //        number x = GraphSetting.XRange.Lo, startx = x;
    //        number y = YRange.Lo;
    //        if (difMode)
    //        {
    //            x -= GraphSetting.XRange.StepSize / 2;
    //            y -= YRange.StepSize / 2;
    //        }

    //        Func<Symbol, number> lookup = null;
    //        lookup = name => (name == sy_x ? x : name == sy_y ? y : Eval(GraphSetting.Vars[name], lookup));

    //        for (int yi = 0; yi < results.GetLength(0); yi++, x = startx)
    //        {
    //            for (int xi = 0; xi < results.GetLength(1); xi++)
    //            {
    //                results[yi, xi] = Eval(expr, lookup);
    //                x += GraphSetting.XRange.StepSize;
    //            }
    //            y += YRange.StepSize;
    //        }
    //        return results;
    //    }
    public override number? GetValueAt(int x, int y)
    {
        var tmp_15 = (uint)x;
        var r = ((number[,])Results);
        return
        tmp_15 < (uint)r.GetLength(1) &&
        (uint)y < (uint)r.GetLength(0) ? r[y, x] : (number?)null;
    }
}

