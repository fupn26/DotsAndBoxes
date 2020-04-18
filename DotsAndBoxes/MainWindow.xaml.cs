using DotsAndBoxes.Classes;
using DotsAndBoxes.Structures;
using DotsAndBoxes.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Drawing.Point;

namespace DotsAndBoxes
{
    public partial class MainWindow : Window
    {
        //private GameController gameController;

        //public event EventHandler InitScore;

        //public event EventHandler RestoreState;

        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new ClassicGameViewModel();
            DataContext = new DiamondGameViewModel();
            //DataContext = new WelcomeViewModel();
        }

    }
}
