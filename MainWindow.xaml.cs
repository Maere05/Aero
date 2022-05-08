using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;


namespace Aero
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>e
    /// 


    public partial class MainWindow : Window
    {
        static int h = 500;
        static int w = 500;

        float zoom = 3f;
        float yOffset = 0;
        float xOffset = 0;  

        Jumper jumper;

        Bitmap bmp = new Bitmap(h, w);
        Graphics g;
        Pen pen = new Pen(Color.White, 2.0f);

        public MainWindow()
        {
            InitializeComponent();
            initBitmap();

        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            jumper = new Jumper();
            jumper.V = Vector2.Zero;
            jumper.Pos.X = 0;
            jumper.Pos.Y = 0;
            jumper.VelocityAngle = 0;

            g = Graphics.FromImage(bmp);

            Path path = new Path();


            g.Clear(Color.Black);
            //g.DrawBezier(pen, 0, 0, 300, 0, 200, 500, 500, 500);




            txtOut.Text = jumper.Init((float)Sld1.Value,0.1f, 29f) + "----------------\n\n";


            for (int i = 0; i < 45; i++)
            {
                points();
            }




            IntPtr hBitmap = bmp.GetHbitmap();

            //what the heck is this statement
            viewport.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
                (hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private void initBitmap()
        {

        }

        private void points()
        {
            Vector2 p1 = jumper.Timestep();

            xOffset = 0;
            yOffset = 0;

            //txtOut.Text += jumper.V.Length().ToString() + " ;";
            //txtOut.Text += jumper.FL.Length().ToString() + " ;";
            //txtOut.Text += jumper.FD.Length().ToString() + "\n";

            txtOut.Text += (jumper.VelocityAngle / MathF.PI*180f).ToString() + "\n";






            g.DrawLine(pen, (int)p1.X*zoom + xOffset, (int)p1.Y*zoom + yOffset, (int)p1.X*zoom + 3 + xOffset, (int)p1.Y*zoom + 3 + yOffset);

        }


    }
}
