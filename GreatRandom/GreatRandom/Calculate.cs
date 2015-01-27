using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace GreatRandom
{
    public class Calculate
    {

        public static double CalculateTickets(HashSet<byte> numbers, SortableObservableCollection<Ticket> myTickets)
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
                                return wonAmount;
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
                                return wonAmount;
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
                                return wonAmount;
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
                                return wonAmount;
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
                                return wonAmount;
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
                                return wonAmount;
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
                                return wonAmount;
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
                                return wonAmount;
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
                                return wonAmount;
                                break;
                        }
                        break;
                }
                myTicket.WonAmount += wonAmount;
                var results = myTicket.Results;
                MainWindow.DispatcherThread.Invoke(() =>
                {
                    results.Add(new Result(results.Count + 1, wonNumbersAmount, wonAmount));
                    results.Sort(x => x.GameNumber, ListSortDirection.Descending);
                });
            }
            return 0;
        }
    }

    public class Result
    {
        public Result(int index,int wonNumbersAmount, double wonAmount)
        {
            GameNumber = index;
            WonAmount = wonAmount;
            WonNumbersAmount = wonNumbersAmount;
        }

        public int GameNumber { get; set; }
        public int WonNumbersAmount { get; set; }

        public double WonAmount { get; set; }
    }
}