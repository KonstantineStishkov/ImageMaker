using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DrawColor = System.Drawing.Color;

namespace BitmapProjectUI
{
    public struct Point
    {
        public int X;
        public int Y;
    }

    public class ImageCreater
    {
        public ImageCreater()
        {
        }

        public BitmapImage CreateImage(ImageProperties properties, decimal k)
        {
            Bitmap image = new Bitmap(properties.Width, properties.Height);
            Graphics graphics = Graphics.FromImage(image);
            DrawColor color = DrawColor.FromArgb(properties.Color);

            graphics.FillRectangle(new SolidBrush(DrawColor.White), 0, 0, properties.Width, properties.Height);

            Random rnd = new Random();

            Point[] shape = GetShape(properties);
            DrawShape(graphics, shape, color, properties.Thickness);

            Point a = shape[rnd.Next(0, properties.ShapeCount)];
            Point b = a;
            while(b.X == a.X && b.Y == a.Y)
            {
                a = shape[rnd.Next(0, properties.ShapeCount)];
            }

            Point middle = GetMiddlePoint(a, b, k);
            DrawDot(graphics, color, properties.Thickness, middle.X, middle.Y);

            for (int i = 0; i < properties.DotCount; i++)
            {
                int random = rnd.Next(0, properties.ShapeCount);

                Point point = shape[random];

                middle = GetMiddlePoint(middle, point, k);

                DrawDot(graphics, color, properties.Thickness, middle.X, middle.Y);
            }

            return ConvertToImage(image);
        }

        private Point GetMiddlePoint(Point a, Point b, decimal k)
        {
            Point point = new Point();
            point.X = (int)((a.X + b.X) * k);
            point.Y = (int)((a.Y + b.Y) * k);

            return point;
        }

        private Point[] GetShape(ImageProperties properties)
        {
            const double pi = 3.14159;
            double angle = 360 * pi/ (properties.ShapeCount * 180);
            int radius = (Math.Min(properties.Width, properties.Height) / 2) - properties.Offset;

            Point[] points = new Point[properties.ShapeCount];
            Point center = new Point();
            center.X = properties.Width / 2;
            center.Y = properties.Height / 2;

            for (int i = 0; i < properties.ShapeCount; i++)
            {
                points[i] = new Point();
                points[i].X = center.X + (int)(Math.Sin(angle * i) * radius);
                points[i].Y = center.Y + (int)(Math.Cos(angle * i) * radius);
            }

            return points;
        }

        private void DrawShape(Graphics graphics, Point[] shape, DrawColor color, int thickness)
        {
            foreach (Point point in shape)
            {
                DrawDot(graphics, color, thickness, point.X, point.Y);
            }
        }

        private void DrawDot(Graphics graphics, DrawColor color, int thickness, int x, int y)
        {
            Rectangle rec = new Rectangle(x, y, thickness, thickness);
            System.Drawing.Brush brush = new SolidBrush(color);
            graphics.FillRectangle(brush, rec);
        }

        private BitmapImage ConvertToImage(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            src.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
