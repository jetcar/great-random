using System.Collections.Generic;
using System.Windows.Media;

namespace GreatRandom
{
    public class Calculate
    {

        public static void CalculateTickets(HashSet<byte> numbers, SortableObservableCollection<Ticket> myTickets)
        {
            foreach (var myTicket in myTickets)
            {
                var wonNumbersAmount = 0;
                foreach (var number in myTicket.Numbers)
                {
                    if (numbers.Contains(number.Value))
                    {
                        wonNumbersAmount++;
                        number.IsChecked = true;
                    }
                }
                double wonAmount = 0;
                switch (myTicket.Numbers.Count)
                {
                    case 1:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                break;
                            case 1:
                                break;
                        }
                        break;
                    case 2:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                break;
                            case 1:
                                break;
                            case 2:
                                wonAmount = myTicket.Amount * 5;
                                break;
                        }

                        break;
                    case 3:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                break;
                            case 1:
                                break;
                            case 2:
                                wonAmount = myTicket.Amount * 1;
                                break;
                            case 3:
                                wonAmount = myTicket.Amount * 11;
                                break;
                        }
                        break;
                    case 4:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Amount * 1;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                wonAmount = myTicket.Amount * 2;
                                break;
                            case 4:
                                wonAmount = myTicket.Amount * 20;
                                break;
                        }
                        break;
                    case 5:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Amount * 1;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                wonAmount = myTicket.Amount * 1;
                                break;
                            case 4:
                                wonAmount = myTicket.Amount * 4;
                                break;
                            case 5:
                                wonAmount = myTicket.Amount * 50;
                                break;
                        }
                        break;
                    case 6:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Amount * 1;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                wonAmount = myTicket.Amount * 2;
                                break;
                            case 5:
                                wonAmount = myTicket.Amount * 20;
                                break;
                            case 6:
                                wonAmount = myTicket.Amount * 200;
                                break;
                        }
                        break;
                    case 7:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Amount * 2;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                wonAmount = myTicket.Amount * 1;
                                break;
                            case 5:
                                wonAmount = myTicket.Amount * 4;
                                break;
                            case 6:
                                wonAmount = myTicket.Amount * 40;
                                break;
                            case 7:
                                wonAmount = myTicket.Amount * 600;
                                break;
                        }
                        break;
                    case 8:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Amount * 2;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                wonAmount = myTicket.Amount * 1;
                                break;
                            case 5:
                                wonAmount = myTicket.Amount * 2;
                                break;
                            case 6:
                                wonAmount = myTicket.Amount * 10;
                                break;
                            case 7:
                                wonAmount = myTicket.Amount * 100;
                                break;
                            case 8:
                                wonAmount = myTicket.Amount * 2000;
                                break;
                        }
                        break;
                    case 9:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Amount * 3;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                break;
                            case 5:
                                wonAmount = myTicket.Amount * 1;
                                break;
                            case 6:
                                wonAmount = myTicket.Amount * 5;
                                break;
                            case 7:
                                wonAmount = myTicket.Amount * 50;
                                break;
                            case 8:
                                wonAmount = myTicket.Amount * 400;
                                break;
                            case 9:
                                wonAmount = myTicket.Amount * 7000;
                                break;
                        }
                        break;
                    case 10:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Amount * 3;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                break;
                            case 5:
                                wonAmount = myTicket.Amount * 1;
                                break;
                            case 6:
                                wonAmount = myTicket.Amount * 2;
                                break;
                            case 7:
                                wonAmount = myTicket.Amount * 10;
                                break;
                            case 8:
                                wonAmount = myTicket.Amount * 140;
                                break;
                            case 9:
                                wonAmount = myTicket.Amount * 1400;
                                break;
                            case 10:
                                wonAmount = myTicket.Amount * 20000;
                                break;
                        }
                        break;
                }
                myTicket.WonAmount += wonAmount;
                myTicket.Results.Add(new Result(wonNumbersAmount, wonAmount));
            }
        }
    }

    public class Result
    {
        public Result(int wonNumbersAmount, double wonAmount)
        {
            WonAmount = wonAmount;
            WonNumbersAmount = wonNumbersAmount;
        }

        public int WonNumbersAmount { get; set; }

        public double WonAmount { get; set; }
    }
}