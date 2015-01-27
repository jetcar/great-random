using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Threading;
using GreatRandom.Annotations;

namespace GreatRandom
{
    public class Ticket : INotifyPropertyChanged
    {
        private SortableObservableCollection<Number> _numbers = new SortableObservableCollection<Number>();
        private double _amount = 1;
        private double _wonAmount;
        private SortableObservableCollection<Result> _results = new SortableObservableCollection<Result>();
        public event PropertyChangedEventHandler PropertyChanged;

        public SortableObservableCollection<Number> Numbers
        {
            get { return _numbers; }
            set { _numbers = value; }
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

        public double Amount
        {
            get { return _amount; }
            set
            {
                if (value.Equals(_amount)) return;
                _amount = value;
                OnPropertyChanged();
            }
        }

        public SortableObservableCollection<Result> Results
        {
            get { return _results; }
            set { _results = value; }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}