using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;

namespace face_detection
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> GrayScale;
        public Form1()
        {
            InitializeComponent();
        }
        VideoCaptureDevice vcd;

        CascadeClassifier cc = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
        private void Form1_Load(object sender, EventArgs e)
        {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            vcd = new VideoCaptureDevice(videoDevices[0].MonikerString);
            vcd.NewFrame += vcd_NewFrame;
            vcd.Start();
        }


        private void vcd_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            
            using (Graphics g = Graphics.FromImage(bitmap)) bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Bitmap resized = new Bitmap(bitmap, new Size(bitmap.Width/3, bitmap.Height/3));
            
            
            GrayScale = new Image<Bgr, byte>(resized);
            Rectangle[] rectangles = cc.DetectMultiScale(GrayScale, 1.1, 4);
            foreach(Rectangle rectangle in rectangles)
            {
                using(Graphics gr = Graphics.FromImage(resized))
                {
                    using (Pen pen = new Pen(Color.Red, 1)) gr.DrawRectangle(pen, rectangle);
                    using(Font arealFont = new Font("Arial", 15))
                    {
                        gr.DrawString("Dumbass", arealFont, Brushes.Red, rectangle);
                    }
                }
                
            }
            pictureBox1.Image = resized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vcd.IsRunning) vcd.Stop();
        }
    }
}
