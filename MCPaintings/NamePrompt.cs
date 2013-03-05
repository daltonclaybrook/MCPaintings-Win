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
    public partial class NamePrompt : Form
    {
        public string texturePackName = null;

        public NamePrompt()
        {
            InitializeComponent();
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            texturePackName = nameBox.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void nameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (doneButton.Enabled)
                    doneButton_Click(null, null);
            }
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            if (nameBox.Text.Length > 0)
            {
                if (doneButton.Enabled == false) doneButton.Enabled = true;
            }
            else
            {
                if (doneButton.Enabled == true) doneButton.Enabled = false;
            }
        }
    }
}
