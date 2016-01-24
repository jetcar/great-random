using System.Collections.Generic;
using Newtonsoft.Json;

namespace GreatRandom
{
    public class Ticket
    {
        private HashSet<int> _numbers;
        private int _stake = 1;
        private int _wonAmount;
        public int wonNumbers;
        public int wonAmount;
        public Ticket()
        {
            _numbers = new HashSet<int>();
        }

        public HashSet<int> Numbers
        {
            get { return _numbers; }
            set { _numbers = value; }
        }

       [JsonIgnore]
        public int WonAmount
        {
            get { return _wonAmount; }
            set
            {
                if (value.Equals(_wonAmount)) return;
                _wonAmount = value;
            }
        }

       [JsonIgnoreAttribute]
        public int Stake
        {
            get { return _stake; }
            set
            {
                if (value.Equals(_stake)) return;
                _stake = value;
            }
        }
        [JsonIgnoreAttribute]
        public bool IsWon { get; set; }

    }
}