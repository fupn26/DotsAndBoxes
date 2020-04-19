using DotsAndBoxes.CustomEventArgs;
using DotsAndBoxes.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace DotsAndBoxes.Classes
{
    interface IBaseGameView
    {
        GameController gameController { get; set; }

        event EventHandler InitScore;

        event EventHandler RestoreState;

        event EventHandler RestartGame;

        bool isCanvasEnabled { get; set; }

        int Time { get; set; }

        DispatcherTimer Timer { get; }


        void timer_Tick(object sender, EventArgs e);
        void GameController_RectangleEnclosed(object sender, RectangleEventArgs e);

        void GameController_ScoreChanged(object sender, EventArgs e);

        void InitGame();
        void OnRestoreState();
        void OnInitScore();
        void DrawEllipses();
        void DrawRectangle(RectangleStructure rectangle);
        void DrawLines();
        void RestartButton_Click(object sender, System.Windows.RoutedEventArgs e);
        void PauseButton_Click(object sender, System.Windows.RoutedEventArgs e);
    }

}
