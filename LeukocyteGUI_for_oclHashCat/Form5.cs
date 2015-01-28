﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeukocyteGUI_for_oclHashCat
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void textBoxHashcat_TextChanged(object sender, EventArgs e)
        {
            TryAutoDetectWorkingDirectory();
        }

        private void checkBoxAutoWorkingDirectory_CheckedChanged(object sender, EventArgs e)
        {
            TryAutoDetectWorkingDirectory();
        }

        private void TryAutoDetectWorkingDirectory()
        {
            if (checkBoxAutoWorkingDirectory.Checked)
            {
                try
                {
                    textBoxWorkingDirectory.Text = System.IO.Path.GetDirectoryName(textBoxHashcat.Text);
                }
                catch (Exception)
                {
                    textBoxWorkingDirectory.Text = "";
                }
                    
            }

            textBoxWorkingDirectory.Enabled = !checkBoxAutoWorkingDirectory.Checked;
        }

        private void buttonSettingsOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Hashcat = textBoxHashcat.Text;
            Properties.Settings.Default.WorkingDirectory = textBoxWorkingDirectory.Text;
            Properties.Settings.Default.Save();

            Close();
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            textBoxHashcat.Text = Properties.Settings.Default.Hashcat;
            textBoxWorkingDirectory.Text = Properties.Settings.Default.WorkingDirectory;
        }
    }
}
