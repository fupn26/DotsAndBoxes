using DotsAndBoxes.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ResultsView.xaml
    /// </summary>
    public partial class ResultsView : Page
    {
        public ResultsView()
        {
            InitializeComponent();
            LastGame.Children.Add(new ResultDisplayer(DataProvider.GameStates[^1]));
            for (int i = 1; i < DataProvider.GameStates.Count; ++i)
            {
                PrevGames.Children.Add(new ResultDisplayer(DataProvider.GameStates[i]));
            }
        }
    }
}
