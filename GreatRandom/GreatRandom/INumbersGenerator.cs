using System.Collections.Generic;

namespace GreatRandom
{
    public interface INumbersGenerator
    {
        HashSet<int> Generate();
        int Maxnumber { get; }
        bool HaveNext { get; }
        bool isLast { get;  }
        int MaxSystem { get; }
        int MinSystem { get; }
        int SelectAmountMax { get; }
        int MaxNumbers { get; }
        void Load();
    }
}