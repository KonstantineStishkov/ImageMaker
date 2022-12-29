using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BitmapProjectUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FillPictureBox(ImageProperties properties, decimal k)
        {
            ImageCreater maker = new ImageCreater();
            ImageBox.Source = maker.CreateImage(properties, k);
        }

        private void BTN_Draw_Click(object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(TB_PointCount.Text, out int count)
                || !int.TryParse(TB_ShapeCount.Text, out int shapeCount)
                || !int.TryParse(TB_PointThickness.Text, out int thickness)
                || !decimal.TryParse(TB_K.Text, out decimal k)
                || !int.TryParse(TB_Offset.Text, out int offset))
            {
                return;
            }

            int color;

            switch (CB_Color.SelectedIndex)
            {
                case 0: color = -65536; break;
                case 1: color = -16711936; break;
                case 2: color = -16776961; break;
                default: color = -16777216; break;
            }

            ImageProperties properties = new ImageProperties()
            {
                Width = (int)ImageBox.Width,
                Height = (int)ImageBox.Height,
                Thickness = thickness,
                Offset = offset,
                ShapeCount = shapeCount,
                DotCount = count,
                Color = color
            };

            FillPictureBox(properties, k);
        }
    }
}
