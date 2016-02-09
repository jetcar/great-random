using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GreatRandom.Annotations;
using Newtonsoft.Json;

namespace GreatRandom
{
    public class Player : BaseModel, INotifyPropertyChanged
    {
        private IList<Ticket> _tickets = new List<Ticket>();
        private int _gamesPlayed;
        private int _spendMoney;
        private int _currentMinus;
        private int _biggestMinus;
        private bool _showTickets;
        private Visibility _showTicketsVisibility = Visibility.Collapsed;
        private Random _random;
        private bool _saved;
        private string _name;
        private int _money;
        private Property _coldRange;
        private Property _hotRange;
        private Property _hotNumbers;
        private Property _coldNumbers;
        private Property _numberOfTickets;
        private Property _stake;
        private Property _statRange;
        private Property _system;
        private Property _ticketChangeLost;
        private Property _ticketChangeWon;
        private Property _luckyAmount;
        private IList<int> _luckyNumbers = new List<int>();

        public Player()
        {
            _coldRange = GetProperty("ColdRange");
            _hotRange = GetProperty("HotRange");
            _coldNumbers = GetProperty("ColdNumbers");
            _hotNumbers = GetProperty("HotNumbers");
            _numberOfTickets = GetProperty("NumberOfTickets");
            _stake = GetProperty("Stake");
            _statRange = GetProperty("StatRange");
            _system = GetProperty("System");
            _ticketChangeLost = GetProperty("TicketChangeLost");
            _ticketChangeWon = GetProperty("TicketChangeWon");
            _luckyAmount = GetProperty("LuckyAmount");
        }

        [JsonProperty]
        public IList<int> LuckyNumbers
        {
            get { return _luckyNumbers; }
            set { _luckyNumbers = value; }
        }

        [JsonIgnoreAttribute]
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

        public bool Saved
        {
            get { return _saved; }
            set
            {
                if (value == _saved) return;
                _saved = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnoreAttribute]
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
        [JsonProperty]
        public int ColdRange
        {
            get { return _coldRange.IntValue; }
            set
            {
                if (value == _coldRange.IntValue) return;
                _coldRange.IntValue = value;
            }
        }

        [JsonProperty]
        public int TicketChangeLost
        {
            get { return _ticketChangeLost.IntValue; }
            set
            {
                if (value == _ticketChangeLost.IntValue) return;
                _ticketChangeLost.IntValue = value;
            }
        }
        
        [JsonProperty]
        public int LuckyAmount
        {
            get { return _luckyAmount.IntValue; }
            set
            {
                if (value == _luckyAmount.IntValue) return;
                _luckyAmount.IntValue = value;
            }
        }

        [JsonProperty]
        public int TicketChangeWon
        {
            get { return _ticketChangeWon.IntValue; }
            set
            {
                if (value == _ticketChangeWon.IntValue) return;
                _ticketChangeWon.IntValue = value;
            }
        }


        [JsonProperty]
        public int HotRange
        {
            get { return _hotRange.IntValue; }
            set
            {
                if (value == _hotRange.IntValue) return;
                _hotRange.IntValue = value;
            }
        }

        [JsonProperty]
        public int HotNumbers
        {
            get { return _hotNumbers.IntValue; }
            set
            {
                if (value == _hotNumbers.IntValue) return;
                _hotNumbers.IntValue = value;
            }
        }
        [JsonProperty]
        public int ColdNumbers
        {
            get { return _coldNumbers.IntValue; }
            set
            {
                if (value == _coldNumbers.IntValue) return;
                _coldNumbers.IntValue = value;
            }
        }
        [JsonProperty]
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
        [JsonProperty]
        public IList<Ticket> Tickets
        {
            get { return _tickets; }
            set
            {
                if (Equals(value, _tickets)) return;
                _tickets = value;
            }
        }

        [JsonProperty]
        public string Name
        {
            get { return _name; }
            set 
            {
                if (value == _name) return;
                _name = value;
            }
        }

        [JsonProperty]
        public int NumberOfTickets
        {
            get { return _numberOfTickets.IntValue; }
            set
            {
                if (value == _numberOfTickets.IntValue) return;
                _numberOfTickets.IntValue = value;
            }
        }


        [JsonProperty]
        public int Stake
        {
            get { return _stake.IntValue; }
            set
            {
                if (value == _stake.IntValue) return;
                _stake.IntValue = value;
            }
        }
        [JsonProperty]
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

        [JsonIgnoreAttribute]
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
        [JsonProperty]
        public int System
        {
            get { return _system.IntValue; }
            set
            {
                if (value == _system.IntValue) return;
                _system.IntValue = value;
            }
        }

        [JsonProperty]
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
        [JsonIgnoreAttribute]
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
        [JsonProperty]
        public int StatRange
        {
            get { return _statRange.IntValue; }
            set
            {
                if (value == _statRange.IntValue) return;
                _statRange.IntValue = value;
            }
        }
        [JsonIgnoreAttribute]
        public Random Random { get; set; }


        public override string ToString()
        {
            return String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}", NumberOfTickets,  GamesPlayed, Stake, System, ColdNumbers, ColdRange, HotNumbers, HotRange, StatRange, System,TicketChangeLost,TicketChangeWon,LuckyAmount);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
            else
            {

            }
        }

    }

    public class BaseModel
    {
        private IDictionary<string, Property> _properties = new Dictionary<string, Property>();

        public IDictionary<string, Property> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        public Property CreateProperty(string name)
        {
            var property = new Property();
            Properties.Add(name, property);
            return property;
        }

        public Property GetProperty(string name)
        {
            if (Properties.ContainsKey(name))
            {
                return Properties[name];
            }
            return CreateProperty(name);
        }

        
    }
}
