using System;
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
                if (player.NumbersAmount != player.System)
                    Assert.AreEqual(player.NumberOfTickets, MainWindow.Combinations(player.NumbersAmount, player.System), 0, player.ToString());
                Assert.AreEqual((window.PlayerCounter - 1).ToString(), player.Name);

                window.GenerateNumbersBuyTickets(player);
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
            var array = MainWindow.GenerateRandomsArray(6);
            byte[] byteArray = new byte[6];
            array.CopyTo(byteArray);
            Assert.AreEqual((int)MainWindow.Combinations(6, 2), MainWindow.GenerateAllPermutations(byteArray, 2).ToArray().Length);
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


            window.GenerateNumbersBuyTickets(player);
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
            window.NumbersStatistic.Add(new NumberStat() { Number = 1, TimesAppear = 7 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 2, TimesAppear = 6 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 3, TimesAppear = 5 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 4, TimesAppear = 4 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 5, TimesAppear = 3 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 6, TimesAppear = 2 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 7, TimesAppear = 1 });
            var player = window.CreatePlayer(window.PlayerCounter++);
            player.NumbersAmount = 10;
            player.NumberOfTickets = 1;
            player.System = 10;
            player.Stake = 2;
            player.HotNumbers = 2;
            player.ColdNumbers = 3;
            player.SameNumbers = true;
            player.NumberOfTickets = (int)MainWindow.Combinations(player.NumbersAmount, player.System);
            Assert.AreEqual(player.NumberOfTickets, MainWindow.Combinations(player.NumbersAmount, player.System), 0, "Numbers:" + player.NumbersAmount.ToString() + "system:" + player.System.ToString());


            window.GenerateNumbersBuyTickets(player);
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
            window.NumbersStatistic.Add(new NumberStat() { Number = 1, TimesAppear = 7 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 2, TimesAppear = 6 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 3, TimesAppear = 5 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 4, TimesAppear = 4 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 5, TimesAppear = 3 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 6, TimesAppear = 2 });
            window.NumbersStatistic.Add(new NumberStat() { Number = 7, TimesAppear = 1 });
            var player = window.CreatePlayer(window.PlayerCounter++);
            player.NumbersAmount = 10;
            player.System = 7;
            player.Stake = 2;
            player.HotNumbers = 2;
            player.ColdNumbers = 3;
            player.SameNumbers = true;
            player.NumberOfTickets = (int)MainWindow.Combinations(player.NumbersAmount, player.System);
            Assert.AreEqual(player.NumberOfTickets, MainWindow.Combinations(player.NumbersAmount, player.System), 0, "Numbers:" + player.NumbersAmount.ToString() + "system:" + player.System.ToString());


            window.GenerateNumbersBuyTickets(player);
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
