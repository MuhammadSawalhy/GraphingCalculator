
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Graphing.Controls;

namespace Graphing
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        public DataSet dataset_Save = new DataSet();

        private Microsoft.VisualBasic.Devices.Audio SoundPlayer = new Microsoft.VisualBasic.Devices.Audio();

        public void Playsound(string path_)
        {
            if (Properties.Settings.Default.PlaySound)
                SoundPlayer.Play(path_);
        }

        private void Graph_Load(object sender, EventArgs e)
        {
            CoorTypeComboBox.Items.Clear();
            switch (MainFunctions.GetLanguage())
            {
                case MathPackage.Main.Language.AR:
                    {
                        CoorTypeComboBox.Items.AddRange(new string[] {
                        "تخطيط مُعْتَاد",
                        "زوايا دائرية"
                    });
                        break;
                    }

                case MathPackage.Main.Language.EN:
                    {
                        CoorTypeComboBox.Items.AddRange(new string[] {
                        "Custom",
                        "Radian"
                    });
                        break;
                    }
            }
            try
            {
               ///AddSavedDrawings();
            } catch { }
        }

        void AddSavedDrawings()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Saving))
            {
                List<string> drawing = new List<string>(Properties.Settings.Default.Saving.Split('$'));
                for (int i = 0; i < drawing.Count; i++)
                {
                    AddSketchFromXml(System.Xml.Linq.XElement.Parse(drawing[i]));
                }
            }
        }

        void AddSketchFromXml(System.Xml.Linq.XElement text)
        {
            Objects.Sketch sketch = new Objects.Sketch();
            sketch.GraphSetting.GetDrawingFromXml(text);
            Tab tab = new Tab(sketch.Name, sketch);
            AddTab(tab);
            sketch.SketchControl.Dock = DockStyle.Fill;
            SketchPanel.Controls.Add(sketch.SketchControl);
            sketch.SketchControl.GraphPanel.MouseMove += WriteCoor;
            tab.Activate();
        }

        #region "Tabs"

        public string SelectHeaderName()
        {
            List<string> str = new List<string>();
            foreach (Tab TAB in Module1.TabArray)
                str.Add(TAB.HeaderBtn.Text);
            int i = 1;
            while (str.Contains("tab " + i))
                i += 1;
            return "tab " + i;
        }

        public void AddTab(Tab Tab)
        {
            Tab.Select();
            HeaderPanel.Width += Tab.Width;
            HeaderPanel.Controls.Add(Tab);
            Tab.BringToFront();
            Tab.Dock = DockStyle.Left;
            AddPanel.BringToFront();
            Module1.TabArray.Add(Tab);
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            string name = SelectHeaderName();
            Objects.Sketch sketch = new Objects.Sketch();
            sketch.Name = name;
            Tab tab = new Tab(name, sketch);
            AddTab(tab);
            sketch.SketchControl.Dock = DockStyle.Fill;
            sketch.SketchControl.BringToFront();
            SketchPanel.Controls.Add(sketch.SketchControl);
            sketch.SketchControl.GraphPanel.MouseMove += WriteCoor;
            sketch.GraphSetting.Center = new PointF(sketch.SketchControl.GraphPanel.Width / 2, sketch.SketchControl.GraphPanel.Height / 2);
            tab.Activate();
        }

        private void RightBtn_MouseDown(object sender, MouseEventArgs e)
        {
            if (HeaderPanel.Location.X + HeaderPanel.Width > TopPanel.Width)
                TimerTabsMoveLeft.Start();
        }

        private void RightBtn_MouseUp(object sender, MouseEventArgs e)
        {
            TimerTabsMoveLeft.Stop();
        }

        private void TimerTabsMoveLeft_Tick(object sender, EventArgs e)
        {
            if (HeaderPanel.Location.X + HeaderPanel.Width > Panel_.Width)
                HeaderPanel.Location = new Point(HeaderPanel.Location.X - 3, HeaderPanel.Location.Y);
            else
                TimerTabsMoveLeft.Stop();
        }

        private void LeftBtn_MouseDown(object sender, MouseEventArgs e)
        {
            if (HeaderPanel.Location.X < 0)
                TimerTabsMoveRight.Start();
        }

        private void LeftBtn_MouseUp(object sender, MouseEventArgs e)
        {
            TimerTabsMoveRight.Stop();
        }

        private void TimerTabsMoveRight_Tick(object sender, EventArgs e)
        {
            if (HeaderPanel.Location.X < 0)
                HeaderPanel.Location = new Point(HeaderPanel.Location.X + 3, HeaderPanel.Location.Y);
            else
                TimerTabsMoveRight.Stop();
        }

        #endregion

        private void WriteCoor(Object s, MouseEventArgs e)
        {
            coordinatesLabel.Text = MathPackage.Main.Approximate(((Graphing.Controls.SketchControl)((Control)s).Parent).GraphSetting.xChangeToCoor(e.Location.X, e.Location.Y), "###") + ", " + MathPackage.Main.Approximate(((Graphing.Controls.SketchControl)((Control)s).Parent).GraphSetting.yChangeToCoor(e.Location.X, e.Location.Y), "###");
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Max_Normal_Btn_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string saving = "";
            try
            {
                foreach (Tab tab in Module1.TabArray)
                {
                    if (saving == "")
                        saving += tab.Sketch.GraphSetting.GetAllAsXml().ToString();
                    else
                        saving += "$" + tab.Sketch.GraphSetting.GetAllAsXml().ToString();
                }
            }
            catch { }
            Properties.Settings.Default.Saving = saving;
            Properties.Settings.Default.Save();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Xml file (*.xml)|*.txt";
            if (save.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(save.FileName);
                System.Xml.Linq.XElement value = Module1.TargettedTab.Sketch.GraphSetting.GetAllAsXml();
                value.SetAttributeValue("Name", Module1.TargettedTab.HeaderBtn.Text);
                file.Write(value.ToString());
                file.Dispose();
            }
        }

        private void BunifuFlatButton4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "Xml file (*.xml)|*.txt";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader fileReader = new System.IO.StreamReader(openfile.FileName);
                AddSketchFromXml(System.Xml.Linq.XElement.Parse(fileReader.ReadToEnd()));
                fileReader.Dispose();
            }
        }

        Point p;
        bool ismousedown = false;

        private void Panel4_MouseDown(object sender, MouseEventArgs e) 
        {
            ismousedown = true;
            p = MousePosition;
        }

        private void Panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (ismousedown)
            {
                Left += MousePosition.X - p.X;
                Top += MousePosition.Y - p.Y;
                p = MousePosition;
            }
        }

        private void Panel4_MouseUp(object sender, MouseEventArgs e)
        {
            ismousedown = false;
        }
    }
}
