using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xnaDemo
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            StreamWriter fileStream = new StreamWriter("Settings.txt");

            //Writes skill level and invicibility stat (on or not) to file.
            fileStream.WriteLine(optionInvincible.Checked);
            fileStream.WriteLine(skillLevel.SelectedIndex.ToString());

            fileStream.Close();
        }
    }
}
