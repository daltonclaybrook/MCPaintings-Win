using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace MCPaintings
{
    public class PaintingsController
    {
        private string texturePackName;
        private string mcFolder;
        public Image sourceImage;
        public Painting[] paintings;

#region Public Functions

        public PaintingsController()
        {
            mcFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft\";
        }

        public void LoadSourceForNewTexturePack(string name)
        {
            texturePackName = name;
            if (File.Exists(mcFolder + "bin\\minecraft.jar"))
            {
                try
                {
                    LoadSourceFromArchive(mcFolder + "bin\\minecraft.jar");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else if (Directory.Exists(mcFolder + "bin\\minecraft.jar"))
            {
                try
                {
                    sourceImage = Image.FromStream(new MemoryStream(File.ReadAllBytes(mcFolder + "bin\\minecraft.jar\\art\\kz.png")));
                    //sourceImage = Image.FromFile(mcFolder + "bin\\minecraft.jar\\art\\kz.png");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }

            if (sourceImage == null)
            {
                sourceImage = MCPaintings.Properties.Resources.kz;
            }
        }

        public void LoadSourceForTexturePack(string path)
        {
            string texturePackFolder = mcFolder + @"texturepacks\";
            texturePackName = path.Substring(texturePackFolder.Length);
            if (File.Exists(path))
            {
                try
                {
                    LoadSourceFromArchive(path);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else if (Directory.Exists(path))
            {
                try
                {
                    sourceImage = Image.FromStream(new MemoryStream(File.ReadAllBytes(path + @"\art\kz.png")));
                    
                    //This way keeps the file open for some reason,
                    //Throwing an Exception if you attempt to overwrite
                    //sourceImage = Image.FromFile(path + @"\art\kz.png");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }

            if (sourceImage == null)
            {
                Debug.WriteLine("null");
                sourceImage = MCPaintings.Properties.Resources.kz;
            }
            else
            {
                Debug.WriteLine("not null");
            }
        }

        public Painting[] PaintingsArrayFromSource()
        {
            List<Painting> tempPaintings = new List<Painting>();
            int numSections = 7;
            int[] sectionCounts = {
                //Number of images in each section
                7, 5, 2, 1, 6, 2, 3
            };

            Rectangle[] sectionBoxes = {
                //Rect containing each image section in the source image
                //Measured in 16px x 16px blocks
                new Rectangle(0, 0, 12, 2),
                new Rectangle(0, 2, 12, 2),
                new Rectangle(0, 4, 12, 2),
                new Rectangle(0, 6, 12, 2),
                new Rectangle(0, 8, 12, 4),
                new Rectangle(12, 4, 4, 6),
                new Rectangle(0, 12, 16, 4)
            };

            Size[] imageSizes = {
                //Size of the images in each section
                //Measured in 16px x 16px blocks
                new Size(1, 1),
                new Size(2, 1),
                new Size(1, 2),
                new Size(4, 2),
                new Size(2, 2),
                new Size(4, 3),
                new Size(4, 4)
            };

            for (int i=0; i<numSections; i++) {
                for (int j=0; j<sectionCounts[i]; j++) {
                    Painting painting = new Painting(sourceImage, new Rectangle((int)(sectionBoxes[i].Location.X + (j*imageSizes[i].Width)%(sectionBoxes[i].Size.Width)), (int)(/*(sourceImage.Size.Height/16-imageSizes[i].Height) - */(sectionBoxes[i].Location.Y + Math.Floor((double)((j*imageSizes[i].Width)/sectionBoxes[i].Size.Width)) * imageSizes[i].Height)), imageSizes[i].Width, imageSizes[i].Height));
                    tempPaintings.Add(painting);
                }
            }

            paintings = tempPaintings.ToArray();
            return paintings;
        }

        public bool SaveImageOverPainting(Image image, Rectangle rect, Painting painting, bool preserveBorder)
        {
            string texturePackPath = mcFolder + @"texturepacks\" + texturePackName;
            int padding = (preserveBorder == true) ? 1 : 0;
            using (Bitmap bitmap = new Bitmap(sourceImage.Width, sourceImage.Height))
            {
    	        using (Graphics canvas = Graphics.FromImage(bitmap))
    	        {
    		        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
    		        canvas.DrawImage(sourceImage, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height));
    		        canvas.DrawImage(image, new Rectangle(painting.rect.X * 16 + padding, painting.rect.Y * 16 + padding, painting.rect.Width * 16 - padding * 2, painting.rect.Width * 16 - padding * 2), rect, GraphicsUnit.Pixel);
    		        canvas.Save();
    	        }
                if (File.Exists(texturePackPath) == true)
                {
                    //Save inside Zip
                    return InsertBitmapInsideArchive(bitmap, texturePackPath);
                }
                else
                {
                    //Save to Directory
                    return SaveBitmapInFolder(bitmap, texturePackPath);
                }
            }
        }

#endregion

#region Private Functions

        private void LoadSourceFromArchive(string path)
        {
            using (FileStream zipToOpen = new FileStream(path, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry imageEntry = archive.GetEntry(@"art/kz.png");
                    if (imageEntry != null)
                    {
                        Debug.WriteLine("exists");
                        sourceImage = Image.FromStream(imageEntry.Open());
                    }
                    else
                    {
                        Debug.WriteLine("does not exist");
                    }
                }
            }
        }

        private bool InsertBitmapInsideArchive(Bitmap bitmap, string path)
        {
            string tempPath = mcFolder + @"texturepacks\.tmpImg.png";
            try 
            {
                bitmap.Save(tempPath, ImageFormat.Png);
                using (FileStream zipToOpen = new FileStream(path, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        try
                        {
                            archive.CreateEntryFromFile(tempPath, @"art/kz.png");
                            File.Delete(tempPath);
                            return true;
                        }
                        catch (Exception e2)
                        {
                            File.Delete(tempPath);
                            return false;
                        }
                    }
                }
            }
            catch (Exception e1) 
            {
                return false;
            }
        }

        private bool SaveBitmapInFolder(Bitmap bitmap, string path)
        {
            if (Directory.Exists(path + @"\art\") == false)
            {
                Directory.CreateDirectory(path + @"\art\");
            }
            string filePath = path + @"\art\kz.png";
            //string tmpPath = path + @"\art\.oldKz.png";
            string textFilePath = path + @"\pack.txt";
            //if (File.Exists(filePath) == true) File.Move(filePath, tmpPath);
            try
            {
                bitmap.Save(filePath, ImageFormat.Png);
                if (File.Exists(textFilePath) == false)
                {
                    using (StreamWriter file = new StreamWriter(textFilePath)) {
                        file.WriteLine("Made using MCPaintings by Dalton");
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                //if (File.Exists(tmpPath)) File.Move(tmpPath, filePath);
                Debug.WriteLine("Save to Folder: " + e.ToString());
                return false;
            }
        }

#endregion
    }
}
