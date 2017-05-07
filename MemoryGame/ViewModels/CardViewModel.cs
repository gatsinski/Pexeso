using MemoryGame.Models;
using System.Windows.Media;

namespace MemoryGame.ViewModels
{
    public class CardViewModel : ObservableObject
    {
        private CardModel _model;

        public int Id { get; private set; }

        private bool _isViewed;
        private bool _isMatched;
        private bool _isFailed;

        public bool isViewed
        {
            get
            {
                return _isViewed;
            }
            private set
            {
                _isViewed = value;
                OnPropertyChanged("CardImage");
                OnPropertyChanged("BorderBrush");
            }
        }

        public bool isMatched
        {
            get
            {
                return _isMatched;
            }
            private set
            {
                _isMatched = value;
                OnPropertyChanged("CardImage");
                OnPropertyChanged("BorderBrush");
            }
        }

        public bool isFailed
        {
            get
            {
                return _isFailed;
            }
            private set
            {
                _isFailed = value;
                OnPropertyChanged("CardImage");
                OnPropertyChanged("BorderBrush");
            }
        }

        public bool isSelectable
        {
            get
            {
                if (isMatched || isViewed)
                    return false;

                return true;
            }
        }

        public string CardImage
        {
            get
            {
                if (isMatched || isViewed)
                    return _model.ImageSource;

                return "/MemoryGame;component/Assets/card_back.png";
            }
        }

        public Brush BorderBrush
        {
            get
            {
                if (isFailed)
                    return Brushes.Red;
                if (isMatched)
                    return Brushes.Green;
                if (isViewed)
                    return Brushes.Yellow;

                return Brushes.Black;
            }
        }


        public CardViewModel(CardModel model)
        {
            _model = model;
            Id = model.Id;
        }

        public void MarkMatched()
        {
            isMatched = true;
        }

        public void MarkFailed()
        {
            isFailed = true;
        }

        public void ClosePeek()
        {
            isViewed = false;
            isFailed = false;
            OnPropertyChanged("isSelectable");
            OnPropertyChanged("CardImage");
        }


        public void PeekAtImage()
        {
            isViewed = true;
            OnPropertyChanged("CardImage");
        }
    }
}
