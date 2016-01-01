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
        private double _timesAppear;
        public int Number { get; set; }

        public double TimesAppear
        {
            get { return _timesAppear; }
            set
            {
                if (value == _timesAppear) return;
                _timesAppear = value;
                OnPropertyChanged();
            }
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
            history.Insert(0,contains);
            while (history.Count > MainWindow.intStatisticIterations)
            {
                history.RemoveAt(history.Count-1);
            }
            TimesAppear = history.Count(x => x);

        }
    }
}
