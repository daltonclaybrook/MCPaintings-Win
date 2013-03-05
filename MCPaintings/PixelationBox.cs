using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MCPaintings
{
    class PixelationBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            pe.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            Rectangle newRect = new Rectangle();
            Size viewSize = this.Size;
            float sizeMod = 1.0F;
            if ((float)this.Image.Size.Width / (float)this.Image.Size.Height >= (float)viewSize.Width / (float)viewSize.Height)
            {
                //Width is greater than ratio
                newRect.Width = viewSize.Width;
                sizeMod = (float)this.Image.Size.Width / 64.0F;
                newRect.Height = this.Image.Size.Height * viewSize.Width / this.Image.Size.Width;
            }
            else
            {
                newRect.Width = this.Image.Size.Width * viewSize.Height / this.Image.Size.Height;
                newRect.Height = viewSize.Height;
                sizeMod = (float)this.Image.Size.Height / 64.0F;
            }

            newRect.Width = (int)(newRect.Width * sizeMod);
            newRect.Height = (int)(newRect.Height * sizeMod);
            newRect.X = (int)((float)(viewSize.Width - newRect.Width) / 2.0F);
            newRect.Y = (int)((float)(viewSize.Height - newRect.Height) / 2.0F);

            pe.Graphics.DrawImage(this.Image, newRect);
            
            //base.OnPaint(pe);
        }
    }
}
