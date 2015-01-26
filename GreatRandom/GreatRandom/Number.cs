using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using System.Windows.Media;
using GreatRandom.Annotations;

namespace GreatRandom
{
    public class Number : INotifyPropertyChanged
    {
        private byte _number;
        private bool _isChecked = false;

        public Number(byte value)
        {
            Value = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public byte Value
        {
            get { return _number; }
            set
            {
                if (value == _number) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value.Equals(_isChecked)) return;
                _isChecked = value;
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