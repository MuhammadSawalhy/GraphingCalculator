namespace Graphing.Controls
{
    partial class Edit_Pen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //public override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Button1 = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.NumericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.Color_btn = new System.Windows.Forms.Button();
            this.Label8 = new System.Windows.Forms.Label();
            this.Style_ = new System.Windows.Forms.ComboBox();
            this.ColorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // Button1
            // 
            this.Button1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Button1.Location = new System.Drawing.Point(14, 169);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(264, 47);
            this.Button1.TabIndex = 34;
            this.Button1.Text = "Okey";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.SystemColors.Desktop;
            this.Label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Label2.Location = new System.Drawing.Point(64, 33);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(95, 23);
            this.Label2.TabIndex = 33;
            this.Label2.Text = "Color";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Tan;
            this.Label1.Location = new System.Drawing.Point(14, 112);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(264, 45);
            this.Label1.TabIndex = 32;
            // 
            // NumericUpDown1
            // 
            this.NumericUpDown1.Location = new System.Drawing.Point(178, 76);
            this.NumericUpDown1.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NumericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDown1.Name = "NumericUpDown1";
            this.NumericUpDown1.Size = new System.Drawing.Size(36, 20);
            this.NumericUpDown1.TabIndex = 31;
            this.NumericUpDown1.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NumericUpDown1.Click += new System.EventHandler(this.NumericUpDown1_ValueChanged);
            // 
            // Color_btn
            // 
            this.Color_btn.BackColor = System.Drawing.Color.Crimson;
            this.Color_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Color_btn.ForeColor = System.Drawing.SystemColors.Control;
            this.Color_btn.Location = new System.Drawing.Point(64, 56);
            this.Color_btn.Name = "Color_btn";
            this.Color_btn.Size = new System.Drawing.Size(95, 22);
            this.Color_btn.TabIndex = 29;
            this.Color_btn.UseVisualStyleBackColor = false;
            this.Color_btn.Click += new System.EventHandler(this.Color_btn_Click);
            // 
            // Label8
            // 
            this.Label8.BackColor = System.Drawing.SystemColors.HotTrack;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Label8.ForeColor = System.Drawing.SystemColors.Control;
            this.Label8.Location = new System.Drawing.Point(166, 33);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(60, 69);
            this.Label8.TabIndex = 30;
            this.Label8.Text = "Width";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Style_
            // 
            this.Style_.BackColor = System.Drawing.Color.PowderBlue;
            this.Style_.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Style_.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Style_.FormattingEnabled = true;
            this.Style_.Items.AddRange(new object[] {
            "ـــــــــــــــــــــــــــــــ",
            "----------",
            "..........",
            "_._._._._.",
            "_.._.._.._"});
            this.Style_.Location = new System.Drawing.Point(64, 82);
            this.Style_.Name = "Style_";
            this.Style_.Size = new System.Drawing.Size(95, 21);
            this.Style_.TabIndex = 28;
            this.Style_.Text = "ـــــــــــــــــــــــــــــــ";
            this.Style_.SelectedIndexChanged += new System.EventHandler(this.Style__SelectedIndexChanged);
            // 
            // Edit_Pen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(292, 249);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.NumericUpDown1);
            this.Controls.Add(this.Color_btn);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.Style_);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Edit_Pen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit_Pen";
            this.Load += new System.EventHandler(this.Edit_pen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.NumericUpDown NumericUpDown1;
        internal System.Windows.Forms.Button Color_btn;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.ComboBox Style_;
        private System.Windows.Forms.ColorDialog ColorDialog1;
    }
}