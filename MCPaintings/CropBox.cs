using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MCPaintings
{
    class CropBox : PictureBox
    {
        enum AspectRatio {
            Unset,
            OneToOne,
            OneToTwo,
            TwoToOne,
            FourToThree
        };

        enum Corner
        {
            UpperLeft,
            UpperRight,
            LowerLeft,
            LowerRight
        }

        private int padding = 20;
        private int startingRectSize = 200;
        private int minRectSize = 40;
        private int cornerEllipseDiameter = 10;
        private Rectangle cropRect = Rectangle.Empty;
        private Rectangle imageRect = Rectangle.Empty;
        private AspectRatio aspectRatio = AspectRatio.Unset;
        private bool resizing = false;
        private bool moving = false;
        private Corner fixedCorner;
        private Point startRectOffset = Point.Empty;

        public CropBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.MouseDown += new MouseEventHandler(CropMouseDown);
            this.MouseMove += new MouseEventHandler(CropMouseMove);
            this.MouseUp += new MouseEventHandler(CropMouseUp);
        }

        public Rectangle AdjustedCropRect() {
            Rectangle adjusted = new Rectangle(cropRect.X - imageRect.X, cropRect.Y - imageRect.Y, cropRect.Width, cropRect.Height);
            float modifier = (float)this.Image.Size.Width / (float)imageRect.Width;

            adjusted.X = (int)((float)adjusted.X * modifier);
            adjusted.Y = (int)((float)adjusted.Y * modifier);
            adjusted.Width = (int)((float)adjusted.Width * modifier);
            adjusted.Height = (int)((float)adjusted.Height * modifier);
            return adjusted;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (this.Image != null)
            {
                DrawImage(this.Image, pe);
            }

            if (cropRect != Rectangle.Empty)
            {
                DrawCropRect(cropRect, pe);
            }
        }

        public void SetupCropRectFromRect(Rectangle rect)
        {
            if (this.Image == null) return;
            if (imageRect == Rectangle.Empty) imageRect = SetupImageRect(this.Image);

            aspectRatio = AspectRatioFromRect(rect);
            Size newSize = new Size(0, 0);
            switch (aspectRatio)
            {
                case AspectRatio.OneToOne:
                    {
                        Debug.WriteLine("one to one");
                        newSize.Width = startingRectSize;
                        newSize.Height = startingRectSize;
                        Debug.WriteLine("size: " + newSize.ToString());
                        break;
                    }
                case AspectRatio.OneToTwo:
                    {
                        newSize.Width = startingRectSize / 2;
                        newSize.Height = startingRectSize;
                        break;
                    }
                case AspectRatio.TwoToOne:
                    {
                        newSize.Width = startingRectSize;
                        newSize.Height = startingRectSize / 2;
                        break;
                    }
                case AspectRatio.FourToThree:
                    {
                        newSize.Width = startingRectSize;
                        newSize.Height = startingRectSize / 4 * 3;
                        break;
                    }
                default:
                    {
                        return;
                    }
            }

            if (imageRect.Size.Width < newSize.Width)
            {
                newSize.Width = imageRect.Size.Width;
                newSize.Height = (int)((float)imageRect.Size.Width / (float)newSize.Width * (float)newSize.Height);
            }
            else if (imageRect.Size.Height < newSize.Height)
            {
                newSize.Width = (int)((float)imageRect.Size.Height / (float)newSize.Height * (float)newSize.Width);
                newSize.Height = imageRect.Size.Height;
            }
            
            cropRect = new Rectangle(0, 0, newSize.Width, newSize.Height);
            cropRect.X = imageRect.Location.X + (imageRect.Width - cropRect.Size.Width) / 2;
            cropRect.Y = imageRect.Location.Y + (imageRect.Height - cropRect.Size.Height) / 2;
        }

        #region Private Methods

        private Rectangle SetupImageRect(Image theImage)
        {
            Rectangle newRect = new Rectangle();
            Size viewSize = new Size(this.Size.Width - padding * 2, this.Size.Height - padding * 2);
            if ((float)theImage.Size.Width / (float)theImage.Size.Height >= (float)viewSize.Width / (float)viewSize.Height)
            {
                //Width is greater than ratio
                newRect.Width = viewSize.Width;
                newRect.Height = theImage.Size.Height * viewSize.Width / theImage.Size.Width;
            }
            else
            {
                newRect.Width = theImage.Size.Width * viewSize.Height / theImage.Size.Height;
                newRect.Height = viewSize.Height;
            }

            newRect.X = (int)((float)(viewSize.Width - newRect.Width) / 2.0F + padding);
            newRect.Y = (int)((float)(viewSize.Height - newRect.Height) / 2.0F + padding);
            return newRect;
        }

        private Rectangle CornerRectFromRect(Corner corner, Rectangle rect)
        {
            switch (corner)
            {
                case Corner.UpperLeft:
                    {
                        return new Rectangle(rect.X - cornerEllipseDiameter / 2, rect.Y - cornerEllipseDiameter / 2, cornerEllipseDiameter, cornerEllipseDiameter);
                    }
                case Corner.UpperRight:
                    {
                        return new Rectangle(rect.X + rect.Width - cornerEllipseDiameter / 2, rect.Y - cornerEllipseDiameter / 2, cornerEllipseDiameter, cornerEllipseDiameter);
                    }
                case Corner.LowerLeft:
                    {
                        return new Rectangle(rect.X - cornerEllipseDiameter / 2, rect.Y + rect.Height - cornerEllipseDiameter / 2, cornerEllipseDiameter, cornerEllipseDiameter);
                    }
                case Corner.LowerRight:
                    {
                        return new Rectangle(rect.X + rect.Width - cornerEllipseDiameter / 2, rect.Y + rect.Height - cornerEllipseDiameter / 2, cornerEllipseDiameter, cornerEllipseDiameter);
                    }
                default:
                    {
                        return Rectangle.Empty;
                    }
            }
        }

        private Size FixedSizeFromDynamic(Size dynamicSize)
        {
            switch (aspectRatio)
            {
                case AspectRatio.OneToOne:
                    {
                        int smallest = (dynamicSize.Width <= dynamicSize.Height) ? dynamicSize.Width : dynamicSize.Height;
                        return new Size(smallest, smallest);
                    }
                case AspectRatio.OneToTwo:
                    {
                        if ((float)dynamicSize.Width / (float)dynamicSize.Height >= 1.0F / 2.0F)
                        {
                            return new Size(dynamicSize.Height / 2, dynamicSize.Height);
                        }
                        else
                        {
                            return new Size(dynamicSize.Width, dynamicSize.Width * 2);
                        }
                    }
                case AspectRatio.TwoToOne:
                    {
                        if ((float)dynamicSize.Width / (float)dynamicSize.Height >= 2.0F / 1.0F)
                        {
                            return new Size(dynamicSize.Height * 2, dynamicSize.Height);
                        }
                        else
                        {
                            return new Size(dynamicSize.Width, dynamicSize.Width / 2);
                        }
                    }
                case AspectRatio.FourToThree:
                    {
                        if ((float)dynamicSize.Width / (float)dynamicSize.Height >= 4.0F / 3.0F)
                        {
                            return new Size((int)((float)dynamicSize.Height / 3.0F * 4.0F), dynamicSize.Height);
                        }
                        else
                        {
                            return new Size(dynamicSize.Width, (int)((float)dynamicSize.Width / 4.0F * 3.0F));
                        }
                    }
                default:
                    {
                        return Size.Empty;
                    }
            }
        }

        private void ResizeCropRect(Point mouseLocation)
        {
            if (mouseLocation.X < imageRect.X) mouseLocation.X = imageRect.X;
            if (mouseLocation.Y < imageRect.Y) mouseLocation.Y = imageRect.Y;
            if (mouseLocation.X > imageRect.X + imageRect.Width) mouseLocation.X = imageRect.X + imageRect.Width;
            if (mouseLocation.Y > imageRect.Y + imageRect.Height) mouseLocation.Y = imageRect.Y + imageRect.Height;

            switch (fixedCorner)
            {
                case Corner.UpperLeft:
                    {
                        Size dynamicSize = new Size(mouseLocation.X - cropRect.X, mouseLocation.Y - cropRect.Y);
                        Size fixedSize = FixedSizeFromDynamic(dynamicSize);
                        if (((fixedSize.Width <= fixedSize.Height) && (fixedSize.Width < minRectSize)) || ((fixedSize.Height <= fixedSize.Width) && (fixedSize.Height < minRectSize))) return;
                        cropRect = new Rectangle(cropRect.X, cropRect.Y, fixedSize.Width, fixedSize.Height);
                        break;
                    }
                case Corner.UpperRight:
                    {
                        Point fixedPoint = new Point(cropRect.X + cropRect.Width, cropRect.Y);
                        Size dynamicSize = new Size(fixedPoint.X - mouseLocation.X, mouseLocation.Y - fixedPoint.Y);
                        Size fixedSize = FixedSizeFromDynamic(dynamicSize);
                        if (((fixedSize.Width <= fixedSize.Height) && (fixedSize.Width < minRectSize)) || ((fixedSize.Height <= fixedSize.Width) && (fixedSize.Height < minRectSize))) return;
                        cropRect = new Rectangle(fixedPoint.X - fixedSize.Width, fixedPoint.Y, fixedSize.Width, fixedSize.Height);
                        break;
                    }
                case Corner.LowerLeft:
                    {
                        Point fixedPoint = new Point(cropRect.X, cropRect.Y + cropRect.Height);
                        Size dynamicSize = new Size(mouseLocation.X - fixedPoint.X, fixedPoint.Y - mouseLocation.Y);
                        Size fixedSize = FixedSizeFromDynamic(dynamicSize);
                        if (((fixedSize.Width <= fixedSize.Height) && (fixedSize.Width < minRectSize)) || ((fixedSize.Height <= fixedSize.Width) && (fixedSize.Height < minRectSize))) return;
                        cropRect = new Rectangle(fixedPoint.X, fixedPoint.Y - fixedSize.Height, fixedSize.Width, fixedSize.Height);
                        break;
                    }
                case Corner.LowerRight:
                    {
                        Point fixedPoint = new Point(cropRect.X + cropRect.Width, cropRect.Y + cropRect.Height);
                        Size dynamicSize = new Size(fixedPoint.X - mouseLocation.X, fixedPoint.Y - mouseLocation.Y);
                        Size fixedSize = FixedSizeFromDynamic(dynamicSize);
                        if (((fixedSize.Width <= fixedSize.Height) && (fixedSize.Width < minRectSize)) || ((fixedSize.Height <= fixedSize.Width) && (fixedSize.Height < minRectSize))) return;
                        cropRect = new Rectangle(fixedPoint.X - fixedSize.Width, fixedPoint.Y - fixedSize.Height, fixedSize.Width, fixedSize.Height);
                        break;
                    }
            }
        }

        private void MoveCropRect(Point mouseLocation)
        {
            Rectangle newRect = new Rectangle(mouseLocation.X + startRectOffset.X, mouseLocation.Y + startRectOffset.Y, cropRect.Width, cropRect.Height);
            if (newRect.X < imageRect.X) newRect.X = imageRect.X;
            if (newRect.Y < imageRect.Y) newRect.Y = imageRect.Y;
            if (newRect.X > imageRect.X + imageRect.Width - cropRect.Width) newRect.X = imageRect.X + imageRect.Width - cropRect.Width;
            if (newRect.Y > imageRect.Y + imageRect.Height - cropRect.Height) newRect.Y = imageRect.Y + imageRect.Height - cropRect.Height;

            cropRect = newRect;
        }

        private void DrawImage(Image theImage, PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            pe.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            if (imageRect == Rectangle.Empty) imageRect = SetupImageRect(theImage);

            /*Rectangle sourceRect = new Rectangle(pe.ClipRectangle.X - imageRect.X, pe.ClipRectangle.Y - imageRect.Y, pe.ClipRectangle.Width, pe.ClipRectangle.Height);
            float multiplier = (float)this.Image.Size.Width / (float)imageRect.Width;
            sourceRect.X = (int)((float)sourceRect.X * multiplier);
            sourceRect.Y = (int)((float)sourceRect.Y * multiplier);
            sourceRect.Width = (int)((float)sourceRect.Width * multiplier);
            sourceRect.Height = (int)((float)sourceRect.Height * multiplier);

            pe.Graphics.DrawImage(theImage, pe.ClipRectangle, sourceRect, GraphicsUnit.Pixel);*/

            pe.Graphics.DrawImage(theImage, imageRect);
        }

        private void DrawCropRect(Rectangle rect, PaintEventArgs pe)
        {
            Pen blackDashPen = new Pen(Color.Black, 1.0F);
            float[] dashValues = { 5, 5 };
            blackDashPen.DashPattern = dashValues;
            pe.Graphics.DrawRectangle(blackDashPen, rect);

            Pen whiteDashPen = new Pen(Color.White, 1.0F);
            whiteDashPen.DashPattern = dashValues;
            whiteDashPen.DashOffset = 5;
            pe.Graphics.DrawRectangle(whiteDashPen, rect);

            SolidBrush whiteBrush = new SolidBrush(Color.White);
            Pen blackSolidPen = new Pen(Color.Black, 1.0F);

            pe.Graphics.FillEllipse(whiteBrush, CornerRectFromRect(Corner.UpperLeft, rect));
            pe.Graphics.FillEllipse(whiteBrush, CornerRectFromRect(Corner.UpperRight, rect));
            pe.Graphics.FillEllipse(whiteBrush, CornerRectFromRect(Corner.LowerLeft, rect));
            pe.Graphics.FillEllipse(whiteBrush, CornerRectFromRect(Corner.LowerRight, rect));

            pe.Graphics.DrawEllipse(blackSolidPen, CornerRectFromRect(Corner.UpperLeft, rect));
            pe.Graphics.DrawEllipse(blackSolidPen, CornerRectFromRect(Corner.UpperRight, rect));
            pe.Graphics.DrawEllipse(blackSolidPen, CornerRectFromRect(Corner.LowerLeft, rect));
            pe.Graphics.DrawEllipse(blackSolidPen, CornerRectFromRect(Corner.LowerRight, rect));
        }

        private AspectRatio AspectRatioFromRect(Rectangle rect)
        {
            if ((float)rect.Width / (float)rect.Height == 1.0F)
            {
                return AspectRatio.OneToOne;
            }
            else if ((float)rect.Width / (float)rect.Height == 1.0F/2.0F)
            {
                return AspectRatio.OneToTwo;
            }
            else if ((float)rect.Width / (float)rect.Height == 2.0F/1.0F)
            {
                return AspectRatio.TwoToOne;
            }
            else if ((float)rect.Width / (float)rect.Height == 4.0F/3.0F)
            {
                return AspectRatio.FourToThree;
            }
            return AspectRatio.Unset;
        }

        #endregion

        #region Mouse Events

        private void CropMouseDown(object sender, MouseEventArgs e)
        {
            if (CornerRectFromRect(Corner.UpperLeft, cropRect).Contains(e.Location) == true)
            {
                //Resizing From Upper Left
                resizing = true;
                fixedCorner = Corner.LowerRight;
            }
            else if (CornerRectFromRect(Corner.UpperRight, cropRect).Contains(e.Location) == true)
            {
                //Resizing From Upper Right
                resizing = true;
                fixedCorner = Corner.LowerLeft;
            }
            else if (CornerRectFromRect(Corner.LowerLeft, cropRect).Contains(e.Location) == true)
            {
                //Resizing From Lower Left
                resizing = true;
                fixedCorner = Corner.UpperRight;
            }
            else if (CornerRectFromRect(Corner.LowerRight, cropRect).Contains(e.Location) == true)
            {
                //Resizing From Lower Right
                resizing = true;
                fixedCorner = Corner.UpperLeft;
            }
            else if (cropRect.Contains(e.Location) == true)
            {
                //Moving Entire Box
                moving = true;
                startRectOffset = new Point(cropRect.X - e.Location.X, cropRect.Y - e.Location.Y);
            }
        }

        private void CropMouseMove(object sender, MouseEventArgs e)
        {
            if (resizing == true)
            {
                ResizeCropRect(e.Location);
                //this.Invalidate(new Rectangle(cropRect.X - cornerEllipseDiameter, cropRect.Y - cornerEllipseDiameter / 2, cropRect.Width + cornerEllipseDiameter, cropRect.Height + cornerEllipseDiameter * 2));
                this.Invalidate();
            }
            else if (moving == true)
            {
                MoveCropRect(e.Location);
                //this.Invalidate(new Rectangle(cropRect.X - cornerEllipseDiameter, cropRect.Y - cornerEllipseDiameter / 2, cropRect.Width + cornerEllipseDiameter, cropRect.Height + cornerEllipseDiameter * 2));
                this.Invalidate();
            }
        }

        private void CropMouseUp(object sender, MouseEventArgs e)
        {
            if (resizing == true) resizing = false;
            if (moving == true) moving = false;
        }

        #endregion
    }
}
