using System.Windows;
using DotsAndBoxes.Classes;
using DotsAndBoxes.Structures;

namespace DotsAndBoxes.Views
{
    /// <summary>
    ///     Interaction logic for ResultsView.xaml
    /// </summary>
    public partial class ResultsView
    {
        public ResultsView()
        {
            InitializeComponent();
            var last = new ResultDisplayer(DataProvider.GameStates[^1]);
            LastGame.Children.Add(last);
            for (var i = DataProvider.GameStates.Count - 2; i >= 0; --i)
            {
                var prev = new ResultDisplayer(DataProvider.GameStates[i]);
                prev.Scale.CenterX = prev.Width / 2;
                prev.Scale.ScaleX = 0.9;
                prev.Scale.ScaleY = 0.9;
                PrevGames.Children.Add(prev);
            }
        }

        private void FillGameContParameters()
        {
            if (!GameControllerParameters.IsNewGame)
            {
                var last = DataProvider.GameStates[^1];
                GameControllerParameters.IsNewGame = true;
                GameControllerParameters.GameMode = last.GameMode;
                GameControllerParameters.GameType = last.GameType;
                GameControllerParameters.GridSize = last.GridSize;
                GameControllerParameters.Player1 = last.Player1;
                GameControllerParameters.Player2 = last.Player2;
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new WelcomeView());
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RematchButton_Click(object sender, RoutedEventArgs e)
        {
            FillGameContParameters();
            NavigationService?.Navigate(new GameView());
        }
    }
}