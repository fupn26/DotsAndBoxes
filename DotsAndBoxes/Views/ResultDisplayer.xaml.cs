using System;
using DotsAndBoxes.Classes;

namespace DotsAndBoxes.Views
{
    /// <summary>
    ///     Interaction logic for ResultDisplayer.xaml
    /// </summary>
    public partial class ResultDisplayer
    {
        public ResultDisplayer()
        {
            InitializeComponent();
        }

        public ResultDisplayer(GameState gameState)
        {
            InitializeComponent();
            Player1Name.Content = gameState.Player1;
            Player1Name.Icon = gameState.Player1[0];
            Player2Name.Content = gameState.Player2;
            Player2Name.Icon = gameState.Player2[0];
            Player1Score.Text = gameState.Scores[0].ToString();
            Player2Score.Text = gameState.Scores[1].ToString();
            GameMode.Text = gameState.GameMode.ToString();
            GameType.Text = gameState.GameType.ToString();
            var size = gameState.GridSize.ToString();
            GridSize.Text = size + "x" + size;
            var time = TimeSpan.FromSeconds(gameState.Length);
            Length.Text = time.ToString("mm':'ss");
        }
    }
}