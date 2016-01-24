using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using GreatRandom.Annotations;
using Newtonsoft.Json;

namespace GreatRandom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        static int SelectAmountMax = 15;
        static int SystemMax = 10;
        static int SystemMin = 2;
        public static int startMoney = 1000;
        private static int MaxStake = 1;
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
        public INumbersGenerator generator = new KenoLoader();
        //        public INumbersGenerator generator = new KenoGenerator();
        private HashSet<string> playerSignatures = new HashSet<string>();
        private string filename = "processedplayers.txt";
        private Random random;
        StreamWriter writer;
        public MainWindow()
        {
            random = new Random();
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
            SavePlayer = new Command<Player>(SaveClick);
            KillCommand = new Command<Player>(KillPlayer);

            stakes.Add(1, 1);
            stakes.Add(2, 2);
            stakes.Add(3, 5);
            stakes.Add(4, 10);
            stakes.Add(5, 15);

        }

        private void KillPlayer(Player obj)
        {
            obj.Money = 0;
        }

        IList<Player> savedplayers = new List<Player>();
        private void SaveClick(Player obj)
        {
            if (obj != null && !savedplayers.Contains(obj))
                savedplayers.Add(obj);
            else if (savedplayers.Contains(obj))
            {
                savedplayers.Remove(obj);
            }
            var stringwriter = Serialize(savedplayers);

            var xml = stringwriter.ToString();
            StreamWriter writer = new StreamWriter("saved.xml");
            writer.Write(xml);
            writer.Flush();
            writer.Close();
        }
        public static string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o); ;
        }

        private Thread gameprocessingthread;
        private Command<Player> _savePlayer;
        private Command<Player> _killCommand;

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            gameprocessingthread.Abort();
            writer.Flush();

            var stringwriter = Serialize(savedplayers);

            var xml = stringwriter.ToString();
            StreamWriter writer2 = new StreamWriter("saved.xml");
            writer2.Write(xml);
            writer2.Flush();
            writer2.Close();
        }

        void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            StreamReader reader = new StreamReader(filename);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                playerSignatures.Add(line);
            }
            reader.Close();
            writer = new StreamWriter(filename, true);

            generator.Load();
            for (int i = 0; i < 100; i++)
            {
                var numbers = generator.Generate();
                foreach (var numberStat in NumbersStatistic)
                {
                    numberStat.Appear(numbers.Contains(numberStat.Number));
                }
            }

            StreamReader sr = new StreamReader("saved.xml");
            var str = sr.ReadToEnd();
            sr.Close();
            savedplayers = Deserialize<List<Player>>(str);
            if (savedplayers == null)
                savedplayers = new List<Player>();
            foreach (var savedplayer in savedplayers)
            {
                savedplayer.Saved = true;
                savedplayer.Money = savedplayer.BiggestMinus;
                if (savedplayer.Money < startMoney)
                    savedplayer.Money = startMoney * 100;
                savedplayer.Random = new Random(random.Next());
                Players.Add(savedplayer);
            }

            LoadStoredPlayers();
            Players.Sort(x => x.Name, ListSortDirection.Ascending);
            for (int i = 0; Players.Count < 100; i++)
            {
                var playernew = CreatePlayer(PlayerCounter++);

                Players.Add(playernew);
            }

            gameprocessingthread = new Thread(ProcessGames);
            gameprocessingthread.Start();

        }
        public static T Deserialize<T>(string xml)
        {
            return JsonConvert.DeserializeObject<T>(xml); ;
        }

        private void LoadStoredPlayers()
        {
            var playernew = new Player() { NumbersAmount = 11, Name = "Loaded1", NumberOfTickets = 11, Stake = 1, SameNumbers = false, Money = startMoney, System = 10, HotNumbers = 6, ColdNumbers = 2, HotRange = 12, ColdRange = 30, StatRange = 7, Random = new Random(random.Next()) };
            Players.Add(playernew);
        }


        public void ProcessGames()
        {
            int counter = PlayerCounter;
            while (generator.HaveNext)
            {
                var start = DateTime.UtcNow;
                var numbers = generator.Generate();


                SortableObservableCollection<Player> lostPlayers = new SortableObservableCollection<Player>();
                //                MyThreadPool<Player>.Foreach(Players, player =>
                foreach (var player in Players)
                {
                    var tempStat = new SortableObservableCollection<NumberStat>();
                    tempStat.Sync(NumbersStatistic);

                    var random = player.Random;
                    if (player.ColdNumbers + player.HotNumbers > 0)
                    {
                        var player1 = player;
                        tempStat.Sort(x => x.TimesAppear(player1.StatRange), ListSortDirection.Descending);
                    }
                    GenerateNumbersBuyTickets(player, tempStat, random);
                    player.CurrentMinus += player.Tickets.Count * player.Stake;
                    var moneyWon = Calculate.CalculateTickets(numbers, player.Tickets);

                    player.Money += moneyWon;
                    if (moneyWon == 0)
                    {
                        player.NumberOfTickets += player.TicketChangeLost;
                        if (player.TicketChangeLost < 0)
                        {
                            while (player.Tickets.Count > player.NumberOfTickets && player.Tickets.Count > 1)
                            {
                                player.Tickets.RemoveAt(player.Tickets.Count - 1);
                            }
                        }
                    }
                    if (moneyWon > 0)
                    {
                        player.NumberOfTickets += player.TicketChangeWon;
                        if (player.TicketChangeWon < 0)
                        {
                            while (player.Tickets.Count > player.NumberOfTickets && player.Tickets.Count > 1)
                            {
                                player.Tickets.RemoveAt(player.Tickets.Count - 1);
                            }
                        }
                    }
                    var currentMinus = moneyWon - player.CurrentMinus;
                    if (currentMinus < 0)
                        player.CurrentMinus = -currentMinus;
                    else
                    {
                        player.CurrentMinus = 0;
                    }
                    

                    //if (player.CurrentMinus > startMoney)
                    //  lostPlayers.Add(player);
                    if (player.Money <= 0)
                        lostPlayers.Add(player);

                    player.NumberOfTickets = player.Tickets.Count;
                    player.GamesPlayed++;
                }
                //                    );

                foreach (var numberStat in NumbersStatistic)
                {
                    numberStat.Appear(numbers.Contains(numberStat.Number));
                }

                foreach (var player in lostPlayers)
                {
                    counter = RemoveAndCreateNewPlayer(player, counter);
                }

                GamesPlayed++;
                if (GamesPlayed % 100 == 0)
                {
                    counter = CreateChildren(counter);
                }
                //if (generator.isLast)
                //{
                //    foreach (var player in Players)
                //    {
                //        player.Money = startMoney;
                //    }
                //}

                while (Players.Count > 2000)
                {
                    Players.RemoveAt(Players.Count - 1);
                }
                PlayerCounter = Players.Count;
                if (GamesPlayed % 20 == 0)
                    if (Dispatcher.Thread.IsAlive)
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

        private int CreateChildren(int counter)
        {
            Players.Sort(x => x.Money, ListSortDirection.Descending);

            var topPlayers = Players.Where(x => x.Money > startMoney).ToList();

            for (int i = 0; i < topPlayers.Count() && i < 20; i++)
            {
                var pl1 = topPlayers[i];
                for (int j = 0; j < topPlayers.Count(); j++)
                {
                    var pl2 = topPlayers[j];
                    var enum1 = pl1.Properties.GetEnumerator();
                    enum1.MoveNext();
                    var enum2 = pl2.Properties.GetEnumerator();
                    enum2.MoveNext();
                    if (i != j)
                    {
                        var child = new Player();
                        child.Random = new Random(random.Next());
                        child.Money = startMoney;
                        child.Name = "c" + counter++;
                        for (int k = 0; k < child.Properties.Count; k++)
                        {
                            var prop1 = enum1.Current;
                            var prop2 = enum2.Current;
                            var usepl2 = random.Next(0, 2) == 1;
                            if (usepl2)
                            {
                                child.GetProperty(prop2.Key).IntValue = prop2.Value.IntValue;
                                child.GetProperty(prop2.Key).BoolValue = prop2.Value.BoolValue;
                                child.GetProperty(prop2.Key).StrValue = prop2.Value.StrValue;
                            }
                            else
                            {
                                child.GetProperty(prop1.Key).IntValue = prop1.Value.IntValue;
                                child.GetProperty(prop1.Key).BoolValue = prop1.Value.BoolValue;
                                child.GetProperty(prop1.Key).StrValue = prop1.Value.StrValue;
                            }
                            enum1.MoveNext();
                            enum2.MoveNext();
                        }

                        child.HotRange = random.Next(child.HotNumbers, generator.Maxnumber / 2 + 1);
                        child.ColdRange = random.Next(child.ColdNumbers, generator.Maxnumber / 2 + 1);
                        child.System = random.Next(1, child.NumbersAmount + 1);
                        Debug.Assert(child.HotRange >= child.HotNumbers);
                        Debug.Assert(child.ColdRange >= child.ColdNumbers);
                        Players.Add(child);
                        if (Players.Count > 1500)
                            break;
                    }
                }
                if (Players.Count > 1500)
                    break;
            }
            for (int index = Players.Count; index < 2000; index++)
            {
                var player = CreatePlayer(counter++);
                Players.Add(player);
            }
            return counter;
        }

        private int RemoveAndCreateNewPlayer(Player player, int counter)
        {
            playerSignatures.Add(player.ToString());

            Players.Remove(player);
            writer.WriteLine(player.ToString());
            savedplayers.Remove(player);

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

        public void GenerateNumbersBuyTickets(Player player, SortableObservableCollection<NumberStat> stats, Random random)
        {
            if (player.System != player.NumbersAmount)
            {
                if (!player.SameNumbers || player.Tickets.Count != player.NumberOfTickets)
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
                            if (!array.Contains(stats.Get(stats.Count - (coldindex + 1)).Number))
                                array.Add(stats.Get(stats.Count - (coldindex + 1)).Number);

                        }
                    }
                    array = GenerateRandomsArray(player.NumbersAmount, array, random);

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
                    while (player.Tickets.Count < player.NumberOfTickets)
                    {
                        var ticket = new Ticket();
                        ticket.Numbers = GenerateRandomsArray(player.System, ticket.Numbers, random);
                        player.Tickets.Add(ticket);
                    }
                }
            }
            else
            {
                if (!player.SameNumbers)
                    player.Tickets.Clear();

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
                            var stat = stats.Get(stats.Count - (coldindex + 1));
                            if (!ticket.Numbers.Contains(stat.Number))
                                ticket.Numbers.Add(stat.Number);

                        }
                    }
                    ticket.Numbers = GenerateRandomsArray(player.System, ticket.Numbers, random);
                    player.Tickets.Add(ticket);
                }
                for (int j = 0; j < player.Tickets.Count; j++)
                {
                    var currentTicket = player.Tickets[j];
                    if (!player.SameNumbers || currentTicket.IsWon)
                    {
                        currentTicket.Numbers = GenerateRandomsArray(player.System, currentTicket.Numbers, random);
                    }
                    currentTicket.Stake = player.Stake;

                }
            }

            //Debug.Assert(player.NumberOfTickets == player.Tickets.Count);
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
                long ticketsAmount = 0;
                var useSystem = random.Next(0, 2) == 1;


                if (useSystem)
                {
                    do
                    {
                        numbersAmount = random.Next(SystemMin, SelectAmountMax + 1);
                        system = random.Next(SystemMin, numbersAmount + 1);
                        while (system > SystemMax)
                        {
                            system = random.Next(1, numbersAmount + 1);
                        }
                        combinations = Combinations(numbersAmount, system);

                    } while (combinations * stake > startMoney);
                    ticketsAmount = combinations;
                }
                else
                {
                    while (ticketsAmount * stake > 100 || ticketsAmount < 1)
                    {
                        ticketsAmount = random.Next(1, 15 + 1);
                        stake = stakes[random.Next(1, MaxStake + 1)];
                    }
                    numbersAmount = random.Next(SystemMin, SystemMax + 1);
                    system = (int)numbersAmount;
                }


                var hotnumbers = 0;
                var coldnumbers = 0;
                var hotRange = 0;
                var coldRange = 0;
                var useColdHot = random.Next(0, 2) == 1;
                var statRange = 0;

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
                    if (hotnumbers > 0)
                        hotRange = random.Next(hotnumbers, generator.Maxnumber / 3);
                    if (coldnumbers > 0)
                        coldRange = random.Next(coldnumbers, generator.Maxnumber / 3);
                    if (hotnumbers > 0 && coldnumbers > 0)
                        statRange = random.Next(1, 100 + 1);

                }
                int changeLost = random.Next((int)-ticketsAmount, (int)ticketsAmount);
                int changeWon = random.Next((int)-ticketsAmount, (int)ticketsAmount);
                Debug.Assert(numbersAmount > 0);
                Debug.Assert(numbersAmount >= system);
                playernew = new Player()
                {
                    NumbersAmount = numbersAmount,
                    Name = counter++.ToString(),
                    NumberOfTickets = (int)ticketsAmount,
                    Stake = stake,
                    SameNumbers = random.Next(0, 2) == 1,
                    Money = startMoney,
                    System = system,
                    HotNumbers = hotnumbers,
                    ColdNumbers = coldnumbers,
                    HotRange = hotRange,
                    ColdRange = coldRange,
                    StatRange = statRange,
                    TicketChangeLost = changeLost,
                    TicketChangeWon = changeWon,
                    Random = new Random(random.Next())
                };

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
        public HashSet<int> GenerateRandomsArray(int numbers, HashSet<int> array, Random random)
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

        public Command<Player> KillCommand
        {
            get { return _killCommand; }
            set
            {
                if (Equals(value, _killCommand)) return;
                _killCommand = value;
                OnPropertyChanged();
            }
        }

        public Command<Player> SavePlayer
        {
            get { return _savePlayer; }
            set
            {
                if (Equals(value, _savePlayer)) return;
                _savePlayer = value;
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
