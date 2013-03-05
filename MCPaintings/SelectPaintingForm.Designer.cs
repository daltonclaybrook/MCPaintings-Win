namespace MCPaintings
{
    partial class SelectPaintingForm
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
            this.selectButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.previousButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(154, 338);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(96, 46);
            this.selectButton.TabIndex = 1;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(256, 338);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(46, 46);
            this.nextButton.TabIndex = 2;
            this.nextButton.Text = "->";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // previousButton
            // 
            this.previousButton.Location = new System.Drawing.Point(102, 338);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(46, 46);
            this.previousButton.TabIndex = 3;
            this.previousButton.Text = "<-";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // SelectPaintingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 392);
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.selectButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(420, 430);
            this.MinimumSize = new System.Drawing.Size(420, 430);
            this.Name = "SelectPaintingForm";
            this.Text = "Select a Painting to Replace";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button previousButton;
    }
}