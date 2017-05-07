using MemoryGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace MemoryGame.ViewModels
{
    public class CardCollectionViewModel : ObservableObject
    {
        public ObservableCollection<CardViewModel> GameCards { get; private set; }

        private CardViewModel FirstSelectedCard;
        private CardViewModel SecondSelectedCard;

        private DispatcherTimer _peekTimer;
        private DispatcherTimer _openingTimer;

        private const int _peekSeconds = 1;
        private const int _openSeconds = 5;
        private int _score = 0;

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }

        public bool areCardsActive
        {
            get
            {
                if (FirstSelectedCard == null || SecondSelectedCard == null)
                    return true;

                return false;
            }
        }

        public bool AllCardsMatched
        {
            get
            {
                foreach(var slide in GameCards)
                {
                    if (!slide.isMatched)
                        return false;
                }

                return true;
            }
        }

        public bool canSelect { get; private set; }


        public CardCollectionViewModel()
        {
            _peekTimer = new DispatcherTimer();
            _peekTimer.Interval = new TimeSpan(0, 0, _peekSeconds);
            _peekTimer.Tick += PeekTimer_Tick;

            _openingTimer = new DispatcherTimer();
            _openingTimer.Interval = new TimeSpan(0, 0, _openSeconds);
            _openingTimer.Tick += OpeningTimer_Tick;
            Score = 0;
        }

        public void CreateCards()
        {
            GameCards = new ObservableCollection<CardViewModel>();
            var models = new List<CardModel>();
            var images = Directory.GetFiles("../../Assets/Cards", "*.png", SearchOption.AllDirectories);

            var id = 0;

            foreach (string i in images)
            {
                models.Add(new CardModel() { Id = id, ImageSource = i });
                id++;
            }


            Random random = new Random();
            List<int> usedNumbers = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                int randomNumber = random.Next(0, models.Count);
                if (!usedNumbers.Contains(randomNumber))
                {
                    var newCard = new CardViewModel(models[randomNumber]);
                    var newCardMatch = new CardViewModel(models[randomNumber]);
                    GameCards.Add(newCard);
                    GameCards.Add(newCardMatch);
                    newCard.PeekAtImage();
                    newCardMatch.PeekAtImage();
                    usedNumbers.Add(randomNumber);
                }
                else
                {
                    i--;
                }
            }

            ShuffleCards();
            OnPropertyChanged("GameCards");
        }

        public void SelectCard(CardViewModel slide)
        {
            if (slide.isViewed)
                return;

            slide.PeekAtImage();

            if (FirstSelectedCard == null)
            {
                FirstSelectedCard = slide;
            }
            else if (SecondSelectedCard == null)
            {
                SecondSelectedCard = slide;
                HideUnmatched();
            }

            OnPropertyChanged("areCardsActive");
        }

        public bool CheckIfMatched()
        {
            if (FirstSelectedCard.Id == SecondSelectedCard.Id)
            {
                MatchCorrect();
                return true;
            }
            else
            {
                MatchFailed();
                return false;
            }
        }

        private void MatchFailed()
        {
            FirstSelectedCard.MarkFailed();
            SecondSelectedCard.MarkFailed();
            Score = Score - 2;
            OnPropertyChanged("Score");
            ClearSelected();
        }

        private void MatchCorrect()
        {
            FirstSelectedCard.MarkMatched();
            SecondSelectedCard.MarkMatched();
            Score = Score + 10;
            OnPropertyChanged("Score");
            ClearSelected();
        }

        private void ClearSelected()
        {
            FirstSelectedCard = null;
            SecondSelectedCard = null;
            canSelect = false;
        }

        public void RevealUnmatched()
        {
            foreach(var slide in GameCards)
            {
                if(!slide.isMatched)
                {
                    _peekTimer.Stop();
                    slide.MarkFailed();
                    slide.PeekAtImage();
                }
            }
        }

        public void HideUnmatched()
        {
            _peekTimer.Start();
        }

        public void Memorize()
        {
            _openingTimer.Start();
        }


        private void ShuffleCards()
        {
            var rnd = new Random();
            for (int i = 0; i < 64; i++)
            {
                GameCards.Reverse();
                GameCards.Move(rnd.Next(0, GameCards.Count), rnd.Next(0, GameCards.Count));
            }
        }

        private void OpeningTimer_Tick(object sender, EventArgs e)
        {
            foreach (var slide in GameCards)
            {
                slide.ClosePeek();
                canSelect = true;
            }
            OnPropertyChanged("areCardsActive");
            _openingTimer.Stop();
        }

        private void PeekTimer_Tick(object sender, EventArgs e)
        {
            foreach(var slide in GameCards)
            {
                if(!slide.isMatched)
                {
                    slide.ClosePeek();
                    canSelect = true;
                }
            }
            OnPropertyChanged("areCardsActive");
            _peekTimer.Stop();
        }
    }
}
