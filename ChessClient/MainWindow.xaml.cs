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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PaintMap();
        }
        void PaintMap()
        {
            int table_POS_X = 100;
            int table_POS_y = 100;
            int header_POS_y = 65;
            int header_POS_x = 65;
            List<Rectangle> cells = new List<Rectangle>();
            
            for (int i = 0; i < 8; i++) {
                Label lab1 = new Label();
                lab1.Width = 25;
                lab1.Height = 25;
                lab1.VerticalAlignment = VerticalAlignment.Center;
                lab1.BorderBrush = Brushes.Black;
                lab1.Content = (i + 1).ToString();
                Canvas1.Children.Add(lab1);
                Canvas.SetLeft(lab1, table_POS_X + i * 52);
                Canvas.SetTop(lab1, header_POS_y);

                Label lab2 = new Label();
                lab2.Width = 25;
                lab2.Height = 25;
                lab2.VerticalAlignment = VerticalAlignment.Center;
                int a = (int)'A';
                a += i;
                lab2.Content = ((char)a).ToString();
                Canvas1.Children.Add(lab2);
                Canvas.SetLeft(lab2, header_POS_x);
                Canvas.SetTop(lab2, table_POS_y + i * 52);
            }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var el = new Rectangle();
                    el.Width = 50;
                    el.Height = 50;
                    el.VerticalAlignment = VerticalAlignment.Top;
                    el.Fill = (i + j) % 2 == 0 ? Brushes.Black : Brushes.White;
                    el.StrokeThickness = 1;
                    Canvas1.Children.Add(el);
                    Canvas.SetLeft(el, table_POS_X + i * 52);
                    Canvas.SetTop(el, table_POS_y +j * 52);
                }
        }
    }
}
