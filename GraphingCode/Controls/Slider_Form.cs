using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualBasic;
using Graphing.Objects;

namespace Graphing.Controls
{
    public partial class Slider_Form : Form
    {
        private FormType FormType_;
        private Slider SliderReference;
        private bool isloaded = false;

        /// <summary>
        ///     ''' When you create a new form
        ///     ''' </summary>
        ///     ''' <param name="slider_">Slider to adjust</param>
        ///     '''
        ///     
        public Slider_Form(Slider slider_)
        {

            FormType_ = FormType.Adjust;
            SliderReference = slider_;
          
            // Add any initialization after the InitializeComponent() call.
            // This call is required by the designer.
            InitializeComponent();


            HeaderText.Text = slider_.Name;
            IncreamentText.Text = SliderReference.Increament.ToString();
            NumericUpDown1.Value = SliderReference.Timer.Interval;

            switch (MainFunctions.GetLanguage())
            {
                case MathPackage.Main.Language.AR:
                    {
                        interval2.Text = SliderReference.Interval_start.ToString();
                        interval1.Text = SliderReference.Interval_end.ToString();
                        break;
                    }

                case MathPackage.Main.Language.EN:
                    {
                        interval1.Text = SliderReference.Interval_start.ToString();
                        interval2.Text = SliderReference.Interval_end.ToString();
                        break;
                    }
            }

            switch (SliderReference.ValueType_)
            {
                case Slider.ValueType.Integer_:
                    {
                        Radioint.Checked = true;
                        break;
                    }

                case Slider.ValueType.Angle_:
                    {
                        Radioangle.Checked = true;
                        break;
                    }

                case Slider.ValueType.Decimal_:
                    {
                        Radionum.Checked = true;
                        break;
                    }
            }

            ComboBox1.SelectedIndex = (int)(SliderReference.OscillationType_);

            Button1.Text = "Adjust";

            isloaded = true;

        }

        /// <summary>
        ///     ''' When you create a new form
        ///     ''' </summary>
        public Slider_Form(Point location_,GraphSetting graphSetting)
        {

            // Add any initialization after the InitializeComponent() call.
            FormType_ = FormType.Add;
            SliderReference = new Slider(graphSetting);
            SliderReference.SliderControl.Location = location_;
            isloaded = true;
            // This call is required by the designer.

            InitializeComponent();

            UpdateVaues(SliderReference, false);

            HeaderText.Text = SliderReference.GraphSetting.SelectName();

            Button1.Text = "Add";

            switch (MainFunctions.GetLanguage())
            {
                case MathPackage.Main.Language.AR:
                    {
                        interval1.Text = "5";
                        interval2.Text = "-5";
                        break;
                    }

                case MathPackage.Main.Language.EN:
                    {
                        interval1.Text = "-5";
                        interval2.Text = "5";
                        break;
                    }
            }

        }

        public enum FormType
        {
            Add,
            Adjust
        }

        private void UpdateVaues(Slider slider,bool SetName)
        {

            slider.Increament = MathPackage.Main.CalculateString(IncreamentText.Text, slider.GraphSetting.CalculationSetting);
            slider.Interval_start = MathPackage.Main.CalculateString(interval1.Text, slider.GraphSetting.CalculationSetting);
            slider.Interval_end = MathPackage.Main.CalculateString(interval2.Text, slider.GraphSetting.CalculationSetting);
            slider.Timer.Interval = (int)NumericUpDown1.Value;
            slider.OscillationType_ = (Slider.OscillationType)ComboBox1.SelectedIndex;

            if (Radionum.Checked) slider.ValueType_ = Slider.ValueType.Decimal_;
            else if (Radioint.Checked) slider.ValueType_ = Slider.ValueType.Integer_;
            else if (Radioangle.Checked) slider.ValueType_ = Slider.ValueType.Angle_;

            if (SetName)
            {
                slider.SetName(HeaderText.Text);
            }

        }

        private void Button1_Click(System.Object sender, System.EventArgs e)
        {
            try {
                if (Radioint.Checked && (MathPackage.Main.CalculateString(IncreamentText.Text, SliderReference.GraphSetting.CalculationSetting) % 1 != 0 || MathPackage.Main.CalculateString(interval1.Text, SliderReference.GraphSetting.CalculationSetting) % 1 != 0 || MathPackage.Main.CalculateString(interval2.Text, SliderReference.GraphSetting.CalculationSetting) % 1 != 0) || MathPackage.Main.CalculateString(IncreamentText.Text, SliderReference.GraphSetting.CalculationSetting) == 0 || (MathPackage.Main.CalculateString(interval1.Text, SliderReference.GraphSetting.CalculationSetting) >= MathPackage.Main.CalculateString(interval2.Text, SliderReference.GraphSetting.CalculationSetting)))
                {
                    string str = "";
                    switch (MainFunctions.GetLanguage())
                    {
                        case MathPackage.Main.Language.AR:
                            {
                                str = "راجع مدخلاتك.";
                                break;
                            }

                        case MathPackage.Main.Language.EN:
                            {
                                str = "Revise your inputs.";
                                break;
                            }
                    }
                    throw new Exception(str);
                }

                UpdateVaues(SliderReference, true);

                SliderReference.ModifyValues();

                if (FormType_ == FormType.Add)
                {

                    if (SliderReference.OscillationType_ == Slider.OscillationType.Up || SliderReference.OscillationType_ == Slider.OscillationType.UpOnce)
                        SliderReference.SliderControl.SliderTrack.Value = 0;
                    else if (SliderReference.OscillationType_ == Slider.OscillationType.Down || SliderReference.OscillationType_ == Slider.OscillationType.DownOnce)
                        SliderReference.SliderControl.SliderTrack.Value = SliderReference.SliderControl.SliderTrack.Maximum;
                    else
                        SliderReference.SliderControl.SliderTrack.Value = (int)Math.Round(SliderReference.SliderControl.SliderTrack.Maximum / (double)2);

                    SliderReference.GraphSetting.Sketch.Sliders.Add(SliderReference);
                    SliderReference.GraphSetting.Sketch.SketchControl.GraphPanel.Controls.Add(SliderReference.SliderControl);

                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }



    }


}
