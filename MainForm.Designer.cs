namespace Graphing
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.SketchPanel = new System.Windows.Forms.Panel();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.Panel_ = new System.Windows.Forms.Panel();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.AddPanel = new System.Windows.Forms.Panel();
            this.AddBtn = new System.Windows.Forms.Button();
            this.RightBtn = new System.Windows.Forms.Button();
            this.LeftBtn = new System.Windows.Forms.Button();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.FuncDrawing_Polar = new System.Windows.Forms.RadioButton();
            this.FuncDrawing_Oscillate = new System.Windows.Forms.RadioButton();
            this.FuncDrawing_Custom = new System.Windows.Forms.RadioButton();
            this.Label16 = new System.Windows.Forms.Label();
            this.Panel7 = new System.Windows.Forms.Panel();
            this.CoorTypeComboBox = new System.Windows.Forms.ComboBox();
            this.coordinatesLabel = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Panel4 = new System.Windows.Forms.Panel();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.RecordBtn = new System.Windows.Forms.Button();
            this.Panel6 = new System.Windows.Forms.Panel();
            this.BunifuFlatButton4 = new System.Windows.Forms.Button();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.Max_Normal_Btn = new System.Windows.Forms.Button();
            this.MinimizeBtn = new System.Windows.Forms.Button();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.SettingBtn = new System.Windows.Forms.Button();
            this.TimerTabsMoveLeft = new System.Windows.Forms.Timer(this.components);
            this.TimerTabsMoveRight = new System.Windows.Forms.Timer(this.components);
            this.MainPanel.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.Panel_.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            this.AddPanel.SuspendLayout();
            this.Panel3.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.Panel7.SuspendLayout();
            this.Panel4.SuspendLayout();
            this.Panel6.SuspendLayout();
            this.Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.SketchPanel);
            this.MainPanel.Controls.Add(this.TopPanel);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 41);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(930, 726);
            this.MainPanel.TabIndex = 45;
            // 
            // SketchPanel
            // 
            this.SketchPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SketchPanel.Location = new System.Drawing.Point(0, 40);
            this.SketchPanel.Name = "SketchPanel";
            this.SketchPanel.Size = new System.Drawing.Size(930, 686);
            this.SketchPanel.TabIndex = 79;
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.Panel_);
            this.TopPanel.Controls.Add(this.RightBtn);
            this.TopPanel.Controls.Add(this.LeftBtn);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(930, 40);
            this.TopPanel.TabIndex = 78;
            // 
            // Panel_
            // 
            this.Panel_.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.Panel_.Controls.Add(this.HeaderPanel);
            this.Panel_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_.Location = new System.Drawing.Point(15, 0);
            this.Panel_.Name = "Panel_";
            this.Panel_.Size = new System.Drawing.Size(900, 40);
            this.Panel_.TabIndex = 2;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.Transparent;
            this.HeaderPanel.Controls.Add(this.AddPanel);
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(40, 40);
            this.HeaderPanel.TabIndex = 6;
            // 
            // AddPanel
            // 
            this.AddPanel.Controls.Add(this.AddBtn);
            this.AddPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.AddPanel.Location = new System.Drawing.Point(0, 0);
            this.AddPanel.Name = "AddPanel";
            this.AddPanel.Size = new System.Drawing.Size(40, 40);
            this.AddPanel.TabIndex = 6;
            // 
            // AddBtn
            // 
            this.AddBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.AddBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddBtn.Dock = System.Windows.Forms.DockStyle.Left;
            this.AddBtn.FlatAppearance.BorderSize = 0;
            this.AddBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddBtn.Font = new System.Drawing.Font("Segoe MDL2 Assets", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddBtn.ForeColor = System.Drawing.Color.White;
            this.AddBtn.Location = new System.Drawing.Point(0, 0);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(40, 40);
            this.AddBtn.TabIndex = 1;
            this.AddBtn.Text = "";
            this.AddBtn.UseVisualStyleBackColor = false;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // RightBtn
            // 
            this.RightBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.RightBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightBtn.FlatAppearance.BorderSize = 0;
            this.RightBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RightBtn.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10F);
            this.RightBtn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.RightBtn.Location = new System.Drawing.Point(915, 0);
            this.RightBtn.Name = "RightBtn";
            this.RightBtn.Size = new System.Drawing.Size(15, 40);
            this.RightBtn.TabIndex = 2;
            this.RightBtn.TabStop = false;
            this.RightBtn.Text = "";
            this.RightBtn.UseVisualStyleBackColor = false;
            this.RightBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RightBtn_MouseDown);
            this.RightBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RightBtn_MouseUp);
            // 
            // LeftBtn
            // 
            this.LeftBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.LeftBtn.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftBtn.FlatAppearance.BorderSize = 0;
            this.LeftBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LeftBtn.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeftBtn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.LeftBtn.Location = new System.Drawing.Point(0, 0);
            this.LeftBtn.Name = "LeftBtn";
            this.LeftBtn.Size = new System.Drawing.Size(15, 40);
            this.LeftBtn.TabIndex = 1;
            this.LeftBtn.TabStop = false;
            this.LeftBtn.Text = "";
            this.LeftBtn.UseVisualStyleBackColor = false;
            this.LeftBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LeftBtn_MouseDown);
            this.LeftBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LeftBtn_MouseUp);
            // 
            // Panel3
            // 
            this.Panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(175)))));
            this.Panel3.Controls.Add(this.Panel1);
            this.Panel3.Controls.Add(this.Panel7);
            this.Panel3.Controls.Add(this.coordinatesLabel);
            this.Panel3.Controls.Add(this.Label11);
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel3.Location = new System.Drawing.Point(0, 767);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(930, 24);
            this.Panel3.TabIndex = 43;
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.FuncDrawing_Polar);
            this.Panel1.Controls.Add(this.FuncDrawing_Oscillate);
            this.Panel1.Controls.Add(this.FuncDrawing_Custom);
            this.Panel1.Controls.Add(this.Label16);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Panel1.Location = new System.Drawing.Point(356, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(377, 24);
            this.Panel1.TabIndex = 43;
            // 
            // FuncDrawing_Polar
            // 
            this.FuncDrawing_Polar.AutoSize = true;
            this.FuncDrawing_Polar.Location = new System.Drawing.Point(316, 4);
            this.FuncDrawing_Polar.Name = "FuncDrawing_Polar";
            this.FuncDrawing_Polar.Size = new System.Drawing.Size(49, 17);
            this.FuncDrawing_Polar.TabIndex = 32;
            this.FuncDrawing_Polar.TabStop = true;
            this.FuncDrawing_Polar.Text = "Polar";
            this.FuncDrawing_Polar.UseVisualStyleBackColor = true;
            // 
            // FuncDrawing_Oscillate
            // 
            this.FuncDrawing_Oscillate.AutoSize = true;
            this.FuncDrawing_Oscillate.Location = new System.Drawing.Point(220, 4);
            this.FuncDrawing_Oscillate.Name = "FuncDrawing_Oscillate";
            this.FuncDrawing_Oscillate.Size = new System.Drawing.Size(73, 17);
            this.FuncDrawing_Oscillate.TabIndex = 31;
            this.FuncDrawing_Oscillate.TabStop = true;
            this.FuncDrawing_Oscillate.Text = "Oscillation";
            this.FuncDrawing_Oscillate.UseVisualStyleBackColor = true;
            // 
            // FuncDrawing_Custom
            // 
            this.FuncDrawing_Custom.AutoSize = true;
            this.FuncDrawing_Custom.Checked = true;
            this.FuncDrawing_Custom.Location = new System.Drawing.Point(128, 4);
            this.FuncDrawing_Custom.Name = "FuncDrawing_Custom";
            this.FuncDrawing_Custom.Size = new System.Drawing.Size(60, 17);
            this.FuncDrawing_Custom.TabIndex = 30;
            this.FuncDrawing_Custom.TabStop = true;
            this.FuncDrawing_Custom.Text = "Custom";
            this.FuncDrawing_Custom.UseVisualStyleBackColor = true;
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.BackColor = System.Drawing.Color.Transparent;
            this.Label16.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Label16.Location = new System.Drawing.Point(3, 3);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(105, 17);
            this.Label16.TabIndex = 29;
            this.Label16.Text = "FuncDrawingType";
            this.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel7
            // 
            this.Panel7.Controls.Add(this.CoorTypeComboBox);
            this.Panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.Panel7.Location = new System.Drawing.Point(217, 0);
            this.Panel7.Name = "Panel7";
            this.Panel7.Size = new System.Drawing.Size(139, 24);
            this.Panel7.TabIndex = 40;
            // 
            // CoorTypeComboBox
            // 
            this.CoorTypeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(118)))), ((int)(((byte)(140)))));
            this.CoorTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CoorTypeComboBox.Font = new System.Drawing.Font("Tahoma", 8F);
            this.CoorTypeComboBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.CoorTypeComboBox.FormattingEnabled = true;
            this.CoorTypeComboBox.Location = new System.Drawing.Point(18, 2);
            this.CoorTypeComboBox.Name = "CoorTypeComboBox";
            this.CoorTypeComboBox.Size = new System.Drawing.Size(105, 21);
            this.CoorTypeComboBox.TabIndex = 26;
            this.CoorTypeComboBox.Text = "أرقام";
            // 
            // coordinatesLabel
            // 
            this.coordinatesLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.coordinatesLabel.ForeColor = System.Drawing.Color.Black;
            this.coordinatesLabel.Location = new System.Drawing.Point(73, 0);
            this.coordinatesLabel.Name = "coordinatesLabel";
            this.coordinatesLabel.Size = new System.Drawing.Size(144, 24);
            this.coordinatesLabel.TabIndex = 38;
            this.coordinatesLabel.Text = "(x,y)";
            this.coordinatesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label11
            // 
            this.Label11.Dock = System.Windows.Forms.DockStyle.Left;
            this.Label11.ForeColor = System.Drawing.Color.Black;
            this.Label11.Location = new System.Drawing.Point(0, 0);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(73, 24);
            this.Label11.TabIndex = 39;
            this.Label11.Text = "Corrdinates:";
            this.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel4
            // 
            this.Panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(175)))));
            this.Panel4.Controls.Add(this.SaveBtn);
            this.Panel4.Controls.Add(this.RecordBtn);
            this.Panel4.Controls.Add(this.Panel6);
            this.Panel4.Controls.Add(this.Panel2);
            this.Panel4.Controls.Add(this.SettingBtn);
            this.Panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel4.Location = new System.Drawing.Point(0, 0);
            this.Panel4.Name = "Panel4";
            this.Panel4.Size = new System.Drawing.Size(930, 41);
            this.Panel4.TabIndex = 44;
            this.Panel4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel4_MouseDown);
            this.Panel4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel4_MouseMove);
            this.Panel4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Panel4_MouseUp);
            // 
            // SaveBtn
            // 
            this.SaveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(175)))));
            this.SaveBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SaveBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.SaveBtn.FlatAppearance.BorderSize = 0;
            this.SaveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveBtn.ForeColor = System.Drawing.Color.White;
            this.SaveBtn.Location = new System.Drawing.Point(637, 0);
            this.SaveBtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(81, 41);
            this.SaveBtn.TabIndex = 44;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = false;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // RecordBtn
            // 
            this.RecordBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(175)))));
            this.RecordBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RecordBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RecordBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.RecordBtn.FlatAppearance.BorderSize = 0;
            this.RecordBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RecordBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordBtn.ForeColor = System.Drawing.Color.White;
            this.RecordBtn.Location = new System.Drawing.Point(718, 0);
            this.RecordBtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RecordBtn.Name = "RecordBtn";
            this.RecordBtn.Size = new System.Drawing.Size(97, 41);
            this.RecordBtn.TabIndex = 43;
            this.RecordBtn.Text = "Record";
            this.RecordBtn.UseVisualStyleBackColor = false;
            // 
            // Panel6
            // 
            this.Panel6.BackColor = System.Drawing.Color.Teal;
            this.Panel6.Controls.Add(this.BunifuFlatButton4);
            this.Panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.Panel6.Location = new System.Drawing.Point(41, 0);
            this.Panel6.Name = "Panel6";
            this.Panel6.Size = new System.Drawing.Size(92, 41);
            this.Panel6.TabIndex = 5;
            // 
            // BunifuFlatButton4
            // 
            this.BunifuFlatButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.BunifuFlatButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BunifuFlatButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BunifuFlatButton4.Dock = System.Windows.Forms.DockStyle.Left;
            this.BunifuFlatButton4.FlatAppearance.BorderSize = 0;
            this.BunifuFlatButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BunifuFlatButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BunifuFlatButton4.ForeColor = System.Drawing.Color.White;
            this.BunifuFlatButton4.Location = new System.Drawing.Point(0, 0);
            this.BunifuFlatButton4.Name = "BunifuFlatButton4";
            this.BunifuFlatButton4.Size = new System.Drawing.Size(92, 41);
            this.BunifuFlatButton4.TabIndex = 0;
            this.BunifuFlatButton4.Text = "Open";
            this.BunifuFlatButton4.UseVisualStyleBackColor = false;
            this.BunifuFlatButton4.Click += new System.EventHandler(this.BunifuFlatButton4_Click);
            // 
            // Panel2
            // 
            this.Panel2.BackColor = System.Drawing.Color.Gray;
            this.Panel2.Controls.Add(this.Max_Normal_Btn);
            this.Panel2.Controls.Add(this.MinimizeBtn);
            this.Panel2.Controls.Add(this.ExitBtn);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.Panel2.Location = new System.Drawing.Point(815, 0);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(115, 41);
            this.Panel2.TabIndex = 4;
            // 
            // Max_Normal_Btn
            // 
            this.Max_Normal_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Max_Normal_Btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(138)))), ((int)(((byte)(114)))));
            this.Max_Normal_Btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Max_Normal_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Max_Normal_Btn.FlatAppearance.BorderColor = System.Drawing.Color.DarkGreen;
            this.Max_Normal_Btn.FlatAppearance.BorderSize = 2;
            this.Max_Normal_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Max_Normal_Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Max_Normal_Btn.ForeColor = System.Drawing.Color.White;
            this.Max_Normal_Btn.Location = new System.Drawing.Point(47, 10);
            this.Max_Normal_Btn.Name = "Max_Normal_Btn";
            this.Max_Normal_Btn.Size = new System.Drawing.Size(20, 20);
            this.Max_Normal_Btn.TabIndex = 1;
            this.Max_Normal_Btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Max_Normal_Btn.UseVisualStyleBackColor = false;
            this.Max_Normal_Btn.Click += new System.EventHandler(this.Max_Normal_Btn_Click);
            // 
            // MinimizeBtn
            // 
            this.MinimizeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinimizeBtn.BackColor = System.Drawing.Color.Gold;
            this.MinimizeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MinimizeBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MinimizeBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.MinimizeBtn.FlatAppearance.BorderSize = 2;
            this.MinimizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MinimizeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimizeBtn.ForeColor = System.Drawing.Color.White;
            this.MinimizeBtn.Location = new System.Drawing.Point(19, 10);
            this.MinimizeBtn.Name = "MinimizeBtn";
            this.MinimizeBtn.Size = new System.Drawing.Size(20, 20);
            this.MinimizeBtn.TabIndex = 2;
            this.MinimizeBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MinimizeBtn.UseVisualStyleBackColor = false;
            this.MinimizeBtn.Click += new System.EventHandler(this.MinimizeBtn_Click);
            // 
            // ExitBtn
            // 
            this.ExitBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(61)))), ((int)(((byte)(93)))));
            this.ExitBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ExitBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExitBtn.FlatAppearance.BorderColor = System.Drawing.Color.Maroon;
            this.ExitBtn.FlatAppearance.BorderSize = 2;
            this.ExitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitBtn.ForeColor = System.Drawing.Color.White;
            this.ExitBtn.Location = new System.Drawing.Point(75, 10);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(20, 20);
            this.ExitBtn.TabIndex = 0;
            this.ExitBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ExitBtn.UseVisualStyleBackColor = false;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // SettingBtn
            // 
            this.SettingBtn.BackColor = System.Drawing.Color.SlateGray;
            this.SettingBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SettingBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingBtn.Dock = System.Windows.Forms.DockStyle.Left;
            this.SettingBtn.FlatAppearance.BorderSize = 0;
            this.SettingBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SettingBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SettingBtn.ForeColor = System.Drawing.Color.White;
            this.SettingBtn.Location = new System.Drawing.Point(0, 0);
            this.SettingBtn.Name = "SettingBtn";
            this.SettingBtn.Size = new System.Drawing.Size(41, 41);
            this.SettingBtn.TabIndex = 45;
            this.SettingBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SettingBtn.UseVisualStyleBackColor = false;
            // 
            // TimerTabsMoveLeft
            // 
            this.TimerTabsMoveLeft.Interval = 10;
            this.TimerTabsMoveLeft.Tick += new System.EventHandler(this.TimerTabsMoveLeft_Tick);
            // 
            // TimerTabsMoveRight
            // 
            this.TimerTabsMoveRight.Interval = 10;
            this.TimerTabsMoveRight.Tick += new System.EventHandler(this.TimerTabsMoveRight_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 791);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.Panel3);
            this.Controls.Add(this.Panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "GraphMaker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Graph_Load);
            this.MainPanel.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.Panel_.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.AddPanel.ResumeLayout(false);
            this.Panel3.ResumeLayout(false);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.Panel7.ResumeLayout(false);
            this.Panel4.ResumeLayout(false);
            this.Panel6.ResumeLayout(false);
            this.Panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel MainPanel;
        internal System.Windows.Forms.Panel Panel3;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.RadioButton FuncDrawing_Polar;
        internal System.Windows.Forms.RadioButton FuncDrawing_Oscillate;
        internal System.Windows.Forms.RadioButton FuncDrawing_Custom;
        internal System.Windows.Forms.Label Label16;
        internal System.Windows.Forms.Panel Panel7;
        internal System.Windows.Forms.ComboBox CoorTypeComboBox;
        internal System.Windows.Forms.Label coordinatesLabel;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.Panel Panel4;
        internal System.Windows.Forms.Button RecordBtn;
        internal System.Windows.Forms.Panel Panel6;
        internal System.Windows.Forms.Button BunifuFlatButton4;
        internal System.Windows.Forms.Panel Panel2;
        internal System.Windows.Forms.Button Max_Normal_Btn;
        internal System.Windows.Forms.Button MinimizeBtn;
        internal System.Windows.Forms.Button ExitBtn;
        internal System.Windows.Forms.Panel TopPanel;
        internal System.Windows.Forms.Panel Panel_;
        internal System.Windows.Forms.Panel HeaderPanel;
        internal System.Windows.Forms.Panel AddPanel;
        internal System.Windows.Forms.Button AddBtn;
        internal System.Windows.Forms.Button RightBtn;
        internal System.Windows.Forms.Button LeftBtn;
        internal System.Windows.Forms.Timer TimerTabsMoveLeft;
        internal System.Windows.Forms.Timer TimerTabsMoveRight;
        private System.Windows.Forms.Panel SketchPanel;
        internal System.Windows.Forms.Button SaveBtn;
        internal System.Windows.Forms.Button SettingBtn;
    }
}