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
    public partial class SelectPaintingForm : Form
    {
        public PaintingsController paintingsController = null;
        public SelectedPaintingDelegate selectedPaintingCallback;
        private PixelationBox pixelBox = null;
        private Painting[] paintings;
        private int currentIndex = 0;

        public SelectPaintingForm(PaintingsController pc)
        {
            InitializeComponent();
            pixelBox = new PixelationBox();
            pixelBox.Location = new Point(12, 12);
            pixelBox.Size = new Size(380, 320);
            pixelBox.BorderStyle = BorderStyle.None;
            //pixelBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(pixelBox);

            paintingsController = pc;
            if (paintingsController.sourceImage != null)
            {
                paintings = paintingsController.PaintingsArrayFromSource();
                pixelBox.Image = paintings[currentIndex].image;
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            currentIndex++;
            if (currentIndex >= paintings.Length)
            {
                currentIndex = 0;
            }
            pixelBox.Image = paintings[currentIndex].image;
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = paintings.Length-1;
            }
            pixelBox.Image = paintings[currentIndex].image;
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            selectedPaintingCallback(paintings[currentIndex]);
        }
    }
}
