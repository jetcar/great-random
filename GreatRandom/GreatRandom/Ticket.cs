using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Threading;
using GreatRandom.Annotations;

namespace GreatRandom
{
    public class Ticket
    {
        private HashSet<int> _numbers;
        private int _stake = 1;
        private int _wonAmount;

        public Ticket()
        {
            _numbers = new HashSet<int>();
        }

        public HashSet<int> Numbers
        {
            get { return _numbers; }
            set { _numbers = value; }
        }

        public int WonAmount
        {
            get { return _wonAmount; }
            set
            {
                if (value.Equals(_wonAmount)) return;
                _wonAmount = value;
            }
        }

        public int Stake
        {
            get { return _stake; }
            set
            {
                if (value.Equals(_stake)) return;
                _stake = value;
            }
        }

        public bool IsWon { get; set; }

    }
}