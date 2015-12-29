using System;
using System.Linq;
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
                Assert.AreEqual(0,player.Tickets.Count);
                Assert.AreEqual(1000,player.Money);
                Assert.IsTrue(player.NumberOfTickets > 0);
                Assert.IsTrue(player.NumberOfTickets * player.Stake < player.Money);

                Assert.AreEqual(player.NumberOfTickets, MainWindow.Combinations(player.NumbersAmount, player.System), 0,  "Numbers:" +player.NumbersAmount.ToString() + "system:" + player.System.ToString());
                Assert.AreEqual((window.PlayerCounter-1).ToString(),player.Name);

                MainWindow.GenerateNumbersBuyTickets(player);
                foreach (var ticket in player.Tickets)
                {
                    Assert.AreEqual(ticket.Stake,player.Stake);
                    Assert.AreEqual(ticket.Numbers.Count,player.System);
                    foreach (var number in ticket.Numbers)
                    {
                        Assert.IsTrue(number > 0);
                    }
                }
                Assert.AreEqual(player.Tickets.Count, player.NumberOfTickets, 0, "Numbers:" + player.NumbersAmount.ToString() + "system:" + player.System.ToString() + "stake:" + player.Stake);
                Assert.AreEqual(1000 -player.Money,player.NumberOfTickets * player.Stake,0,player.ToString());
                Assert.AreEqual(1000 -player.Money,player.SpendMoney,0,player.ToString());


            }


        }

        [TestMethod]
        public void TestMethodCombinations()
        {
            var array = MainWindow.GenerateRandomsArray(6);
            byte[] byteArray = new byte[6];
            array.CopyTo(byteArray);
            Assert.AreEqual((int)MainWindow.Combinations(6, 2), MainWindow.GenerateAllPermutations(byteArray,2).ToArray().Length);
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


            MainWindow.GenerateNumbersBuyTickets(player);
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
    }
}
