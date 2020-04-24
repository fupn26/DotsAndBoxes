using DotsAndBoxes.Classes;
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
    /// Interaction logic for ResultDisplayer.xaml
    /// </summary>
    public partial class ResultDisplayer : UserControl
    {

        public ResultDisplayer()
        {
            InitializeComponent();
        }

        public ResultDisplayer(GameState gameState)
        {
            InitializeComponent();
            Player1Name.Content = gameState.Player1;
            Player2Name.Content = gameState.Player2;
            Player1Score.Text = gameState.Scores[0].ToString();
            Player2Score.Text = gameState.Scores[1].ToString();
            GameMode.Text = gameState.GameMode.ToString();
            GameType.Text = gameState.GameType.ToString();
            string size = gameState.GridSize.ToString();
            GridSize.Text = size + "x" + size;
            Length.Text = gameState.Length.ToString();
        }
    }
}
