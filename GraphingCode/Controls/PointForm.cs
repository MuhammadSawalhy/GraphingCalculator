using System;
using System.Drawing;
using System.Windows.Forms;
using Loyc;
using Loyc.Syntax;

namespace Graphing.Controls
{
    public partial class PointForm : Form
    {

        public Objects.Points Point;
        private FormType Form_Type;
        private bool isloaded = false;
        public enum FormType
        {
            Add,
            Adjust
        }

        public PointForm(Objects.Points point_)
        {
            Point = point_;
            Form_Type = FormType.Adjust;

            // This call is required by the designer.
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            LNode x = LNode.Literal(XTextBox.Text);
            LNode y = LNode.Literal(YTextBox.Text);

            Point.X_Value = MathPackage.Transformer.GetNodeFromLoycNode(x);
            Point.Y_Value = MathPackage.Transformer.GetNodeFromLoycNode(y);

            Close();

        }

        private void PointForm_Load(object sender, EventArgs e)
        {
            if (Form_Type == FormType.Adjust)
            {
                XTextBox.Text = Point.X_Value.ToString();
                YTextBox.Text = Point.Y_Value.ToString();
                Button1.Text = "Modify";
            }
            else
                Button1.Text = "Add";

            isloaded = true;
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            if (isloaded == true)
            {
                Graphics Graphics = e.Graphics;
                Graphics.Clear(Color.White);
                GraphSetting GraphSetting = new GraphSetting(null/* TODO Change to default(_) if this is not a reference type */);
                GraphSetting.Width = Panel1.Width;
                GraphSetting.Height = Panel1.Height;
                GraphSetting.Center = new Point((int)(Panel1.Width / (double)2), (int)(Panel1.Height / (double)2));
                Objects.Points p = new Objects.Points(GraphSetting);
                p.X_Value = new MathPackage.Operations.Constant(0);
                p.Y_Value = new MathPackage.Operations.Constant(0);
                Bitmap b = new Bitmap(GraphSetting.Width, GraphSetting.Height);
                p.Draw();
                p.DrawTo(Graphics);
                Graphics.DrawImage(b, 0, 0);
            }
        }
    }

}
