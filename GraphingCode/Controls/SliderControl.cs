using System;
using System.Drawing;
using System.Windows.Forms;
using Loyc;
using Loyc.Syntax;

namespace Graphing.Controls
{

    public partial class SliderControl : UserControl
    {

        public Objects.Slider Slider;

        public SliderControl(Objects.Slider slider)
        {
            Slider = slider;

            InitializeComponent();

            SliderTrack.ValueChanged += SliderTrack_ValueChanged;
            SliderBtn.Click += SliderBtn_Click;
            SliderSetting.Click += SliderSetting_Click;
            SliderMove.MouseDown += SliderMove_MouseDown;
            SliderMove.MouseUp += SliderMove_MouseUp;
            SliderMove.MouseMove += SliderMove_MouseMove;
        }

        #region "Grab"
        private bool isGrabed;
        private Point grabingPoint;

        private void SliderMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (Slider.GraphSetting.IsDelete)
                Slider.GraphSetting.Sketch.Sliders.Delete(Slider,true);
            else
            {
                isGrabed = true;
                grabingPoint = MousePosition;
            }
        }
        private void SliderMove_MouseUp(object sender, MouseEventArgs e)
        {
            isGrabed = false;
        }
        private void SliderMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (isGrabed)
            {
                if ((MousePosition.X - grabingPoint.X) > 0)
                {
                    if (this.Location.X + Width < Slider.GraphSetting.Width)
                        this.Left += (MousePosition.X - grabingPoint.X);
                }
                else if (this.Location.X > 0)
                    this.Left += (MousePosition.X - grabingPoint.X);

                if ((MousePosition.Y - grabingPoint.Y) > 0)
                {
                    if (this.Location.Y + Height < Slider.GraphSetting.Height)
                        this.Top += (MousePosition.Y - grabingPoint.Y);
                }
                else if (this.Location.Y > 0)
                    this.Top += (MousePosition.Y - grabingPoint.Y);


                grabingPoint = MousePosition;
            }
        }
        #endregion

        private void SliderTrack_ValueChanged(object sender, EventArgs e)
        {
            Slider.Value = (SliderTrack.Value * Slider.Increament + Slider.Interval_start);
            ValueBox.Text = Slider.Value.ToString();
            if (Slider.GraphSetting.SliderTimer.Enabled == false)
            {
                Slider.GraphSetting.Sketch.UpdateAndDraw();
                Slider.GraphSetting.Sketch.SketchControl.Draw();
                Slider.GraphSetting.Sketch.SketchControl.FlushMemory();
            }
        }

        private void SliderBtn_Click(object sender, EventArgs e)
        {

            if (Slider.Timer.Enabled)
            {
                Slider.Timer.Enabled = false;
                Slider.GraphSetting.UpdateSliderTimerState(Slider);
                SliderBtn.Image = Properties.Resources.Play;
            }
            else
            {
                Slider.Timer.Enabled = true;
                Slider.GraphSetting.UpdateSliderTimerState(Slider);
                SliderBtn.Image = Properties.Resources.Pause;
            }
        }

        private void SliderSetting_Click(object sender, EventArgs e)
        {
            Slider_Form f = new Slider_Form(Slider);
            f.ShowDialog();
        }

        private void ValueBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (MathPackage.Main.IsNumeric(ValueBox.Text))
                {
                    double value = double.Parse(ValueBox.Text);
                    if(value <= Slider.Interval_end && value >= Slider.Interval_start)
                    {
                        Slider.SetValue(value);
                    }
                    else
                    {
                        MessageBox.Show($"Your value \"{ValueBox.Text}\" is not inside [{Slider.Interval_start}, {Slider.Interval_end}].");
                    }
                }
                else
                {
                    MessageBox.Show($"Your value \"{ValueBox.Text}\" for this variable is not valid.");
                    ValueBox.Text = "";
                }
            }
        }
    }

}
