using DotsAndBoxes.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DotsAndBoxes
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
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

        private void OnCenterWindow()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if(MainFrame.Content is Page actualPage)
            {
                actualPage.Loaded += ActualPage_Loaded;
            }
        }

        private void ActualPage_Loaded(object sender, RoutedEventArgs e)
        {
            OnCenterWindow();
        }
    }
}
