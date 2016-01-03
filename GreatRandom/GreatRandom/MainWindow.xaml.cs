using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GreatRandom.Annotations;

namespace GreatRandom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        static int SelectAmountMax = 15;
        static int SystemMax = 10;
        static int SystemMin = 1;
        private static int startMoney = 1000;
        private static int MaxStake = 1;
        private static Random random = new Random();
        private Dictionary<int, int> stakes = new Dictionary<int, int>();
        private int _numberOfGames = 0;
        private readonly Calculate _calculate;
        private SortableObservableCollection<Player> _players = new SortableObservableCollection<Player>();
        private int _gamesPlayed;
        private int _playerCounter = 1;
        private SortableObservableCollection<Player> _topPlayers = new SortableObservableCollection<Player>();
        private IDictionary<int, NumberStat> numberStatDict = new Dictionary<int, NumberStat>();
        private SortableObservableCollection<NumberStat> _numbersStatistic = new SortableObservableCollection<NumberStat>();
        public static int intStatisticIterations = 100;
        private string _statisticIterations = "100";
        private SortableObservableCollection<Player> _dPlayers = new SortableObservableCollection<Player>();
        private SortableObservableCollection<Player> _dTopPlayers = new SortableObservableCollection<Player>();
        private bool haveChanges = true;
        public INumbersGenerator generator = new KenoGenerator();
        private HashSet<string> playerSignatures = new HashSet<string>(); 
        public MainWindow()
        {
            _calculate = new Calculate();
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
            for (int i = 1; i <= generator.Maxnumber; i++)
            {
                var numberstat = new NumberStat() { Number = i };
                numberStatDict[i] = numberstat;
                _numbersStatistic.Add(numberstat);
            }

            stakes.Add(1, 1);
            stakes.Add(2, 2);
            stakes.Add(3, 5);
            stakes.Add(4, 10);
            stakes.Add(5, 15);

        }

        private Thread gameprocessingthread;
        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            gameprocessingthread.Abort();
        }

        void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            generator.Load();
            for (int i = 0; i < 100; i++)
            {
                var numbers = generator.Generate();
                foreach (var numberStat in NumbersStatistic)
                {
                    numberStat.Appear(numbers.Contains(numberStat.Number));
                }
            }
            LoadStoredPlayers();
            for (int i = 0; Players.Count < 100; i++)
            {
                var playernew = CreatePlayer(PlayerCounter++);

                Players.Add(playernew);


            }
            gameprocessingthread = new Thread(ProcessGames);
            gameprocessingthread.Start();

        }

        private void LoadStoredPlayers()
        {
            var playernew = new Player(8, "Loaded1", 28, 10, false, startMoney, 6, 7, 0, 11, 21, 1);
            Players.Add(playernew);
            playernew = new Player(8, "Loaded2", 28, 5, false, startMoney, 6, 7, 0, 11, 21, 1);
            Players.Add(playernew);
            playernew = new Player(8, "Loaded3", 28, 2, false, startMoney, 6, 7, 0, 11, 21, 1);
            Players.Add(playernew);
            playernew = new Player(8, "Loaded4", 28, 1, false, startMoney, 6, 7, 0, 11, 21, 1);
            Players.Add(playernew);
            playernew = new Player(10, "Loaded4", 28, 1, false, startMoney, 9, 10, 0, 10, 0, 100);
            Players.Add(playernew);
        }


        public void ProcessGames()
        {
            int counter = PlayerCounter;
            while (generator.HaveNext)
            {
                var start = DateTime.UtcNow;
                var numbers = generator.Generate();


                SortableObservableCollection<Player> childs = new SortableObservableCollection<Player>();
                SortableObservableCollection<Player> lostPlayers = new SortableObservableCollection<Player>();
                                                MyThreadPool<Player>.Foreach(Players, player =>
//                foreach (var player in Players)
                {
                    var tempStat = new SortableObservableCollection<NumberStat>();
                    if (player.ColdNumbers + player.HotNumbers > 0)
                    {
                        tempStat.Sync(NumbersStatistic);
                        var player1 = player;
                        tempStat.Sort(x => x.TimesAppear(player1.StatRange), ListSortDirection.Descending);
                    }
                    var random = new Random();
                    GenerateNumbersBuyTickets(player, tempStat,random);
                    player.CurrentMinus += player.Tickets.Count * player.Stake;
                    var moneyWon = Calculate.CalculateTickets(numbers, player.Tickets);
                    //if (moneyWon > 1000)
                    //{
                    //    var childsAmount = moneyWon / 10000;
                    //    for (int j = 0; j < childsAmount && j < 10; j++)
                    //    {
                    //        var child = new Player(player.NumbersAmount, (counter++).ToString(),
                    //            player.NumberOfTickets, player.Stake, player.SameNumbers, 1000, player.System,
                    //            player.HotNumbers, player.ColdNumbers, player.HotRange, player.ColdRange,
                    //            player.StatRange);
                    //        childs.Add(child);

                    //    }
                    //}
                    player.Money += moneyWon;
                    var currentMinus = moneyWon - player.CurrentMinus;
                    if (currentMinus < 0)
                        player.CurrentMinus = -currentMinus;
                    else
                    {
                        player.CurrentMinus = 0;
                    }
                    if (player.CurrentMinus > startMoney)
                        lostPlayers.Add(player);
//                    if (player.Money <= 0)
//                      lostPlayers.Add(player);

                    player.GamesPlayed++;
//                }
                                });


                foreach (var numberStat in NumbersStatistic)
                {
                    numberStat.Appear(numbers.Contains(numberStat.Number));
                }

                foreach (var player in lostPlayers)
                {
                    counter = RemoveAndCreateNewPlayer(player, counter);
                }

                foreach (var child in childs)
                {
                    Players.Add(child);
                }
                for (int index = Players.Count; index < 2000; index++)
                {
                    var player = CreatePlayer(counter++);
                    Players.Add(player);
                }

                while (Players.Count > 2000)
                {
                    Players.RemoveAt(Players.Count - 1);
                }

                PlayerCounter = Players.Count;
                GamesPlayed++;
                Dispatcher.Invoke(() =>
                {
                    DPlayers.Sync(Players);
                    DTopPlayers.Sync(TopPlayers);
                });

                var end = DateTime.UtcNow;
                Console.WriteLine(end - start);

                //haveChanges = true;
            }
            Players.Sort(x => x.Money, ListSortDirection.Descending);
            Dispatcher.Invoke(() =>
            {
                DPlayers.Sync(Players);
                DTopPlayers.Sync(TopPlayers);
            });


        }
        private int RemoveAndCreateNewPlayer(Player player, int counter)
        {
            Players.Remove(player);
            var minSpendMoney = 0;
            var lastPlayer = TopPlayers.LastOrDefault();
            if (lastPlayer != null)
            {
                minSpendMoney = lastPlayer.SpendMoney;
            }
            if (player.SpendMoney > minSpendMoney)
            {
                TopPlayers.Add(player);

            }
            player.SpendMoney += player.Money;

            TopPlayers.Sort(x => x.SpendMoney, ListSortDirection.Descending);
            while (TopPlayers.Count > 10)
            {
                TopPlayers.RemoveAt(TopPlayers.Count - 1);
            }



            return counter;

        }

        public void GenerateNumbersBuyTickets(Player player, SortableObservableCollection<NumberStat> stats,Random random)
        {
            if (player.System != player.NumbersAmount)
            {
                if (!player.SameNumbers || player.Tickets.Count == 0)
                {

                    var array = new HashSet<int>();
                    if (player.HotNumbers > 0)
                    {
                        while (array.Count < player.HotNumbers)
                        {
                            var hotindex = random.Next(0, player.HotRange);
                            if (!array.Contains(stats[hotindex].Number))
                                array.Add(stats[hotindex].Number);
                        }
                    }
                    if (player.ColdNumbers > 0)
                    {
                        while (array.Count < player.ColdNumbers + player.HotNumbers)
                        {
                            var coldindex = random.Next(0, player.ColdRange);
                            if (!array.Contains(stats[stats.Count - (coldindex + 1)].Number))
                                array.Add(stats[stats.Count - (coldindex + 1)].Number);

                        }
                    }
                    array = GenerateRandomsArray(player.NumbersAmount, array,random);

                    var combinations = combination(array.ToArray(), player.System).ToArray();
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
                    if (player.HotNumbers > 0)
                    {
                        while (ticket.Numbers.Count < player.HotNumbers)
                        {
                            var hotindex = random.Next(0, player.HotRange);
                            if (!ticket.Numbers.Contains(stats[hotindex].Number))
                                ticket.Numbers.Add(stats[hotindex].Number);
                        }
                    }
                    if (player.ColdNumbers > 0)
                    {
                        while (ticket.Numbers.Count < player.ColdNumbers + player.HotNumbers)
                        {
                            var coldindex = random.Next(0, player.ColdRange);
                            if (!ticket.Numbers.Contains(stats[stats.Count - (coldindex + 1)].Number))
                                ticket.Numbers.Add(stats[stats.Count - (coldindex + 1)].Number);

                        }
                    }
                    ticket.Numbers = GenerateRandomsArray(player.System, ticket.Numbers,random);
                    player.Tickets.Add(ticket);
                }
                for (int j = 0; j < player.Tickets.Count; j++)
                {
                    var currentTicket = player.Tickets[j];
                    if (!player.SameNumbers || currentTicket.IsWon)
                    {
                        currentTicket.Numbers = GenerateRandomsArray(player.System, currentTicket.Numbers,random);
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



        public Player CreatePlayer(int counter)
        {
            Player playernew;
            do
            {
                var numbersAmount = 0;
                var system = 0;
                long combinations = 0;
                var stake = stakes[random.Next(1, MaxStake + 1)];
                var statRange = random.Next(1, 100 + 1);
                long ticketsAmount = 0;
                var useSystem = random.Next(0, 2) == 1;


                if (useSystem)
                {
                    do
                    {
                        numbersAmount = random.Next(SystemMin, SelectAmountMax + 1);
                        system = random.Next(1, numbersAmount + 1);
                        while (system > SystemMax)
                        {
                            system = random.Next(1, numbersAmount + 1);
                        }
                        combinations = Combinations(numbersAmount, system);

                    } while (combinations*stake > startMoney || combinations > 50 || numbersAmount == system);
                    ticketsAmount = combinations;
                }
                else
                {
                    while (ticketsAmount*stake > 100 || ticketsAmount < 10)
                    {
                        ticketsAmount = random.Next(1, 50 + 1);
                        stake = stakes[random.Next(1, MaxStake + 1)];
                    }
                    numbersAmount = random.Next(SystemMin, SystemMax + 1);
                    system = (int) numbersAmount;
                }


                var hotnumbers = 0;
                var coldnumbers = 0;
                var hotRange = 0;
                var coldRange = 0;
                var useColdHot = random.Next(0, 2) == 1;
                if (useColdHot)
                {
                    var hotFirst = random.Next(0, 2) == 1;
                    if (hotFirst)
                    {
                        hotnumbers = random.Next(0, numbersAmount + 1);
                        coldnumbers = random.Next(0, numbersAmount - hotnumbers + 1);
                    }
                    else
                    {
                        coldnumbers = random.Next(0, numbersAmount + 1);

                        hotnumbers = random.Next(0, numbersAmount - coldnumbers + 1);

                    }
                    hotRange = random.Next(hotnumbers, generator.Maxnumber/2 + 1);
                    coldRange = random.Next(coldnumbers, generator.Maxnumber/2 + 1);


                }
                Debug.Assert(numbersAmount > 0);
                Debug.Assert(numbersAmount >= system);
                playernew = new Player(numbersAmount, counter++.ToString(), (int) ticketsAmount, stake,
                    random.Next(0, 2) == 1, startMoney, system, hotnumbers, coldnumbers, hotRange, coldRange, statRange);

            } while (playerSignatures.Contains(playernew.ToString()));
            playerSignatures.Add(playernew.ToString());
            return playernew;

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
        public HashSet<int> GenerateRandomsArray(int numbers, HashSet<int> array, Random random1)
        {
            if (array == null)
                array = new HashSet<int>();
            while (array.Count < numbers)
            {
                var rnd = random.Next(1, generator.Maxnumber + 1);
                if (array.Contains(rnd))
                    continue;
                array.Add((int)rnd);
            }
            return array;
        }

        public SortableObservableCollection<NumberStat> NumbersStatistic
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

        public SortableObservableCollection<Player> DTopPlayers
        {
            get { return _dTopPlayers; }
            set
            {
                if (Equals(value, _dTopPlayers)) return;
                _dTopPlayers = value;
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

        public SortableObservableCollection<Player> DPlayers
        {
            get { return _dPlayers; }
            set
            {
                if (Equals(value, _dPlayers)) return;
                _dPlayers = value;
                OnPropertyChanged();
            }
        }

        public SortableObservableCollection<Player> Players
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


        public static IList<int[]> combination(int[] elements, int K)
        {
            IList<int[]> result = new List<int[]>();

            // get the length of the array
            // e.g. for {'A','B','C','D'} => N = 4 
            int N = elements.Length;

            if (K > N)
            {
                return null;
            }
            // calculate the possible combinations
            // e.g. c(4,2)
            //c(N,K);

            // get the combination by index 
            // e.g. 01 --> AB , 23 --> CD
            int[] combination = new int[K];

            // position of current index
            //  if (r = 1)              r*
            //  index ==>        0   |   1   |   2
            //  element ==>      A   |   B   |   C
            int r = 0;
            int index = 0;

            while (r >= 0)
            {
                // possible indexes for 1st position "r=0" are "0,1,2" --> "A,B,C"
                // possible indexes for 2nd position "r=1" are "1,2,3" --> "B,C,D"

                // for r = 0 ==> index < (4+ (0 - 2)) = 2
                if (index <= (N + (r - K)))
                {
                    combination[r] = index;

                    // if we are at the last position print and increase the index
                    if (r == K - 1)
                    {

                        //do something with the combination e.g. add to list or print
                        var array = new int[combination.Length];
                        for (int i = 0; i < combination.Length; i++)
                        {
                            array[i] = elements[combination[i]];
                        }
                        result.Add(array);
                        //print(combination, elements);
                        index++;
                    }
                    else
                    {
                        // select index for next position
                        index = (int)(combination[r] + 1);
                        r++;
                    }
                }
                else
                {
                    r--;
                    if (r > 0)
                        index = (int)(combination[r] + 1);
                    else
                        index = (int)(combination[0] + 1);
                }
            }
            return result;
        }
    }
}
