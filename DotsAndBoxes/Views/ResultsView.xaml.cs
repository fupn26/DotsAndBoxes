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
        public ObservableCollection<GameState> PrevGameStates { get; }
        public ResultsView()
        {
            InitializeComponent();
            PrevGameStates = new ObservableCollection<GameState>(DataProvider.GameStates);
        }
    }
}
