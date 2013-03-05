using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MCPaintings
{
    public class Painting
    {
        //Rectangle is measured in 16x16 blocks

        public Image image;
        public Rectangle rect;

        public Painting(Image source, Rectangle coords)
        {
            rect = coords;
            Bitmap src = source as Bitmap;
            Bitmap target = new Bitmap(coords.Width * 16, coords.Height * 16);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                    new Rectangle(coords.Location.X * 16, coords.Location.Y * 16, target.Width, target.Height),
                    GraphicsUnit.Pixel);
                image = target as Image;
            }
        }
    }
}
