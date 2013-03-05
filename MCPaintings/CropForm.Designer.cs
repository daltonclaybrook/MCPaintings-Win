namespace MCPaintings
{
    partial class CropForm
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
            this.cropButton = new System.Windows.Forms.Button();
            this.preserveFrameBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cropButton
            // 
            this.cropButton.Location = new System.Drawing.Point(288, 466);
            this.cropButton.Name = "cropButton";
            this.cropButton.Size = new System.Drawing.Size(180, 43);
            this.cropButton.TabIndex = 0;
            this.cropButton.Text = "Crop Image and Save";
            this.cropButton.UseVisualStyleBackColor = true;
            this.cropButton.Click += new System.EventHandler(this.cropButton_Click);
            // 
            // preserveFrameBox
            // 
            this.preserveFrameBox.AutoSize = true;
            this.preserveFrameBox.Checked = true;
            this.preserveFrameBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.preserveFrameBox.Location = new System.Drawing.Point(474, 480);
            this.preserveFrameBox.Name = "preserveFrameBox";
            this.preserveFrameBox.Size = new System.Drawing.Size(181, 17);
            this.preserveFrameBox.TabIndex = 1;
            this.preserveFrameBox.Text = "Preserve Original Painting Border";
            this.preserveFrameBox.UseVisualStyleBackColor = true;
            // 
            // CropForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 521);
            this.Controls.Add(this.preserveFrameBox);
            this.Controls.Add(this.cropButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(772, 559);
            this.MinimumSize = new System.Drawing.Size(772, 559);
            this.Name = "CropForm";
            this.Text = "Crop Image";
            this.Shown += new System.EventHandler(this.CropForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cropButton;
        private System.Windows.Forms.CheckBox preserveFrameBox;
    }
}