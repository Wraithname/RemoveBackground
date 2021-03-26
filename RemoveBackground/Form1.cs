using OtsuThreshold;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace RemoveBackground
{
    public partial class Form1 : Form
    {
        Image img;
        string path = "";
        private Otsu ot = new Otsu();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    img = new Bitmap(ofd.FileName);
                    button2.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    path = fbd.SelectedPath;
                    Bitmap temp = (Bitmap)img.Clone();
                    ot.Convert2GrayScaleFast(temp);
                    int otsuThreshold = ot.getOtsuThreshold((Bitmap)temp);
                    ot.threshold(temp, otsuThreshold);
                    //Image res = RemoveBg(img, bin);
                    //res.Save(path + @"\result.png");
                }
            }
        }
        private Bitmap RemoveBg(Image img, Bitmap bmp)
        {
            Bitmap resimg = new Bitmap(img);
            for (int i = 0; i < resimg.Width; i++)
            {
                for (int j = 0; j < resimg.Height; j++)
                {
                    if (bmp.GetPixel(i,j).R == 255)
                        resimg.SetPixel(i, j, Color.Transparent);
                }
            }
            return resimg;
        }
    }
}
