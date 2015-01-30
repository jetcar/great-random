﻿using System;
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
        public static Dispatcher DispatcherThread;
        public DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            for (byte i = 0; i < amount; i++)
            {
                var number = new Number((byte)(i + 1));
                Numbers.Add(number);
                _numbersDict.Add(i + 1, number);
            }
            timer.Interval = new TimeSpan(0,0,0,0,100);
            timer.Tick += timer_Tick;
            InitializeComponent();
            DispatcherThread = Dispatcher.CurrentDispatcher;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Results.NotifyAll();
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
            set
            {
                _results = value;
                OnPropertyChanged();
            }
        }

        public SortableObservableCollection<Ticket> MyTickets
        {
            get { return _myTickets; }
            set
            {
                _myTickets = value;
                OnPropertyChanged();
            }
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

        public TimeSpan SpendTime
        {
            get { return _spendTime; }
            set
            {
                if (value.Equals(_spendTime)) return;
                _spendTime = value;
                OnPropertyChanged();
            }
        }

        public bool IsSystem
        {
            get { return _isSystem; }
            set
            {
                if (value.Equals(_isSystem)) return;
                _isSystem = value;
                OnPropertyChanged();
            }
        }

        public int ResultsCount
        {
            get { return _resultsCount; }
            set
            {
                if (value == _resultsCount) return;
                _resultsCount = value;
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
            timer.Start();
            new Thread(() =>
            {
                var startDate = DateTime.Now;

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

                        var value = (byte)(randomNumber[0] % amount + 1);
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
                    DispatcherThread.Invoke(() =>
                    {
                        result.Numbers.NotifyAll();
                        Results.Insert(0, result);
                    });
                    ResultsCount = Results.Count;
                    count++;

                }

                foreach (var myTicket in MyTickets)
                {
                    WonAmount += myTicket.WonAmount;
                }
                DispatcherThread.Invoke(() =>
                {
                    Results.NotifyAll();
                });
                timer.Stop();
                SpendTime = DateTime.Now - startDate;

            }).Start();

        }


        private void Reset_click(object sender, RoutedEventArgs e)
        {
            foreach (var numberStatistic in Numbers)
            {
                numberStatistic.IsChecked = true;
            }
            SpendMoney = 0;
            Results = new SortableObservableCollection<Ticket>();
            foreach (var number in Numbers)
            {
                number.IsChecked = false;
            }
            MyTickets = new SortableObservableCollection<Ticket>();
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
        private TimeSpan _spendTime;
        private bool _isSystem;
        private int _resultsCount;

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
                currentTicket.Numbers.Remove(currentTicket.Numbers.First(x => x.Value == number.Value));
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
                    currentTicket.Numbers.Sort(x => x.Value, ListSortDirection.Ascending);

                }
            }
            if (IsSystem)
            {
                var array = Numbers.Where(x => x.IsChecked).Select(x => x.Value).ToArray();
                currentTicket.Numbers.Clear();
                var combinations = GenerateAllPermutations(array, TicketNumbers);
                foreach (var comb in combinations)
                {
                    foreach (var number in comb)
                    {
                        currentTicket.Numbers.Add(new Number(number));
                    }
                    currentTicket.Amount = TicketStake;
                    MyTickets.Add(currentTicket);
                    currentTicket = new Ticket();
                }

            }


            else
            {
                currentTicket.Amount = TicketStake;
                MyTickets.Add(currentTicket);

            }
            foreach (var number in Numbers)
            {
                number.IsChecked = false;
            }
            currentTicket = new Ticket();
            TicketStake = 1;
        }

        public static IEnumerable<T[]> GenerateAllPermutations<T>(T[] source, int count) where T : IComparable
        {
            return GetKCombs(source, count).Select(x => x.ToArray());
        }
        static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
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
