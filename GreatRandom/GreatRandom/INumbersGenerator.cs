using System.Collections.Generic;

namespace GreatRandom
{
    public interface INumbersGenerator
    {
        HashSet<int> Generate();
        int Maxnumber { get; }
        bool HaveNext { get; }
        bool isLast { get;  }
        void Load();
    }
}