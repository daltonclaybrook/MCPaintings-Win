using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCPaintings
{
    public partial class CropForm : Form
    {
        private CropBox cropBox = null;
        private Image newImage = null;
        public Rectangle sourceRect = Rectangle.Empty;
        public CropRectForNewImageDelegate cropCallback;

        public CropForm()
        {
            InitializeComponent();
            cropBox = new CropBox();
            cropBox.Location = new Point(0, 0);
            cropBox.Size = new Size(772, 466);
            cropBox.BorderStyle = BorderStyle.None;
            this.Controls.Add(cropBox);
            this.Controls.SetChildIndex(cropBox, 0);
        }

        private void CropForm_Shown(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Select a New Image";
            openFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                newImage = Image.FromFile(openFile.FileName);
                cropBox.Image = newImage;
                if (sourceRect != Rectangle.Empty) cropBox.SetupCropRectFromRect(sourceRect);
                cropBox.Invalidate();
            }
        }

        private void cropButton_Click(object sender, EventArgs e)
        {
            Rectangle adjustedRect = cropBox.AdjustedCropRect();
            cropCallback(cropBox.Image, adjustedRect, preserveFrameBox.Checked);
        }
    }
}
