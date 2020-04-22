using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotsAndBoxes.Views
{
    /// <summary>
    /// Interaction logic for ChooseGrid.xaml
    /// </summary>
    public partial class GridTypeChooserView : Page
    {
        public GridTypeChooserView()
        {
            InitializeComponent();
        }

        private void SetTypeAndGridSize(Enums.GameType gameType, int gridSize)
        {
            GameControllerParameters.GameType = gameType;
            GameControllerParameters.GridSize = gridSize;
        }

        private void NavigateToGameView()
        {
            GameView gameView = new GameView();
            this.NavigationService.Navigate(gameView);
            
        }

        private void Diamond3x3_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(Enums.GameType.DIAMOND, 3);
            NavigateToGameView();

        }

        private void Diamond5x5_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(Enums.GameType.DIAMOND, 5);
            NavigateToGameView();
        }

        private void Diamond7x7_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(Enums.GameType.DIAMOND, 7);
            NavigateToGameView();
        }

        private void Classic3x3_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(Enums.GameType.CLASSIC, 3);
            NavigateToGameView();
        }

        private void Classic5x5_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(Enums.GameType.CLASSIC, 5);
            NavigateToGameView();
        }

        private void Classic6x6_Click(object sender, RoutedEventArgs e)
        {
            SetTypeAndGridSize(Enums.GameType.CLASSIC, 6);
            NavigateToGameView();
        }
    }
}
