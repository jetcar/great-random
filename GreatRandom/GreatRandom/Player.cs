using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        public Player(int numbersAmount, string name, int ticketAmount, int stake, bool sameNumbers, int money, int system,int hotnumbers, int coldnumbers)
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

        public override string ToString()
        {
            return String.Format("\nname {0} \n money {1}\n tickets {2} \n samenumbers {3} \n numbers {4} \n played {5} \n stake {6} \nsystem {7}",Name,Money,NumberOfTickets,SameNumbers,NumbersAmount,GamesPlayed,Stake,System);
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
