using DotsAndBoxes.Classes;
using DotsAndBoxes.Enums;
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
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : Page
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

            this.NavigationService.Navigate(new GameTypeChooserView());
        }

        private void Game_Loaded(object sender, RoutedEventArgs e)
        {
            if(sender is GameView game)
            {
                game.LoadComponents();
            }
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            GameType lastGameType = DataProvider.GameStates[^1].GameType;
            GameControllerParameters.IsNewGame = false;

            NavigationService.Navigate(CreateGameView(lastGameType));
        }

        private GameView CreateGameView(GameType gameType)
        {
            GameView gameView = new GameView();

            return gameView;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}
