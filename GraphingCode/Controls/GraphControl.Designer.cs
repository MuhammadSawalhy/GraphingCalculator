namespace Graphing.Controls
{
    partial class GraphControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphControl));
            this.VisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Discription = new System.Windows.Forms.TextBox();
            this.Script = new System.Windows.Forms.TextBox();
            this.EditBtn = new System.Windows.Forms.Button();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.ErrorBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.NameLab = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // VisibleCheckBox
            // 
            this.VisibleCheckBox.AutoSize = true;
            this.VisibleCheckBox.Checked = true;
            this.VisibleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VisibleCheckBox.Enabled = false;
            this.VisibleCheckBox.Location = new System.Drawing.Point(9, 10);
            this.VisibleCheckBox.Name = "VisibleCheckBox";
            this.VisibleCheckBox.Size = new System.Drawing.Size(15, 14);
            this.VisibleCheckBox.TabIndex = 6;
            this.VisibleCheckBox.UseVisualStyleBackColor = true;
            this.VisibleCheckBox.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // Label4
            // 
            this.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label4.AutoSize = true;
            this.Label4.ForeColor = System.Drawing.Color.Gainsboro;
            this.Label4.Location = new System.Drawing.Point(5, 84);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(56, 13);
            this.Label4.TabIndex = 10;
            this.Label4.Text = "Discription";
            // 
            // Discription
            // 
            this.Discription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Discription.Location = new System.Drawing.Point(64, 81);
            this.Discription.Name = "Discription";
            this.Discription.ReadOnly = true;
            this.Discription.Size = new System.Drawing.Size(178, 20);
            this.Discription.TabIndex = 7;
            // 
            // Script
            // 
            this.Script.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Script.Font = new System.Drawing.Font("MV Boli", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Script.Location = new System.Drawing.Point(64, 42);
            this.Script.Name = "Script";
            this.Script.Size = new System.Drawing.Size(152, 35);
            this.Script.TabIndex = 8;
            // 
            // EditBtn
            // 
            this.EditBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EditBtn.BackColor = System.Drawing.Color.Transparent;
            this.EditBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EditBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EditBtn.Enabled = false;
            this.EditBtn.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.EditBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditBtn.ForeColor = System.Drawing.Color.White;
            this.EditBtn.Image = ((System.Drawing.Image)(resources.GetObject("EditBtn.Image")));
            this.EditBtn.Location = new System.Drawing.Point(186, 2);
            this.EditBtn.Name = "EditBtn";
            this.EditBtn.Size = new System.Drawing.Size(30, 30);
            this.EditBtn.TabIndex = 12;
            this.EditBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.EditBtn.UseVisualStyleBackColor = false;
            this.EditBtn.Click += new System.EventHandler(this.EditBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseBtn.BackColor = System.Drawing.Color.Transparent;
            this.CloseBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CloseBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseBtn.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.CloseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseBtn.ForeColor = System.Drawing.Color.White;
            this.CloseBtn.Image = ((System.Drawing.Image)(resources.GetObject("CloseBtn.Image")));
            this.CloseBtn.Location = new System.Drawing.Point(218, 2);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(30, 30);
            this.CloseBtn.TabIndex = 13;
            this.CloseBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CloseBtn.UseVisualStyleBackColor = false;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // Label3
            // 
            this.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label3.AutoSize = true;
            this.Label3.ForeColor = System.Drawing.Color.Gainsboro;
            this.Label3.Location = new System.Drawing.Point(16, 53);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(34, 13);
            this.Label3.TabIndex = 11;
            this.Label3.Text = "Script";
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(78)))), ((int)(((byte)(87)))));
            this.ApplyBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ApplyBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ApplyBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(78)))), ((int)(((byte)(87)))));
            this.ApplyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApplyBtn.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyBtn.ForeColor = System.Drawing.Color.White;
            this.ApplyBtn.Location = new System.Drawing.Point(217, 42);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(25, 35);
            this.ApplyBtn.TabIndex = 12;
            this.ApplyBtn.Text = "";
            this.ApplyBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ApplyBtn.UseVisualStyleBackColor = false;
            this.ApplyBtn.Click += new System.EventHandler(this.Apply_Click);
            // 
            // ErrorBtn
            // 
            this.ErrorBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorBtn.BackColor = System.Drawing.Color.Orange;
            this.ErrorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ErrorBtn.Image = ((System.Drawing.Image)(resources.GetObject("ErrorBtn.Image")));
            this.ErrorBtn.Location = new System.Drawing.Point(150, 2);
            this.ErrorBtn.Name = "ErrorBtn";
            this.ErrorBtn.Size = new System.Drawing.Size(30, 30);
            this.ErrorBtn.TabIndex = 14;
            this.ErrorBtn.TabStop = false;
            this.ErrorBtn.UseVisualStyleBackColor = false;
            this.ErrorBtn.Visible = false;
            this.ErrorBtn.Click += new System.EventHandler(this.ErrorBtn_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.NameLab);
            this.panel1.Controls.Add(this.CloseBtn);
            this.panel1.Controls.Add(this.ErrorBtn);
            this.panel1.Controls.Add(this.VisibleCheckBox);
            this.panel1.Controls.Add(this.EditBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 34);
            this.panel1.TabIndex = 15;
            // 
            // NameLab
            // 
            this.NameLab.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.NameLab.AutoSize = true;
            this.NameLab.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.NameLab.Location = new System.Drawing.Point(30, 11);
            this.NameLab.Name = "NameLab";
            this.NameLab.Size = new System.Drawing.Size(0, 13);
            this.NameLab.TabIndex = 15;
            // 
            // GraphControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(78)))), ((int)(((byte)(87)))));
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Discription);
            this.Controls.Add(this.Script);
            this.Name = "GraphControl";
            this.Size = new System.Drawing.Size(250, 110);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox VisibleCheckBox;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox Discription;
        internal System.Windows.Forms.Button EditBtn;
        internal System.Windows.Forms.Button CloseBtn;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.Button ErrorBtn;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TextBox Script;
        internal System.Windows.Forms.Label NameLab;
    }
}
