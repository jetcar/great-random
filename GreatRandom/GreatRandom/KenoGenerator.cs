using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatRandom
{
    public class KenoGenerator : INumbersGenerator
    {
        private Random random = new Random();
        private int amount = 20;
        public int Maxnumber { get { return 64; } }
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
