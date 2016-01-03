using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using GreatRandom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MainWindow window = new MainWindow();
            for (int i = 0; i < 1000; i++)
            {
                var player = window.CreatePlayer(window.PlayerCounter++);
                Assert.AreEqual(0, player.Tickets.Count);
                Assert.AreEqual(1000, player.Money);
                Assert.IsTrue(player.NumberOfTickets > 0);
                Assert.IsTrue(player.NumbersAmount >= player.System);
                Assert.IsTrue(player.NumberOfTickets * player.Stake <= player.Money, player.ToString());
                Assert.IsTrue(player.ColdRange >= player.ColdNumbers);
                Assert.IsTrue(player.HotRange >= player.HotNumbers);
                if (player.NumbersAmount != player.System)
                    Assert.AreEqual(player.NumberOfTickets, MainWindow.Combinations(player.NumbersAmount, player.System), 0, player.ToString());
                Assert.AreEqual((window.PlayerCounter - 1).ToString(), player.Name);

                window.GenerateNumbersBuyTickets(player, window.NumbersStatistic);
                foreach (var ticket in player.Tickets)
                {
                    Assert.AreEqual(ticket.Stake, player.Stake);
                    //Assert.AreEqual(ticket.Numbers.Count, player.System);
                    foreach (var number in ticket.Numbers)
                    {
                        Assert.IsTrue(number > 0);
                    }
                }
                Assert.AreEqual(player.Tickets.Count, player.NumberOfTickets, 0, player.ToString());
                Assert.AreEqual(1000 - player.Money, player.NumberOfTickets * player.Stake, 0, player.ToString());
                Assert.AreEqual(1000 - player.Money, player.SpendMoney, 0, player.ToString());
                //                Console.WriteLine(i);

            }


        }

        [TestMethod]
        public void TestMethodCombinations()
        {
            MainWindow window = new MainWindow();

            var array = window.GenerateRandomsArray(6);
            int[] byteArray = new int[6];
            array.CopyTo(byteArray);
            Assert.AreEqual((int)MainWindow.Combinations(6, 2), MainWindow.GenerateAllPermutations(byteArray, 2).ToArray().Length);
        }


        [TestMethod]
        [DeploymentItem("test.xml")]
        public void GenerateTickets()
        {
            MainWindow window = new MainWindow();
            window.generator = new KenoLoader();
            var generator = (KenoLoader)window.generator;
            generator.Path = "test.xml";
            generator.Load();
            var player = new Player(8, "Loaded4", 28, 1, false, 100, 6, 7, 0, 11, 21, 1);

            for (int i = 7800; i < generator.results.Count; i++)
            {
                var numbers = window.generator.Generate();
                foreach (var numberStat in window.NumbersStatistic)
                {
                    numberStat.Appear(numbers.Contains(numberStat.Number));
                }
                var tempStat = new SortableObservableCollection<NumberStat>();
                tempStat.Sync(window.NumbersStatistic);
                tempStat.Sort(x => x.TimesAppear(player.StatRange), ListSortDirection.Descending);

                window.GenerateNumbersBuyTickets(player, window.NumbersStatistic);
                var wonMoney = Calculate.CalculateTickets(numbers, player.Tickets);
                Console.WriteLine(wonMoney);


            }
            

            //foreach (var ticket in player.Tickets)
            //{
            //    foreach (var number in ticket.Numbers)
            //    {
            //        Console.Write(number + ";");
            //    }
            //    Console.WriteLine("\t" + ticket.wonNumbers + " " + ticket.wonAmount);
            //}
            Console.WriteLine();

            window.GenerateNumbersBuyTickets(player, window.NumbersStatistic);

            foreach (var ticket in player.Tickets)
            {
                foreach (var number in ticket.Numbers)
                {
                    Console.Write(number + " ");
                }
                Console.WriteLine();
            }

        }

        [TestMethod]
        public void TestMethodPlayer()
        {
            MainWindow window = new MainWindow();

            var player = window.CreatePlayer(window.PlayerCounter++);
            player.NumbersAmount = 2;
            player.System = 2;
            player.Stake = 2;
            player.SameNumbers = true;
            player.NumberOfTickets = (int)MainWindow.Combinations(player.NumbersAmount, player.System);
            Assert.AreEqual(player.NumberOfTickets, MainWindow.Combinations(player.NumbersAmount, player.System), 0, "Numbers:" + player.NumbersAmount.ToString() + "system:" + player.System.ToString());


            window.GenerateNumbersBuyTickets(player, window.NumbersStatistic);
            Assert.AreEqual(player.Tickets.Count, player.NumberOfTickets, 0, player.ToString());

            Assert.AreEqual(1000 - player.Money, player.NumberOfTickets * player.Stake, 0, player.ToString());
            Assert.AreEqual(1000 - player.Money, player.SpendMoney, 0, player.ToString());

            foreach (var ticket in player.Tickets)
            {
                Assert.AreEqual(ticket.Stake, player.Stake);
                foreach (var number in ticket.Numbers)
                {
                    Assert.IsTrue(number > 0);
                }
            }


        }

        [TestMethod]
        public void TestMethodPlayerHotCold()
        {
            MainWindow window = new MainWindow();

            window.NumbersStatistic = new SortableObservableCollection<NumberStat>();
            window.NumbersStatistic.Add(new NumberStat() { Number = 1, });
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 2, });
            window.NumbersStatistic[1].Appear(true);
            window.NumbersStatistic[1].Appear(true);
            window.NumbersStatistic[1].Appear(true);
            window.NumbersStatistic[1].Appear(true);
            window.NumbersStatistic[1].Appear(true);
            window.NumbersStatistic[1].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 3, });
            window.NumbersStatistic[2].Appear(true);
            window.NumbersStatistic[2].Appear(true);
            window.NumbersStatistic[2].Appear(true);
            window.NumbersStatistic[2].Appear(true);
            window.NumbersStatistic[2].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 4 });
            window.NumbersStatistic[3].Appear(true);
            window.NumbersStatistic[3].Appear(true);
            window.NumbersStatistic[3].Appear(true);
            window.NumbersStatistic[3].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 5, });
            window.NumbersStatistic[4].Appear(true);
            window.NumbersStatistic[4].Appear(true);
            window.NumbersStatistic[4].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 6, });
            window.NumbersStatistic[5].Appear(true);
            window.NumbersStatistic[5].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 7, });
            window.NumbersStatistic[6].Appear(true);

            var player = window.CreatePlayer(window.PlayerCounter++);
            player.NumbersAmount = 10;
            player.NumberOfTickets = 1;
            player.System = 10;
            player.Stake = 2;
            player.HotNumbers = 2;
            player.HotRange = 2;
            player.ColdNumbers = 3;
            player.ColdRange = 3;
            player.SameNumbers = true;
            player.StatRange = 8;
            player.NumberOfTickets = (int)MainWindow.Combinations(player.NumbersAmount, player.System);
            Assert.AreEqual(player.NumberOfTickets, MainWindow.Combinations(player.NumbersAmount, player.System), 0, "Numbers:" + player.NumbersAmount.ToString() + "system:" + player.System.ToString());


            window.GenerateNumbersBuyTickets(player, window.NumbersStatistic);
            Assert.AreEqual(player.Tickets.Count, player.NumberOfTickets, 0, player.ToString());

            Assert.AreEqual(1000 - player.Money, player.NumberOfTickets * player.Stake, 0, player.ToString());
            Assert.AreEqual(1000 - player.Money, player.SpendMoney, 0, player.ToString());

            foreach (var ticket in player.Tickets)
            {
                Assert.AreEqual(ticket.Stake, player.Stake);
                Assert.IsTrue(ticket.Numbers.Contains(1));
                Assert.IsTrue(ticket.Numbers.Contains(2));
                Assert.IsTrue(ticket.Numbers.Contains(6));
                Assert.IsTrue(ticket.Numbers.Contains(7));
                Assert.IsTrue(ticket.Numbers.Contains(5));
                foreach (var number in ticket.Numbers)
                {
                    Assert.IsTrue(number > 0);
                }
            }


        }
        [TestMethod]
        public void TestMethodPlayerHotColdSystem()
        {
            MainWindow window = new MainWindow();

            window.NumbersStatistic = new SortableObservableCollection<NumberStat>();
            window.NumbersStatistic.Add(new NumberStat() { Number = 1, });
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);
            window.NumbersStatistic[0].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 2, });
            window.NumbersStatistic[1].Appear(true);
            window.NumbersStatistic[1].Appear(true);
            window.NumbersStatistic[1].Appear(true);
            window.NumbersStatistic[1].Appear(true);
            window.NumbersStatistic[1].Appear(true);
            window.NumbersStatistic[1].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 3, });
            window.NumbersStatistic[2].Appear(true);
            window.NumbersStatistic[2].Appear(true);
            window.NumbersStatistic[2].Appear(true);
            window.NumbersStatistic[2].Appear(true);
            window.NumbersStatistic[2].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 4 });
            window.NumbersStatistic[3].Appear(true);
            window.NumbersStatistic[3].Appear(true);
            window.NumbersStatistic[3].Appear(true);
            window.NumbersStatistic[3].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 5, });
            window.NumbersStatistic[4].Appear(true);
            window.NumbersStatistic[4].Appear(true);
            window.NumbersStatistic[4].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 6, });
            window.NumbersStatistic[5].Appear(true);
            window.NumbersStatistic[5].Appear(true);

            window.NumbersStatistic.Add(new NumberStat() { Number = 7, });
            window.NumbersStatistic[6].Appear(true);
            var player = window.CreatePlayer(window.PlayerCounter++);
            player.NumbersAmount = 10;
            player.System = 7;
            player.Stake = 2;
            player.HotNumbers = 2;
            player.HotRange = 2;
            player.ColdNumbers = 3;
            player.ColdRange = 3;
            player.SameNumbers = true;
            player.NumberOfTickets = (int)MainWindow.Combinations(player.NumbersAmount, player.System);
            Assert.AreEqual(player.NumberOfTickets, MainWindow.Combinations(player.NumbersAmount, player.System), 0, "Numbers:" + player.NumbersAmount.ToString() + "system:" + player.System.ToString());


            window.GenerateNumbersBuyTickets(player, window.NumbersStatistic);
            Assert.AreEqual(player.Tickets.Count, player.NumberOfTickets, 0, player.ToString());

            Assert.AreEqual(1000 - player.Money, player.NumberOfTickets * player.Stake, 0, player.ToString());
            Assert.AreEqual(1000 - player.Money, player.SpendMoney, 0, player.ToString());

            Assert.IsTrue(player.Tickets.Any(x => x.Numbers.Contains(1)));
            Assert.IsTrue(player.Tickets.Any(x => x.Numbers.Contains(2)));
            Assert.IsTrue(player.Tickets.Any(x => x.Numbers.Contains(6)));
            Assert.IsTrue(player.Tickets.Any(x => x.Numbers.Contains(7)));
            Assert.IsTrue(player.Tickets.Any(x => x.Numbers.Contains(5)));

            foreach (var ticket in player.Tickets)
            {
                Assert.AreEqual(ticket.Stake, player.Stake);
                foreach (var number in ticket.Numbers)
                {
                    Assert.IsTrue(number > 0);
                }
            }


        }
    }
}
