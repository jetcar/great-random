using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GreatRandom.Annotations;

namespace GreatRandom
{
    public class Player : INotifyPropertyChanged
    {
        private int _numberOfTickets;
        private int _numbersAmount;
        private bool _sameNumbers;
        private int _stake;
        private int _money;
        private string _name;
        private IList<Ticket> _tickets = new List<Ticket>();
        private int _gamesPlayed;
        private int _spendMoney;
        private int _system;
        private int _hotNumbers;
        private int _coldNumbers;
        private int _currentMinus;
        private int _biggestMinus;
        private int _coldRange;
        private int _hotRange;
        private int _statRange;
        private bool _showTickets;
        private Visibility _showTicketsVisibility = Visibility.Collapsed;

        public Player(int numbersAmount, string name, int ticketAmount, int stake, bool sameNumbers, int money, int system, int hotnumbers, int coldnumbers, int hotRange, int coldRange,int statRange)
        {
            NumbersAmount = numbersAmount;
            Name = name;
            NumberOfTickets = ticketAmount;
            Stake = stake;
            SameNumbers = sameNumbers;
            Money = money;
            System = system;
            HotNumbers = hotnumbers;
            ColdNumbers = coldnumbers;
            HotRange = hotRange;
            ColdRange = coldRange;
            StatRange = statRange;
        }

        public Visibility ShowTicketsVisibility
        {
            get { return _showTicketsVisibility; }
            set
            {
                if (value == _showTicketsVisibility) return;
                _showTicketsVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool ShowTickets
        {
            get { return _showTickets; }
            set
            {
                if (value == _showTickets) return;
                _showTickets = value;
                if (value)
                {
                    ShowTicketsVisibility = Visibility.Visible;
                }
                else
                {
                    ShowTicketsVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }

        public int ColdRange
        {
            get { return _coldRange; }
            set
            {
                if (value == _coldRange) return;
                _coldRange = value;
                OnPropertyChanged();
            }
        }

        public int HotRange
        {
            get { return _hotRange; }
            set
            {
                if (value == _hotRange) return;
                _hotRange = value;
                OnPropertyChanged();
            }
        }


        public int HotNumbers
        {
            get { return _hotNumbers; }
            set
            {
                if (value == _hotNumbers) return;
                _hotNumbers = value;
                OnPropertyChanged();
            }
        }

        public int ColdNumbers
        {
            get { return _coldNumbers; }
            set
            {
                if (value == _coldNumbers) return;
                _coldNumbers = value;
                OnPropertyChanged();
            }
        }

        public int GamesPlayed
        {
            get { return _gamesPlayed; }
            set
            {
                if (value == _gamesPlayed) return;
                _gamesPlayed = value;
                OnPropertyChanged();
            }
        }

        public IList<Ticket> Tickets
        {
            get { return _tickets; }
            set
            {
                if (Equals(value, _tickets)) return;
                _tickets = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfTickets
        {
            get { return _numberOfTickets; }
            set
            {
                if (value == _numberOfTickets) return;
                _numberOfTickets = value;
                OnPropertyChanged();
            }
        }

        public int NumbersAmount
        {
            get { return _numbersAmount; }
            set
            {
                if (value == _numbersAmount) return;
                _numbersAmount = value;
                OnPropertyChanged();
            }
        }

        public bool SameNumbers
        {
            get { return _sameNumbers; }
            set
            {
                if (value == _sameNumbers) return;
                _sameNumbers = value;
                OnPropertyChanged();
            }
        }

        public int Stake
        {
            get { return _stake; }
            set
            {
                if (value == _stake) return;
                _stake = value;
                OnPropertyChanged();
            }
        }

        public int Money
        {
            get { return _money; }
            set
            {
                if (value == _money) return;
                _money = value;
                OnPropertyChanged();
            }
        }

        public int SpendMoney
        {
            get { return _spendMoney; }
            set
            {
                if (value == _spendMoney) return;
                _spendMoney = value;
                OnPropertyChanged();
            }
        }

        public int System
        {
            get { return _system; }
            set
            {
                if (value == _system) return;
                _system = value;
                OnPropertyChanged();
            }
        }

        public int BiggestMinus
        {
            get { return _biggestMinus; }
            set
            {
                if (value == _biggestMinus) return;
                _biggestMinus = value;
                OnPropertyChanged();
            }
        }

        public int CurrentMinus
        {
            get { return _currentMinus; }
            set
            {
                if (value == _currentMinus) return;
                _currentMinus = value;
                if (_currentMinus > BiggestMinus)
                    BiggestMinus = _currentMinus;
                OnPropertyChanged();
            }
        }

        public int StatRange
        {
            get { return _statRange; }
            set
            {
                if (value == _statRange) return;
                _statRange = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", NumberOfTickets, SameNumbers, NumbersAmount, GamesPlayed, Stake, System,ColdNumbers,ColdRange,HotNumbers,HotRange,StatRange,System);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
