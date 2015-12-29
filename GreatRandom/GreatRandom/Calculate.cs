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

        public static int CalculateTickets(HashSet<byte> numbers, IList<Ticket> myTickets)
        {
            int TotalWon = 0;
            foreach (var myTicket in myTickets)
            {
                var wonNumbersAmount = 0;
                foreach (var number in myTicket.Numbers)
                {
                    if (numbers.Contains(number))
                    {
                        wonNumbersAmount++;
                    }
                }
                int wonAmount = 0;
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
                                wonAmount = myTicket.Stake * 5;
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
                                wonAmount = myTicket.Stake * 1;
                                break;
                            case 3:
                                wonAmount = myTicket.Stake * 11;
                                break;
                        }
                        break;
                    case 4:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Stake * 1;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                wonAmount = myTicket.Stake * 2;
                                break;
                            case 4:
                                wonAmount = myTicket.Stake * 20;
                                break;
                        }
                        break;
                    case 5:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Stake * 1;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                wonAmount = myTicket.Stake * 1;
                                break;
                            case 4:
                                wonAmount = myTicket.Stake * 4;
                                break;
                            case 5:
                                wonAmount = myTicket.Stake * 50;
                                break;
                        }
                        break;
                    case 6:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Stake * 1;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                wonAmount = myTicket.Stake * 2;
                                break;
                            case 5:
                                wonAmount = myTicket.Stake * 20;
                                break;
                            case 6:
                                wonAmount = myTicket.Stake * 200;
                                break;
                        }
                        break;
                    case 7:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Stake * 2;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                wonAmount = myTicket.Stake * 1;
                                break;
                            case 5:
                                wonAmount = myTicket.Stake * 4;
                                break;
                            case 6:
                                wonAmount = myTicket.Stake * 40;
                                break;
                            case 7:
                                wonAmount = myTicket.Stake * 600;
                                break;
                        }
                        break;
                    case 8:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Stake * 2;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            case 4:
                                wonAmount = myTicket.Stake * 1;
                                break;
                            case 5:
                                wonAmount = myTicket.Stake * 2;
                                break;
                            case 6:
                                wonAmount = myTicket.Stake * 10;
                                break;
                            case 7:
                                wonAmount = myTicket.Stake * 100;
                                break;
                            case 8:
                                wonAmount = myTicket.Stake * 2000;
                                break;
                        }
                        break;
                    case 9:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Stake * 3;
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
                                wonAmount = myTicket.Stake * 1;
                                break;
                            case 6:
                                wonAmount = myTicket.Stake * 5;
                                break;
                            case 7:
                                wonAmount = myTicket.Stake * 50;
                                break;
                            case 8:
                                wonAmount = myTicket.Stake * 400;
                                break;
                            case 9:
                                wonAmount = myTicket.Stake * 7000;
                                break;
                        }
                        break;
                    case 10:
                        switch (wonNumbersAmount)
                        {
                            case 0:
                                wonAmount = myTicket.Stake * 3;
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
                                wonAmount = myTicket.Stake * 1;
                                break;
                            case 6:
                                wonAmount = myTicket.Stake * 2;
                                break;
                            case 7:
                                wonAmount = myTicket.Stake * 10;
                                break;
                            case 8:
                                wonAmount = myTicket.Stake * 140;
                                break;
                            case 9:
                                wonAmount = myTicket.Stake * 1400;
                                break;
                            case 10:
                                wonAmount = myTicket.Stake * 20000;
                                break;
                        }
                        break;
                }
                myTicket.IsWon = wonAmount > myTicket.Stake;
                TotalWon += wonAmount;
            }
            return TotalWon;
        }
    }

}