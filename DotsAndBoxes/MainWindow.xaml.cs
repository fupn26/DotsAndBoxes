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

namespace DotsAndBoxes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GameWidth = 100;
        private const int GameHeight = 100;

        private readonly int NumberOfPoints;

        private List<Point> points;
        private List<Ellipse> ellipses;

        public MainWindow()
        {
            InitializeComponent();
            NumberOfPoints = (int)canvas.Width / GameWidth * (int)canvas.Height / GameHeight;
            InitGame();
        }

        private void InitGame()
        {
            for (int i = 0; i < NumberOfPoints/2; ++i)
            {
                for (int j = 0; j < NumberOfPoints/2; ++j)
                {
                    DrawPoint(i*GameHeight, j*GameWidth);
                }
            }
        }

        private void DrawPoint(int x, int y)
        {
            // Create pen.
            Pen blackPen = new Pen(Brushes.Black, 3);

            // Create rectangle for ellipse.
//            Rectangle rect = new Rectangle(0, 0, 200, 100);

            Ellipse ellipse = new Ellipse();
            ellipse.Height = 10;
            ellipse.Width = 10;
            ellipse.Fill = Brushes.Red;

            Canvas.SetLeft(ellipse, y-10/2);
            Canvas.SetTop(ellipse, x-10/2);
            canvas.Children.Add(ellipse);
            ellipses.Add(ellipse);

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //ScorePlayer1.Text = "Left mouse pressed";
            //Console.WriteLine("Left mouse pressed");
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            //ScorePlayer2.Text = "MouseMove";
            //Console.WriteLine("MouseMove");
        }
    }
}
