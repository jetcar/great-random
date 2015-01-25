using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private SortableObservableCollection<NumberStatistic> _numbersCollection = new SortableObservableCollection<NumberStatistic>();
        private IDictionary<int,NumberStatistic> _numbersDict = new Dictionary<int, NumberStatistic>();
        private int amount = 100;
        private Thread currentThread;
        private int _currentCount;
        DispatcherTimer timer = new DispatcherTimer(); 
        public MainWindow()
        {
            timer.Interval = new TimeSpan(0,0,0,0,100);
            timer.Tick += timer_Tick;
            for (byte i = 0; i < amount; i++)
            {
                var number = new NumberStatistic() { Number = (byte)(i + 1) };
                NumbersCollection.Add(number);
                _numbersDict.Add(i + 1, number);
            }
            InitializeComponent();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            NumbersCollection.Sort(x => x.Count, ListSortDirection.Descending);
        }

        public SortableObservableCollection<NumberStatistic> NumbersCollection
        {
            get { return _numbersCollection; }
            set { _numbersCollection = value; }
        }

        public static int Count { get; set; }

        public int CurrentCount
        {
            get { return _currentCount; }
            set
            {
                if (value == _currentCount) return;
                _currentCount = value;
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            timer.Start();
            var random = new Random();
           currentThread = new Thread(() =>
           {
               while (true)
               {
                   var value = random.Next(1, 101);
                   Count++;
                   CurrentCount = Count;
                   _numbersDict[value].Count++;
               }
           });
            currentThread.Start();
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            currentThread.Abort();
            timer.Stop();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            currentThread.Abort();
            timer.Stop();
        }

        private void Reset_click(object sender, RoutedEventArgs e)
        {
            if (currentThread != null) currentThread.Abort();
            timer.Stop();
            foreach (var numberStatistic in NumbersCollection)
            {
                numberStatistic.Count = 0;
            }

            Count = 0;
        }
    }
}
