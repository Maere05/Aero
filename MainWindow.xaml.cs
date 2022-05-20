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

    public partial class MainWindow : Window
    {
        static int h = 600;
        static int w = 600;

        float zoom = 1.5f;
        float yOffset = 50;
        float xOffset = 50;

        float Mass = 60;
        float Speed = 22f;
        float Alpha = 30f;
        float Wind = 0.5f;

        Jumper jumper = new Jumper();
        Bitmap bmp = new Bitmap(h, w);
        Graphics g;
        Pen pen = new Pen(Color.White, 1.5f);
        Pen pen2 = new Pen(Color.OrangeRed, 3.0f);
        Brush brush = new SolidBrush(Color.FloralWhite);
        Vector2 p2;
        Hill[] hills = new Hill[5];
        public MainWindow()
        {
            InitializeComponent();
            initHills();
        }

        public void updateViewport()
        {
            p2 = Vector2.Zero;
            jumper.V = Vector2.Zero;
            jumper.Pos.X = 0;
            jumper.Pos.Y = 0;
            jumper.VelocityAngle = 0;
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.Clear(Color.Black);
            drawHill(HillSelection.SelectedIndex);

            jumper.Init(Mass, 0.01f, Speed, Alpha, Wind); // 22 Kandersteg, 23 Oberstdorf, 27 Vikersund, 28Planica

            for (int i = 0; i < 2000; i++)
            {
                points();
            }

            IntPtr hBitmap = bmp.GetHbitmap();
            //what the heck is this statement
            viewport.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        void points()
        {
            Vector2 p1 = jumper.Timestep();
            g.DrawLine(pen, (p1.X * zoom) + xOffset, (p1.Y * zoom) +yOffset, (p2.X * zoom) +xOffset, (p2.Y * zoom) +yOffset);
            p2 = p1;
        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            updateViewport();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateViewport();
        }
        void initHills()
        {
            hills[0] = new Hill();
            hills[1] = new Hill();
            hills[2] = new Hill();
            hills[3] = new Hill();
            hills[4] = new Hill();

            // Kandersteg
            hills[0].Alpha = 11f;
            hills[0].Beta = 38f;
            hills[0].h = 45.11f;
            hills[0].n = 83.07f;
            hills[0].Zu = 68.93f;
            hills[0].kToFlat = 50f;
            hills[0].SplineDistance = 33f;
            hills[0].S = 2;
            hills[0].l2 = 10;

            // Einsiedeln
            hills[1].Alpha = 10.5f;
            hills[1].Beta = 33.9f;
            hills[1].h = 50.53f;
            hills[1].n = 83.07f;
            hills[1].Zu = 70.26f;
            hills[1].kToFlat = 50f;
            hills[1].SplineDistance = 40f;
            hills[1].S = 2.3f;
            hills[1].l2 = 14.3f;

            //Oberstdorf
            hills[2].Alpha = 11f;
            hills[2].Beta = 35.5f;
            hills[2].h = 59.5f;
            hills[2].n = 103.5f;
            hills[2].Zu = 86f;
            hills[2].kToFlat = 60f;
            hills[2].SplineDistance = 50f; 
            hills[2].S = 3.38f;
            hills[2].l2 = 17.4f;

            //Planica
            hills[3].Alpha = 11.5f;
            hills[3].Beta = 33.2f;
            hills[3].h = 102f;
            hills[3].n = 170.45f;
            hills[3].Zu = 135f;
            hills[3].kToFlat = 100f;
            hills[3].SplineDistance = 70f;
            hills[3].S = 2.93f;
            hills[3].l2 = 40f;

            //Vikersund
            hills[4].Alpha = 11f;
            hills[4].Beta = 38f;
            hills[4].h = 102.2f;
            hills[4].n = 170.5f;
            hills[4].Zu = 135f;
            hills[4].kToFlat = 100f;
            hills[4].SplineDistance = 70f;
            hills[4].S = 2.64f;
            hills[4].l2 = 33;

            HillSelection.Items.Add("Kandersteg HS106");
            HillSelection.Items.Add("Einsiedeln HS117");
            HillSelection.Items.Add("Oberstdorf HS137");
            HillSelection.Items.Add("Planica HS240");
            HillSelection.Items.Add("Vikersund HS240");

            HillSelection.SelectedIndex = 0;
            zoomSlider.Value = 3f;
            zoomSlider.Minimum = 0.7f;

            SpeedSlider.Value = 88f;
            SpeedSlider.Minimum = 50f;
            SpeedSlider.Maximum = 120f;

            MassSlider.Value = 60f;
            MassSlider.Minimum = 20f;

            AoASlider.Value = 30f;
            AoASlider.Minimum = 10f;
            AoASlider.Maximum = 60f;

            windSlider.Value = 0.5f;
            windSlider.Minimum = -5f;
            windSlider.Maximum = 5f;


        }
        void drawHill(int index)
        {
            g.DrawBezier(pen2, xOffset, yOffset + hills[index].S * zoom,

                (MathF.Cos(toRad(hills[index].Alpha)) * hills[index].SplineDistance * zoom) + xOffset, (MathF.Sin(toRad(hills[index].Alpha)) * hills[index].SplineDistance * zoom) + yOffset + hills[index].S * zoom

                , ((hills[index].n - MathF.Cos(toRad(hills[index].Beta)) * hills[index].SplineDistance) * zoom) + xOffset, ((hills[index].h - MathF.Sin(toRad(hills[index].Beta)) * hills[index].SplineDistance) * zoom) + yOffset + hills[index].S * zoom

                , (hills[index].n * zoom) + xOffset, (hills[index].h * zoom) + yOffset + hills[index].S * zoom);

            g.DrawBezier(pen2, (hills[index].n * zoom)+ xOffset, (hills[index].h * zoom) + yOffset + hills[index].S * zoom,

                ((hills[index].n + MathF.Cos(toRad(hills[index].Beta)) * hills[index].SplineDistance) * zoom) + xOffset, ((hills[index].h + MathF.Sin(toRad(hills[index].Beta)) * hills[index].SplineDistance) * zoom) + yOffset + hills[index].S * zoom,

                ((hills[index].n + hills[index].kToFlat) * zoom) + xOffset, (hills[index].Zu * zoom) + yOffset + hills[index].S * zoom,

                ((hills[index].n + hills[index].kToFlat + hills[index].SplineDistance) * zoom) + xOffset, (hills[index].Zu * zoom) + yOffset + hills[index].S * zoom);
            g.DrawLine(pen2, xOffset,yOffset + hills[index].S * zoom,xOffset,yOffset);

            g.DrawBezier(pen2, xOffset, yOffset,
                -(MathF.Cos(toRad(hills[index].Alpha)) * hills[index].SplineDistance * zoom), yOffset - (MathF.Sin(toRad(hills[index].Alpha)) * hills[index].SplineDistance * zoom),
                -10f* zoom ,-10f * zoom,
                -20f * zoom,-20f * zoom );

            // K point
            g.DrawLine(pen, (hills[index].n * zoom) + xOffset, (hills[index].h * zoom) + yOffset + hills[index].S * zoom, (hills[index].n * zoom) + xOffset -5, (hills[index].h * zoom) + yOffset + hills[index].S * zoom + 5);

            g.DrawString("K", new Font("Arial", 6), brush, (hills[index].n * zoom) + xOffset-20, (hills[index].h * zoom) + yOffset + hills[index].S * zoom + 10);

            // HS
            g.DrawLine(pen, ((hills[index].n + MathF.Cos(toRad(hills[index].Beta)) * hills[index].l2) * zoom) + xOffset, ((hills[index].h + MathF.Sin(toRad(hills[index].Beta)) * hills[index].l2) * zoom) + yOffset + hills[index].S * zoom, 
                ((hills[index].n + MathF.Cos(toRad(hills[index].Beta)) * hills[index].l2) * zoom) + xOffset-5, ((hills[index].h + MathF.Sin(toRad(hills[index].Beta)) * hills[index].l2) * zoom) + yOffset + hills[index].S * zoom+5);
            g.DrawString("HS", new Font("Arial", 6), brush, ((hills[index].n + MathF.Cos(toRad(hills[index].Beta)) * hills[index].l2) * zoom) + xOffset - 20, ((hills[index].h + MathF.Sin(toRad(hills[index].Beta)) * hills[index].l2) * zoom) + yOffset + hills[index].S * zoom + 10);
        }
        float toRad(float input)
        {
            return input * MathF.PI / 180f;
        }
        private void HillSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateViewport();
        }

        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            zoom = (float)zoomSlider.Value;
            updateViewport();
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Speed = (float)SpeedSlider.Value / 3.6f;
            SpeedText.Text = MathF.Round( (float)SpeedSlider.Value, 1).ToString() + " km/h";
            updateViewport();

        }
        private void MassSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Mass = (float)MassSlider.Value;
            MassText.Text = MathF.Round( (float)MassSlider.Value, 1).ToString() + " Kg";
            updateViewport();
        }

        private void AoASlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Alpha = (float)AoASlider.Value;
            AoAText.Text = MathF.Round((float)AoASlider.Value, 1).ToString() + "°";
            updateViewport();
        }

        private void windSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Wind = (float)windSlider.Value;
            windText.Text = MathF.Round((float)windSlider.Value, 2).ToString() + "m/s Wind";
            updateViewport();
        }
    }
}
