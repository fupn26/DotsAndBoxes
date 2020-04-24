using DotsAndBoxes.Classes;
using DotsAndBoxes.Structures;
using System.Windows;

namespace DotsAndBoxes.Views
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView
    {
        public WelcomeView()
        {
            InitializeComponent();

            if (CanLoadGameState())
            {
                Load.Visibility = Visibility.Visible;
            }
        }

        private bool CanLoadGameState()
        {
            if (DataProvider.GameStates.Count != 0)
            {
                return !DataProvider.GameStates[^1].IsEnded;
            }

            else return false;
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (CanLoadGameState())
            {
                DataProvider.RemoveLastElement();
                DataProvider.CommitChanges();
            }

            GameControllerParameters.IsNewGame = true;

            NavigationService?.Navigate(new GameModeChooserView());
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            GameControllerParameters.IsNewGame = false;

            NavigationService?.Navigate(CreateGameView());
        }

        private GameView CreateGameView()
        {
            GameView gameView = new GameView();

            return gameView;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
