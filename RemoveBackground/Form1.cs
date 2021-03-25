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
                    Bitmap bmp = MakeGrayscale3(new Bitmap(img));
                    int[,] mat = ConvertToMatrix(bmp);
                    var bin = BinImg(mat);
                    Image res = RemoveBg(img, bin);
                    res.Save(path + @"\result.png");
                }
            }
        }
        private int[,] ConvertToMatrix(Bitmap bmp)
        {
            int[,] bin = new int[bmp.Width, bmp.Height];
            int r, g, b;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    r = bmp.GetPixel(i, j).R;
                    g = bmp.GetPixel(i, j).G;
                    b = bmp.GetPixel(i, j).B;
                    bin[i,j] = (r + g + b) / 3;
                }
            }
            return bin;
        }
        private int[,] BinImg(int[,] img)
        {
            for (int i = 0; i < (img.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < img.Length / (img.GetUpperBound(0) + 1); j++)
                {
                    if (img[i,j] > 100)
                        img[i, j] = 255;
                    else
                        img[i, j] = 0;
                }
            }
            return img;
        }
        private Bitmap RemoveBg(Image img, int[,] bmp)
        {
            Bitmap resimg = new Bitmap(img);
            for (int i = 0; i < resimg.Width; i++)
            {
                for (int j = 0; j < resimg.Height; j++)
                {
                    if (bmp[i, j] == 255)
                        resimg.SetPixel(i, j, Color.Transparent);
                }
            }
            return resimg;
        }
        public Bitmap MakeGrayscale3(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
            new float[] {.3f, .3f, .3f, 0, 0},
            new float[] {.59f, .59f, .59f, 0, 0},
            new float[] {.11f, .11f, .11f, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1}
               });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            g.Dispose();
            return newBitmap;
        }
    }
}
