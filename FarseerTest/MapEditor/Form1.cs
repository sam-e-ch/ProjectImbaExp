using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEditor
{
    public partial class Form1 : Form
    {
        private List<string> files = new List<string>();
        private List<Image> images = new List<Image>();
        private List<PictureBox> pictures = new List<PictureBox>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            files.AddRange(openFileDialog1.FileNames);
            LoadImages();
        }

        private void LoadImages()
        {
            images.Clear();
            foreach(string s in files)
            {
                images.Add( Image.FromFile(s));                
            }

            pictures.Clear();
            foreach (Image i in images)
            {
                pictures.Add(new PictureBox());
                pictures.Last().Image = i;
                pictures.Last().Size = i.Size;
                splitContainer1.Panel2.Controls.Add(pictures.Last());
            }
        }
    }
}
