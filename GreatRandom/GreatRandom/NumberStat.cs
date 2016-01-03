using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using GreatRandom.Annotations;

namespace GreatRandom
{
    public class NumberStat : INotifyPropertyChanged
    {
        public int Number { get; set; }
        private IDictionary<int, int> appersDict = new Dictionary<int, int>();

        public int TimesAppear(int fromRange)
        {
            if (appersDict.ContainsKey(fromRange))
                return appersDict[fromRange];
            var appears = 0;
            for (int i = 0; i < fromRange; i++)
            {
                if (history[i])
                    appears++;
            }
            appersDict[fromRange] = appears;
            return appears;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

        }
        IList<bool> history = new List<bool>();
        public void Appear(bool contains)
        {
            history.Insert(0, contains);
            while (history.Count > MainWindow.intStatisticIterations)
            {
                history.RemoveAt(history.Count - 1);
            }
            appersDict.Clear();

        }
    }
}
