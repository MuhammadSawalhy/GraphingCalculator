
namespace Graphing.Controls
{
  using System;
using System.Drawing;
using System.Windows.Forms;
  public partial class Edit_Pen : Form
    {


        private Pen pen_ = new Pen(Color.AliceBlue);
        private Pen RefPen;
        public Pen Pen
        {
            get
            {
                using (Pen p = new Pen(pen_.Color, pen_.Width))
                {
                    p.DashStyle = pen_.DashStyle;
                    return p;
                }
            }
            set
            {
                pen_ = value;
                Draw_pen();
                Update_Controls();
            }
        }

        public Edit_Pen(Pen _pen)
        {

            pen_.Width = _pen.Width;
            pen_.Color  = _pen.Color;
            pen_.DashStyle = _pen.DashStyle;
            RefPen = _pen;
            // This call is required by the designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            Update_Controls();
        }

        public void Update_Controls()
        {
            Style_.SelectedIndex = (int)pen_.DashStyle;
            NumericUpDown1.Value = (decimal)pen_.Width;
            Color_btn.BackColor = pen_.Color;
        }
   
        private void Edit_pen_Load(System.Object sender, System.EventArgs e)
        {
            
            switch (MainFunctions.GetLanguage())
            {
                case MathPackage.Main.Language.AR:
                    {
                        Label2.Text = "اللون";
                        Label8.Text = "الاتساع";
                        Button1.Text = "تم";
                        Text = "تعديل القلم";
                        break;
                    }
            }



        }

        private void Button1_Click(System.Object sender, System.EventArgs e)
        {
            UpdatePen();
            Close();
        }

        private void UpdatePen()
        {
            RefPen.Width = pen_.Width;
            RefPen.DashStyle = pen_.DashStyle;
            RefPen.Color = pen_.Color;
        }

        private void Style__SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            pen_.DashStyle = (System.Drawing.Drawing2D.DashStyle)Style_.SelectedIndex;
            Draw_pen();
        }

        private void NumericUpDown1_ValueChanged(System.Object sender, System.EventArgs e)
        {
            pen_.Width = (float)NumericUpDown1.Value;
            Draw_pen();
        }
        private void Color_btn_Click(System.Object sender, System.EventArgs e)
        {
            ColorDialog1.ShowDialog();
            Color_btn.BackColor = ColorDialog1.Color;
            pen_.Color = ColorDialog1.Color;
            Draw_pen();
        }
        public void Draw_pen()
        {
            Label1.CreateGraphics().Clear(Label1.BackColor);
            Label1.CreateGraphics().SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Label1.CreateGraphics().CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            Label1.CreateGraphics().DrawLine(pen_, 0, Convert.ToInt16(Label1.Height / (double)2), Label1.Width, Convert.ToInt16(Label1.Height / (double)2));
        }
        private void Edit_pen_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Draw_pen();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}

