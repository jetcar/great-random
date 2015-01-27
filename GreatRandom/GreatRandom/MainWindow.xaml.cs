using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using GreatRandom.Annotations;

namespace GreatRandom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private SortableObservableCollection<Number> _numbersCollection = new SortableObservableCollection<Number>();
        private IDictionary<int, Number> _numbersDict = new Dictionary<int, Number>();
        private byte amount = 64;
        private SortableObservableCollection<Ticket> _myTickets = new SortableObservableCollection<Ticket>();

        public MainWindow()
        {
            for (byte i = 0; i < amount; i++)
            {
                var number = new Number((byte)(i + 1));
                Numbers.Add(number);
                _numbersDict.Add(i + 1, number);
            }
            InitializeComponent();
        }


        public SortableObservableCollection<Number> Numbers
        {
            get { return _numbersCollection; }
            set { _numbersCollection = value; }
        }



        public int NumberOfGames
        {
            get { return _numberOfGames; }
            set
            {
                if (value == _numberOfGames) return;
                _numberOfGames = value;
                OnPropertyChanged();
            }
        }

        public double SpendMoney
        {
            get { return _spendMoney; }
            set
            {
                if (value.Equals(_spendMoney)) return;
                _spendMoney = value;
                OnPropertyChanged();
            }
        }

        public double WonAmount
        {
            get { return _wonAmount; }
            set
            {
                if (value.Equals(_wonAmount)) return;
                _wonAmount = value;
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

        public SortableObservableCollection<Ticket> Results
        {
            get { return _results; }
            set { _results = value; }
        }

        public SortableObservableCollection<Ticket> MyTickets
        {
            get { return _myTickets; }
            set { _myTickets = value; }
        }

        public Calculate Calculate
        {
            get { return _calculate; }
        }

        public double TicketStake
        {
            get { return _ticketStake; }
            set
            {
                if (value.Equals(_ticketStake)) return;
                _ticketStake = value;
                OnPropertyChanged();
            }
        }

        public int TicketNumbers
        {
            get { return _ticketNumbers; }
            set
            {
                if (value == _ticketNumbers) return;
                _ticketNumbers = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        private void StartClick(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                byte[] randomNumber = new byte[1];
                WonAmount = 0;
                foreach (var myTicket in MyTickets)
                {
                    SpendMoney += NumberOfGames * myTicket.Amount;
                }
                var count = 0;
                while (count < NumberOfGames)
                {
                    var numbers = new HashSet<byte>();
                    while (numbers.Count < 20)
                    {
                        rngCsp.GetBytes(randomNumber);

                        byte value = (byte)(randomNumber[0] % amount + 1);
                        if (numbers.Contains(value))
                            continue;
                        numbers.Add(value);
                    }
                    var result = new Ticket();
                    foreach (var number in numbers)
                    {
                        result.Numbers.Add(new Number(number));
                    }
                    Calculate.CalculateTickets(numbers, MyTickets);
                    result.Numbers.Sort(x => x.Value, ListSortDirection.Ascending);
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        Results.Add(result);
                    });
                    count++;
                }

                foreach (var myTicket in MyTickets)
                {
                    WonAmount += myTicket.WonAmount;
                }
            }).Start();

        }


        private void Reset_click(object sender, RoutedEventArgs e)
        {
            foreach (var numberStatistic in Numbers)
            {
                numberStatistic.IsChecked = true;
            }
            SpendMoney = 0;
            Results.Clear();
            foreach (var number in Numbers)
            {
                number.IsChecked = false;
            }
            MyTickets.Clear();
            currentTicket = new Ticket();
            WonAmount = 0;
            GamesPlayed = 0;
            NumberOfGames = 1;
        }

        Ticket currentTicket = new Ticket();
        private SortableObservableCollection<Ticket> _results = new SortableObservableCollection<Ticket>();
        private double _spendMoney;
        private double _wonAmount;
        private int _gamesPlayed;
        private int _numberOfGames = 1;
        private readonly Calculate _calculate;
        private double _ticketStake = 1;
        private int _ticketNumbers = 10;

        private void SelectNumber(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            var number = toggleButton.DataContext as Number;
            if (number.IsChecked)
            {
                currentTicket.Numbers.Add(new Number(number.Value));
            }
            else
            {
                currentTicket.Numbers.Remove(new Number(number.Value));
            }
            currentTicket.Numbers.Sort(x => x.Value, ListSortDirection.Ascending);

        }

        private void AddTicket(object sender, RoutedEventArgs e)
        {
            if (currentTicket.Numbers.Count == 0)
            {
                var random = new Random();
                while (currentTicket.Numbers.Count < TicketNumbers)
                {
                    var rnd = random.Next(1, amount + 1);
                    if (currentTicket.Numbers.Any(x => x.Value == rnd))
                        continue;
                    currentTicket.Numbers.Add(new Number((byte)rnd));
                }
            }


            currentTicket.Amount = TicketStake;
            MyTickets.Add(currentTicket);
            foreach (var number in Numbers)
            {
                number.IsChecked = false;
            }
            currentTicket = new Ticket();
            TicketStake = 1;
        }

        private void numberOfGamesClick(object sender, RoutedEventArgs e)
        {
            NumberOfGames = Convert.ToInt32(((Button)sender).Content);
        }

        private void Remove_Ticket(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var ticket = button.DataContext as Ticket;
            MyTickets.Remove(ticket);
        }
    }
}
