using System;
using System.Collections.Generic;

namespace GreatRandom
{
    public class VikingGenerator : INumbersGenerator
    {
        private Random random = new Random();
        private int amount = 6;
        public int Maxnumber { get { return 48; } }
        public bool HaveNext { get { return true; } }

        public bool isLast
        {
            get
            {
                if (index % 10000 == 0)
                {
                    return true;
                }
                return false;
            }
        }

        public int MaxSystem { get { return 6; } }
        public int MinSystem { get { return 6; } }
        public int SelectAmountMax { get { return 7; } }
        public int MaxNumbers { get { return amount; } }

        private int index = 0;
        public void Load()
        {

        }

        public HashSet<int> Generate()
        {
            var numbers = new HashSet<int>();
            int randomNumber = 0;
            while (numbers.Count < amount)
            {
                randomNumber = random.Next(Maxnumber);

                var value = (int)(randomNumber % Maxnumber + 1);
                if (numbers.Contains(value))
                    continue;
                numbers.Add(value);
            }
            index++;
            return numbers;

        }
    }
}
