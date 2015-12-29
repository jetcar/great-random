using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using GreatRandom.Annotations;

namespace GreatRandom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public static byte MAXNUMBER = 64;
        private static Random random = new Random();
        private int _numberOfGames = 0;
        private readonly Calculate _calculate;
        private ObservableCollection<Player> _players = new ObservableCollection<Player>();
        private int _gamesPlayed;
        private int _playerCounter = 1;
        private SortableObservableCollection<Player> _topPlayers = new SortableObservableCollection<Player>();
        private IDictionary<int, NumberStat> numberStatDict = new Dictionary<int, NumberStat>();
        private ObservableCollection<NumberStat> _numbersStatistic = new ObservableCollection<NumberStat>();
        private int intStatisticIterations = 100;
        private string _statisticIterations = "100";

        public MainWindow()
        {
            _calculate = new Calculate();
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            for (int i = 1; i <= MAXNUMBER; i++)
            {
                var numberstat = new NumberStat() { Number = i };
                numberStatDict[i] = numberstat;
                _numbersStatistic.Add(numberstat);
            }

        }

        void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                var playernew = CreatePlayer(PlayerCounter++);
                Dispatcher.Invoke(() =>
                {
                    Players.Add(playernew);

                });
            }
            var thread = new Thread(ProcessGames);
            thread.Start();

        }



        public void ProcessGames()
        {
            int counter = PlayerCounter;
            while (true)
            {
                var numbers = new HashSet<byte>();
                int randomNumber = 0;
                while (numbers.Count < 20)
                {
                    randomNumber = random.Next(MAXNUMBER);

                    var value = (byte)(randomNumber % MAXNUMBER + 1);
                    if (numbers.Contains(value))
                        continue;
                    numbers.Add(value);
                }
                foreach (var number in numbers)
                {
                    numberStatDict[number].TimesAppear++;
                }

                for (int i = 0; i < Players.Count; i++)
                {
                    var player = Players[i];
                    GenerateNumbersBuyTickets(player);
                    var moneyWon = Calculate.CalculateTickets(numbers, player.Tickets);
                    if (moneyWon > 1000)
                    {
                        var childsAmount = moneyWon / 1000;
                        for (int j = 0; j < childsAmount; j++)
                        {
                            var child = new Player(player.NumbersAmount, (counter++).ToString(), player.NumberOfTickets, player.Stake, player.SameNumbers, 1000, player.System);
                            Dispatcher.Invoke(() =>
                            {
                                Players.Add(child);
                            });
                        }
                    }
                    player.Money += moneyWon;
                    counter = RenewPlayer(player, counter);
                    player.GamesPlayed++;
                }

                while (Players.Count < 100)
                {
                    var player = CreatePlayer(counter++);
                    Dispatcher.Invoke(() =>
                    {
                        Players.Add(player);
                    });
                }

                PlayerCounter = Players.Count;
                GamesPlayed++;
            }
        }

        private int RenewPlayer(Player player, int counter)
        {
            if (player.Money < 0)
            {
                Dispatcher.Invoke(() => { Players.Remove(player); });
                var minSpendMoney = 0;
                var lastPlayer = TopPlayers.LastOrDefault();
                if (lastPlayer != null)
                {
                    minSpendMoney = lastPlayer.SpendMoney;
                }
                if (player.SpendMoney > minSpendMoney)
                {
                    Dispatcher.Invoke(() => { TopPlayers.Add(player); });

                }
                Dispatcher.Invoke(() =>
                {
                    TopPlayers.Sort(x => x.SpendMoney, ListSortDirection.Descending);
                    while (TopPlayers.Count > 10)
                    {
                        TopPlayers.RemoveAt(TopPlayers.Count - 1);
                    }
                });


            }
            return counter;
        }

        public static void GenerateNumbersBuyTickets(Player player)
        {
            if (player.System != player.NumbersAmount)
            {
                if (!player.SameNumbers || player.Tickets.Count == 0)
                {
                    var array = GenerateRandomsArray(player.NumbersAmount).ToArray();

                    var combinations = GenerateAllPermutations(array, player.System).ToArray();
                    player.Tickets.Clear();
                    ;
                    foreach (var comb in combinations)
                    {
                        var currentTicket = new Ticket();
                        for (int index = 0; index < comb.Length; index++)
                        {
                            currentTicket.Numbers.Add(comb[index]);
                        }
                        currentTicket.Stake = player.Stake;
                        player.Tickets.Add(currentTicket);
                    }
                }
            }
            else
            {
                while (player.Tickets.Count < player.NumberOfTickets)
                {
                    var ticket = new Ticket();
                    ticket.Numbers = GenerateRandomsArray(player.System);
                    player.Tickets.Add(ticket);
                }
                for (int j = 0; j < player.Tickets.Count; j++)
                {
                    var currentTicket = player.Tickets[j];
                    if (!player.SameNumbers || currentTicket.IsWon)
                    {
                        currentTicket.Numbers = GenerateRandomsArray(player.System);
                    }
                    currentTicket.Stake = player.Stake;

                }
            }

            foreach (var ticket in player.Tickets)
            {
                player.Money -= ticket.Stake;
                player.SpendMoney += ticket.Stake;
                if (player.Money < 0)
                    ticket.Stake = 0;
            }
        }

        public static long Combinations(int n, int k)
        {
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }

        public static long Factorial(int n)
        {
            long result = 1;
            for (int i = n; i > 0; i--)
            {
                result *= i;
            }
            return result;
        }

        public Player CreatePlayer(int counter)
        {
            var numbersAmount = 0;
            var system = 0;
            long combinations = 0;
            var stake = random.Next(1, 15 + 1);
            var Money = 1000;
            long ticketsAmount = 0;
            while (ticketsAmount < 10)
            {
                do
                {
                    numbersAmount = random.Next(1, 15 + 1);
                    system = random.Next(1, numbersAmount + 1);
                    while (system > 10)
                    {
                        system = random.Next(1, numbersAmount + 1);
                    }
                    combinations = Combinations(numbersAmount, system);

                } while (combinations*stake > Money || combinations > 50);
                ticketsAmount = combinations;
                while (ticketsAmount*stake > Money || ticketsAmount == 0)
                {
                    ticketsAmount = random.Next(1, Money/stake + 1);
                }
            }
            var playernew = new Player(numbersAmount, counter++.ToString(), (int)ticketsAmount, stake, random.Next(0, 2) == 1, Money, system);


            return playernew;
        }

        public static HashSet<byte> GenerateRandomsArray(int numbers)
        {
            var array = new HashSet<byte>();
            int k = 0;
            while (k < numbers)
            {
                var rnd = random.Next(1, MAXNUMBER + 1);
                if (array.Any(x => x == rnd))
                    continue;
                array.Add((byte)rnd);
                k++;
            }
            return array;
        }

        public ObservableCollection<NumberStat> NumbersStatistic
        {
            get { return _numbersStatistic; }
            set
            {
                if (Equals(value, _numbersStatistic)) return;
                _numbersStatistic = value;
                OnPropertyChanged();
            }
        }

        public int PlayerCounter
        {
            get { return _playerCounter; }
            set
            {
                if (value == _playerCounter) return;
                _playerCounter = value;
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

        public SortableObservableCollection<Player> TopPlayers
        {
            get { return _topPlayers; }
            set
            {
                if (Equals(value, _topPlayers)) return;
                _topPlayers = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Player> Players
        {
            get { return _players; }
            set
            {
                if (Equals(value, _players)) return;
                _players = value;
                OnPropertyChanged();
            }
        }

        public string StatisticIterations
        {
            get { return _statisticIterations; }
            set
            {
                if (value == _statisticIterations) return;
                _statisticIterations = value;
                OnPropertyChanged();
            }
        }

        public Calculate Calculate
        {
            get { return _calculate; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            intStatisticIterations = Convert.ToInt32(StatisticIterations);
        }
    }
}
