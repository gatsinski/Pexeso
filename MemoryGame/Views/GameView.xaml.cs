using MemoryGame.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MemoryGame.Views
{
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();
        }

        private void Slide_Clicked(object sender, RoutedEventArgs e)
        {
            var game = DataContext as GameViewModel;
            var button = sender as Button;
            game.ClickedCard(button.DataContext);
        }

        private void PlayAgain_C(object sender, RoutedEventArgs e)
        {
            var game = DataContext as GameViewModel;
            game.Restart();
        }
    }
}
