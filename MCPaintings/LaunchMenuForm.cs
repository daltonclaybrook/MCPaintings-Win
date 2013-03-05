using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MCPaintings
{
    public delegate void SelectedPaintingDelegate(Painting painting);
    public delegate void CropRectForNewImageDelegate(Image image, Rectangle rect, bool preserveBorder);

    public partial class LaunchMenuForm : Form
    {
        private PaintingsController paintingsController;
        private NamePrompt namePrompt;
        private SelectPaintingForm selectPaintingForm;
        private CropForm cropForm;
        private List<string> texturePacks;
        private string mcFolder = null;
        private Painting selectedPainting = null;

        public LaunchMenuForm()
        {
            InitializeComponent();
            mcFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\";
            LoadTexturePackFolder();
        }

        private void LoadTexturePackFolder()
        {
            if (Directory.Exists(mcFolder) == false)
            {
                MessageBox.Show("Your Minecraft folder could not be located.");
                Environment.Exit(0);
                return;
            }
            string texturePackFolder = mcFolder + "texturepacks\\";
            if (Directory.Exists(texturePackFolder))
            {
                string[] tempTexturePacks = Directory.GetFileSystemEntries(texturePackFolder);
                texturePacks = new List<string>();
                texturePacksView.Items.Clear();

                for (int i = 0; i < tempTexturePacks.Length; i++)
                {
                    string shortPackName = tempTexturePacks[i].Substring(texturePackFolder.Length);
                    ListViewItem item = new ListViewItem(shortPackName);
                    texturePacksView.Items.Add(item);
                    texturePacks.Add(shortPackName);
                }
            }
            else
            {
                Directory.CreateDirectory(texturePackFolder);
            }
        }

        private void createNewButton_Click(object sender, EventArgs e)
        {
            Reset();

            namePrompt = new NamePrompt();
            if (namePrompt.ShowDialog() == DialogResult.OK)
            {
                paintingsController = new PaintingsController();
                paintingsController.LoadSourceForNewTexturePack(namePrompt.texturePackName);
                selectPaintingForm = new SelectPaintingForm(paintingsController);
                selectPaintingForm.selectedPaintingCallback = new SelectedPaintingDelegate(SelectedPainting);
                selectPaintingForm.Show();

                namePrompt = null;
            }
        }

        private void modifyButton_Click(object sender, EventArgs e)
        {
            Reset();

            int selectedIndex = texturePacksView.SelectedIndices[0];
            string texturePackPath = mcFolder + @"texturepacks\" + texturePacks[selectedIndex];

            paintingsController = new PaintingsController();
            paintingsController.LoadSourceForTexturePack(texturePackPath);
            selectPaintingForm = new SelectPaintingForm(paintingsController);
            selectPaintingForm.selectedPaintingCallback = new SelectedPaintingDelegate(SelectedPainting);
            selectPaintingForm.Show();
        }

        private void texturePacksView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (texturePacksView.SelectedIndices.Count > 0)
            {
                if (modifyButton.Enabled == false) modifyButton.Enabled = true;
            }
            else
            {
                if (modifyButton.Enabled == true) modifyButton.Enabled = false;
            }
        }

        private void Reset()
        {
            paintingsController = null;
            selectedPainting = null;
            if (namePrompt != null)
            {
                if (namePrompt.Visible) namePrompt.Close();
                namePrompt = null;
            }
            if (selectPaintingForm != null)
            {
                if (selectPaintingForm.Visible) selectPaintingForm.Close();
                selectPaintingForm = null;
            }
            if (cropForm != null)
            {
                if (cropForm.Visible) cropForm.Close();
                cropForm = null;
            }
        }

        #region Delegate Methods

        private void SelectedPainting(Painting painting)
        {
            selectedPainting = painting;
            selectPaintingForm.Close();
            selectPaintingForm = null;

            if ((cropForm != null) && (cropForm.Visible == true)) cropForm.Close();
            cropForm = new CropForm();
            cropForm.cropCallback = new CropRectForNewImageDelegate(CropRectForNewImage);
            cropForm.sourceRect = painting.rect;
            cropForm.Show();
        }

        private void CropRectForNewImage(Image image, Rectangle rect, bool preserveBorder)
        {
            cropForm.Close();
            cropForm = null;
            if (paintingsController.SaveImageOverPainting(image, rect, selectedPainting, preserveBorder) == true)
            {
                MessageBox.Show("Success!");
            }
            else
            {
                MessageBox.Show("Sorry. It failed.");
            }
            paintingsController = null;
            LoadTexturePackFolder();
            Reset();
        }

        #endregion
    }
}
