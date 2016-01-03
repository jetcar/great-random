using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfApplication1;

namespace GreatRandom
{
    public interface INumbersGenerator
    {
        HashSet<int> Generate();
        int Maxnumber { get; }
        bool HaveNext { get; }
        void Load();
    }
}