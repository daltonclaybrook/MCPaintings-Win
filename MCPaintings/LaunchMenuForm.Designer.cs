namespace MCPaintings
{
    partial class LaunchMenuForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.createNewButton = new System.Windows.Forms.Button();
            this.modifyButton = new System.Windows.Forms.Button();
            this.texturePacksView = new System.Windows.Forms.ListView();
            this.texturePacksHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(163, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 50);
            this.label1.TabIndex = 1;
            this.label1.Text = "Create a new texture pack or modify an existing one";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // createNewButton
            // 
            this.createNewButton.Location = new System.Drawing.Point(166, 86);
            this.createNewButton.Name = "createNewButton";
            this.createNewButton.Size = new System.Drawing.Size(146, 42);
            this.createNewButton.TabIndex = 2;
            this.createNewButton.Text = "Create New";
            this.createNewButton.UseVisualStyleBackColor = true;
            this.createNewButton.Click += new System.EventHandler(this.createNewButton_Click);
            // 
            // modifyButton
            // 
            this.modifyButton.Enabled = false;
            this.modifyButton.Location = new System.Drawing.Point(166, 134);
            this.modifyButton.Name = "modifyButton";
            this.modifyButton.Size = new System.Drawing.Size(146, 42);
            this.modifyButton.TabIndex = 3;
            this.modifyButton.Text = "Modify";
            this.modifyButton.UseVisualStyleBackColor = true;
            this.modifyButton.Click += new System.EventHandler(this.modifyButton_Click);
            // 
            // texturePacksView
            // 
            this.texturePacksView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.texturePacksHeader});
            this.texturePacksView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.texturePacksView.Location = new System.Drawing.Point(12, 12);
            this.texturePacksView.MultiSelect = false;
            this.texturePacksView.Name = "texturePacksView";
            this.texturePacksView.Size = new System.Drawing.Size(145, 238);
            this.texturePacksView.TabIndex = 4;
            this.texturePacksView.UseCompatibleStateImageBehavior = false;
            this.texturePacksView.View = System.Windows.Forms.View.Details;
            this.texturePacksView.SelectedIndexChanged += new System.EventHandler(this.texturePacksView_SelectedIndexChanged);
            // 
            // texturePacksHeader
            // 
            this.texturePacksHeader.Text = "Texture Packs";
            this.texturePacksHeader.Width = 144;
            // 
            // LaunchMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 262);
            this.Controls.Add(this.texturePacksView);
            this.Controls.Add(this.modifyButton);
            this.Controls.Add(this.createNewButton);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(340, 300);
            this.MinimumSize = new System.Drawing.Size(340, 300);
            this.Name = "LaunchMenuForm";
            this.Text = "MCPaintings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button createNewButton;
        private System.Windows.Forms.Button modifyButton;
        private System.Windows.Forms.ListView texturePacksView;
        private System.Windows.Forms.ColumnHeader texturePacksHeader;
    }
}

