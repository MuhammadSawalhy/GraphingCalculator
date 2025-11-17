using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Graphing
{

    public partial class Tab : UserControl
    {

        public Objects.Sketch Sketch;

        public Tab(string Header, Objects.Sketch sketch)
        {
            // This call is required by the designer.
            Sketch = sketch;
            InitializeComponent();
            // Add any initialization after the InitializeComponent() call.
            switch (MainFunctions.GetLanguage())
            {
                case MathPackage.Main.Language.AR:
                    ChangeNameToolStripMenuItem.Text = "تغيير الإسم";
                    break;
            }
            HeaderBtn.Text = Header;
        }

        public void Delete(System.Object sender, System.EventArgs e)
        {
            for(int i = 0; i < Module1.TabArray.Count; i++)
            {
                if(Module1.TabArray[i] == this)
                {
                    if (Module1.TabArray.Count > 1)
                         Module1.TabArray[i == 0 ? 1 : i - 1].Activate();
                    Module1.TabArray.Remove(this);
                }
            }
            Sketch.SketchControl.Parent.Controls.Remove(Sketch.SketchControl);
            Parent.Controls.Remove(this);
        }

        public void Activate()
        {
            Sketch.SketchControl.BringToFront();
            Sketch.UpdateAndDraw();
            Sketch.SketchControl.Draw();
            Module1.TargettedTab = this;
        }

        private void HeaderBtn_Click(object sender, EventArgs e)
        {
            Activate();
        }

        private void ChangeNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HeaderBtn.Text = Module1.ShowInput("Input a header for this tab", HeaderBtn.Text);
            Sketch.Name = HeaderBtn.Text;
        }

    }
}
