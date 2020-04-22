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
    /// Interaction logic for GameTypeChooserView.xaml
    /// </summary>
    public partial class GameTypeChooserView : Page
    {
        public GameTypeChooserView()
        {
            InitializeComponent();
        }

        private void DialogHost_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if (sender is MaterialDesignThemes.Wpf.DialogHost dialog)
            {
                if(Player1.Text == "")
                {
                    dialog.IsOpen = true;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(Player1.Text))
            {
                return;
            }
            else if (String.IsNullOrEmpty(Player2.Text) && Player2Field.Visibility != Visibility.Collapsed)
            {
                return;
            }
            else
            {
                DialogHost.IsOpen = false;
                GameControllerParameters.Player1 = Player1.Text;
                GameControllerParameters.Player2 = Player2.Text;
                this.NavigationService.Navigate(new GridTypeChooserView());
            }
        }

        private void SingleButton_Click(object sender, RoutedEventArgs e)
        {
            Player2Field.Visibility = Visibility.Collapsed;
            DialogHost.IsOpen = true;
            GameControllerParameters.GameMode = Enums.GameMode.SINGLE;
        }

        private void MultiButton_Click(object sender, RoutedEventArgs e)
        {
            Player2Field.Visibility = Visibility.Visible;
            DialogHost.IsOpen = true;
            GameControllerParameters.GameMode = Enums.GameMode.MULTI;
        }
    }
}
