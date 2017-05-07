using System;

namespace MemoryGame.ViewModels
{
    public enum SlideCategories
    {
        Cards
    }

    public class GameViewModel : ObservableObject
    {
        public CardCollectionViewModel Cards { get; private set; }
        public TimerViewModel Timer { get; private set; }

        private int _matchAttempts;

        public int MatchAttempts
        {
            get
            {
                return _matchAttempts;
            }
            set
            {
                _matchAttempts = value;
            }
        }

        public GameViewModel()
        {
            SetupGame();
        }

        private void SetupGame()
        {
            Cards = new CardCollectionViewModel();
            Timer = new TimerViewModel(new TimeSpan(0, 0, 1));

            MatchAttempts = 10;

            Cards.CreateCards();
            Cards.Memorize();

            Timer.Start();

            OnPropertyChanged("Cards");
            OnPropertyChanged("Timer");
        }

        public void ClickedCard(object slide)
        {
            if(Cards.canSelect)
            {
                var selected = slide as CardViewModel;
                Cards.SelectCard(selected);
            }

            if(!Cards.areCardsActive)
            {
                if (!Cards.CheckIfMatched())
                    MatchAttempts--;
            }

            GameStatus();
        }

        private void GameStatus()
        {
            if(MatchAttempts < 0)
            {
                Cards.RevealUnmatched();
                Timer.Stop();
            }

            if(Cards.AllCardsMatched)
            {
                Timer.Stop();
            }
        }

        public void Restart()
        {
            SetupGame();
        }
    }
}
