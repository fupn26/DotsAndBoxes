using System.Windows;
using DotsAndBoxes.Enums;
using DotsAndBoxes.Structures;

namespace DotsAndBoxes.Views
{
    /// <summary>
    ///     Interaction logic for ChooseGrid.xaml
    /// </summary>
    public partial class GridTypeChooserView
    {
        public GridTypeChooserView()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void Classic3x3_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(GameType.Classic, 3);
            NavigateToGameView();
        }

        private void Classic5x5_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(GameType.Classic, 5);
            NavigateToGameView();
        }

        private void Classic6x6_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(GameType.Classic, 6);
            NavigateToGameView();
        }

        private void Diamond3x3_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(GameType.Diamond, 3);
            NavigateToGameView();
        }

        private void Diamond5x5_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(GameType.Diamond, 5);
            NavigateToGameView();
        }

        private void Diamond7x7_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(GameType.Diamond, 7);
            NavigateToGameView();
        }

        private void NavigateToGameView()
        {
            var gameView = new GameView();
            NavigationService?.Navigate(gameView);
        }

        private void SetTypeAndGridSize(GameType gameType, int gridSize)
        {
            GameControllerParameters.GameType = gameType;
            GameControllerParameters.GridSize = gridSize;
        }
    }
}