using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphing.Objects
{
    public class Slider : GraphObject
    {

        public Slider(string name, GraphSetting GraphSetting_)
        {
            GraphSetting = GraphSetting_;
            SliderControl = new Controls.SliderControl(this);
            SetName(name);
        }
        public Slider(GraphSetting GraphSetting_)
        {
            GraphSetting = GraphSetting_;
            SliderControl = new Controls.SliderControl(this);
        }

        #region Varaibles

        public Controls.SliderControl SliderControl;

        protected double value_ = 1;
        /// <summary>
        /// this will change the value of the <see cref="Variable"/> in <see cref="MathPackage.CalculationSetting"/>
        /// </summary>
        public double Value
        {
            get
            {
                return value_;
            }
            set
            {
                value_ = value;
                for(int i =0;i < GraphSetting.CalculationSetting.Vars.Count; i++)
                {
                    if(GraphSetting.CalculationSetting.Vars[i].Name.Name == Name)
                        GraphSetting.CalculationSetting.Vars[i].Value = new MathPackage.Operations.Constant(value);
                }
            }
        }

        public double Increament = 0.1;

        public void ModifyValues()
        {
            try
            {
                if (Increament > (Interval_end - Interval_start))
                    Increament = (Interval_end - Interval_start);

                SliderControl.SliderTrack.Maximum = (int)((Interval_end - Interval_start) / Increament);
            }
            catch
            {
            }
        }

        public ValueType ValueType_;

        protected OscillationType OscillationType__ = OscillationType.UpDown;
        public OscillationType OscillationType_
        {
            get
            {
                return OscillationType__;
            }
            set
            {
                OscillationType__ = value;
                RemoveTimerEvents();
                switch (value)
                {
                    case OscillationType.Down:
                        {
                            Timer.Tick += Down;
                            direct = -1;
                            break;
                        }

                    case OscillationType.Up:
                        {
                            Timer.Tick += Up;
                            direct = 1;
                            break;
                        }

                    case OscillationType.DownOnce:
                        {
                            Timer.Tick += DownOnce;
                            direct = -1;
                            break;
                        }

                    case OscillationType.UpOnce:
                        {
                            Timer.Tick += UpOnce;
                            direct = 1;
                            break;
                        }

                    case OscillationType.UpDown:
                        {
                            Timer.Tick += UpDown;
                            direct = 1;
                            break;
                        }
                }
            }
        }

        public double Interval_start = -5;

        public double Interval_end = 5;

        public enum ValueType
        {
            Integer_,
            Decimal_,
            Angle_
        }

        public enum OscillationType
        {
            UpDown,
            Up,
            Down,
            UpOnce,
            DownOnce
        }
        #endregion

        #region Timer

        public System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();

        public void RemoveTimerEvents()
        {
            Timer.Tick -= Down;

            Timer.Tick -= Up;

            Timer.Tick -= DownOnce;

            Timer.Tick -= UpOnce;

            Timer.Tick -= UpDown;
        }
        
        public Int32 direct = 1;

        public void Up(object sender, EventArgs e)
        {
            if (SliderControl.SliderTrack.Value == SliderControl.SliderTrack.Maximum)
                SliderControl.SliderTrack.Value = 0;
            else
                SliderControl.SliderTrack.Value += direct;
        }
        public void Down(object sender, EventArgs e)
        {
            if (SliderControl.SliderTrack.Value == 0)
                SliderControl.SliderTrack.Value = SliderControl.SliderTrack.Maximum;
            else
                SliderControl.SliderTrack.Value += direct;
        }
        public void UpOnce(object sender, EventArgs e)
        {
            if (SliderControl.SliderTrack.Value == SliderControl.SliderTrack.Maximum)
            {
                if (Timer.Enabled == true)
                {
                    Timer.Stop();
                    SliderControl.SliderBtn.Image = Properties.Resources.Play;
                }
                else
                    SliderControl.SliderTrack.Value = 0;
            }
            else
                SliderControl.SliderTrack.Value += direct;
        }
        public void DownOnce(object sender, EventArgs e)
        {
            if (SliderControl.SliderTrack.Value == 0)
            {
                if (Timer.Enabled == true)
                {
                    Timer.Stop();
                    SliderControl.SliderBtn.Image = Properties.Resources.Play;
                }
                else
                    SliderControl.SliderTrack.Value = SliderControl.SliderTrack.Maximum;
            }
            else
                SliderControl.SliderTrack.Value += direct;
        }
        public void UpDown(object sender, EventArgs e)
        {
            try
            {
                if (SliderControl.SliderTrack.Value == SliderControl.SliderTrack.Maximum || SliderControl.SliderTrack.Value == 0)
                {
                    direct *= -1;
                    SliderControl.SliderTrack.Value += direct;
                }
                else
                    SliderControl.SliderTrack.Value += direct;
            }
            catch
            {
            }
        }

        #endregion

        #region Name

        public override void SetName(string Name_)
        {
            MainFunctions.CheckName(Name_);

            if (GraphSetting.IsNameUsed(Name_, new List<string>(new[] { Name })))
            {
                throw new Exception($"This name \"{Name_}\" has already been used.");
            }

            bool found = false;
           
            /// If the variable does exist, then change (variable name) and (this.Name).
            for (int i = 0; i < GraphSetting.CalculationSetting.Vars.Count; i++)
            {
                if (GraphSetting.CalculationSetting.Vars[i].Name.Name == Name)
                {
                    /// Change Var.Name
                    GraphSetting.ChangeVarKey((Loyc.Symbol)Name_, (Loyc.Symbol)Name);
                    Name = Name_;
                    found = true;
                }
            }
          
            /// If the variable doesn't exist, then create new one and add the variable. 
            if (!found)
            {
                if (Name_ != null)
                {
                    GraphSetting.CalculationSetting.Vars.Add(new Variable((Loyc.Symbol)Name_, new MathPackage.Operations.Constant(Value)));
                    Name = Name_;
                }
            }

            SliderControl.HeaderLab.Text = Name_;
        }
       
        #endregion

        /// <summary>
        /// SetValue method is used to change the value of this slider as a consequence of changing the SliderTrack value 
        /// </summary>
        public void SetValue(double value)
        {
            SliderControl.SliderTrack.Value = (int)((value - Interval_start) / Increament);
        }

        public override string DiscriptionString()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override System.Xml.Linq.XElement GetAsXml()
        {
            System.Xml.Linq.XElement element = new System.Xml.Linq.XElement("Variable");
            element.SetAttributeValue("Name", Name);
            element.SetAttributeValue("Value", Value.ToString());
            string Values = "";
            Values += $"Start = {Interval_start.ToString()},";
            Values += $"End = {Interval_end.ToString()},";
            Values += $"Increament = {Increament.ToString()},";
            Values += $"Oscillation = {OscillationType_.ToString()},";
            Values += $"Speed = {Timer.Interval.ToString()}";
            element.SetAttributeValue("Slider", "{" + Values + "}");
            return element;
        }

        public override void UpdateScriptText()
        {
            throw new NotImplementedException();
        }
    }
}
