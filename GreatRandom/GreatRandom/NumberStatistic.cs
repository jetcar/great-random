using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using System.Windows.Media;
using GreatRandom.Annotations;

namespace GreatRandom
{
    public class NumberStatistic : INotifyPropertyChanged
    {
        private byte _number;
        private double _count;
        private double _barWidth = 0;
        private double _maxWidth = 200;

        public event PropertyChangedEventHandler PropertyChanged;

        public double MaxWidth
        {
            get { return _maxWidth; }
            set
            {
                if (value.Equals(_maxWidth)) return;
                _maxWidth = value;
                OnPropertyChanged();
            }
        }

        public byte Number
        {
            get { return _number; }
            set
            {
                if (value == _number) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        public double Count
        {
            get { return _count; }
            set
            {
                if (value == _count) return;
                _count = value;
                OnPropertyChanged();
                BarWidth = value / MainWindow.Count * MaxWidth;
            }
        }

        public Brush Color
        {
            get
            {
                var color = new Color() { R = Number, G = Number, B = Number, A = 255 };
                return new SolidColorBrush(color);
            }
        }

        public double BarWidth
        {
            get { return _barWidth; }
            set
            {
                if (value.Equals(_barWidth)) return;
                _barWidth = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}