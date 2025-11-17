namespace Graphing
{
    partial class Tab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.HeaderBtn = new System.Windows.Forms.Button();
            this.BunifuFlatButton1 = new System.Windows.Forms.Button();
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ChangeNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel1.SuspendLayout();
            this.ContextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.HeaderBtn);
            this.Panel1.Controls.Add(this.BunifuFlatButton1);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(196, 55);
            this.Panel1.TabIndex = 5;
            // 
            // HeaderBtn
            // 
            this.HeaderBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(129)))), ((int)(((byte)(138)))));
            this.HeaderBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.HeaderBtn.ContextMenuStrip = this.ContextMenuStrip1;
            this.HeaderBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HeaderBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderBtn.FlatAppearance.BorderSize = 0;
            this.HeaderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HeaderBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.HeaderBtn.Location = new System.Drawing.Point(0, 0);
            this.HeaderBtn.Name = "HeaderBtn";
            this.HeaderBtn.Size = new System.Drawing.Size(173, 55);
            this.HeaderBtn.TabIndex = 4;
            this.HeaderBtn.Text = "BunifuFlatButton1";
            this.HeaderBtn.UseVisualStyleBackColor = false;
            this.HeaderBtn.Click += new System.EventHandler(this.HeaderBtn_Click);
            // 
            // BunifuFlatButton1
            // 
            this.BunifuFlatButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.BunifuFlatButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BunifuFlatButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BunifuFlatButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.BunifuFlatButton1.FlatAppearance.BorderSize = 0;
            this.BunifuFlatButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BunifuFlatButton1.Font = new System.Drawing.Font("Segoe MDL2 Assets", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BunifuFlatButton1.ForeColor = System.Drawing.Color.White;
            this.BunifuFlatButton1.Location = new System.Drawing.Point(173, 0);
            this.BunifuFlatButton1.Name = "BunifuFlatButton1";
            this.BunifuFlatButton1.Size = new System.Drawing.Size(23, 55);
            this.BunifuFlatButton1.TabIndex = 5;
            this.BunifuFlatButton1.Text = "";
            this.BunifuFlatButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BunifuFlatButton1.UseVisualStyleBackColor = false;
            this.BunifuFlatButton1.Click += new System.EventHandler(this.Delete);
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangeNameToolStripMenuItem});
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(151, 26);
            // 
            // ChangeNameToolStripMenuItem
            // 
            this.ChangeNameToolStripMenuItem.Name = "ChangeNameToolStripMenuItem";
            this.ChangeNameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ChangeNameToolStripMenuItem.Text = "Change Name";
            this.ChangeNameToolStripMenuItem.Click += new System.EventHandler(this.ChangeNameToolStripMenuItem_Click);
            // 
            // Tab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.Controls.Add(this.Panel1);
            this.Name = "Tab";
            this.Size = new System.Drawing.Size(196, 55);
            this.Panel1.ResumeLayout(false);
            this.ContextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Button HeaderBtn;
        internal System.Windows.Forms.Button BunifuFlatButton1;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem ChangeNameToolStripMenuItem;
    }
}
