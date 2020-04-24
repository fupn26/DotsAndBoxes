using DotsAndBoxes.Classes;
using System.Windows;

namespace DotsAndBoxes.Views
{
    /// <summary>
    /// Interaction logic for ResultsView.xaml
    /// </summary>
    public partial class ResultsView
    {
        public ResultsView()
        {
            InitializeComponent();
            var last = new ResultDisplayer(DataProvider.GameStates[^1]);
            LastGame.Children.Add(last);
            for (int i = DataProvider.GameStates.Count-2; i >= 0; --i)
            {
                var prev = new ResultDisplayer(DataProvider.GameStates[i]);
                prev.Scale.CenterX = prev.Width / 2;
                prev.Scale.ScaleX = 0.9;
                prev.Scale.ScaleY = 0.9;
                PrevGames.Children.Add(prev);
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new WelcomeView());
        }

        private void RematchButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new GameView());
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
