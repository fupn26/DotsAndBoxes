using DotsAndBoxes.Classes;
using DotsAndBoxes.Structures;
using DotsAndBoxes.ViewModels;
using DotsAndBoxes.Views;
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
        public event EventHandler GameViewNeedToLoadComponent;

        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new ClassicGameViewModel();
            //DataContext = new DiamondGameViewModel();
            //DataContext = new WelcomeViewModel();
        }

        private void OnGameViewNeedToLoadComponents()
        {
            GameViewNeedToLoadComponent?.Invoke(this, EventArgs.Empty);
        }

        private void GameView_Loaded(object sender, RoutedEventArgs e)
        {
            var view = (GameView)sender;
            GameViewNeedToLoadComponent += view.MainWindow_NeedToLoadComponents;
            OnGameViewNeedToLoadComponents();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //WelcomeView welcome = new WelcomeView();
            //MainFrame.NavigationService.Navigate(welcome);
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            MainFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            WelcomeView welcome = new WelcomeView();
            MainFrame.NavigationService.Navigate(welcome);
        }
    }
}
