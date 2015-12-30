using System;
using System.Threading;
using GreatRandom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            SortableObservableCollection<int> list1 = new SortableObservableCollection<int>();
            var list2 = new SortableObservableCollection<int>();
            var random = new Random();
            var ready = false;
            var thread = new Thread(() =>
            {

                for (int i = 0; i < 100000; i++)
                {
                    list2.Insert(0,random.Next());
                    while (list2.Count > 100)
                    {
                        list2.RemoveAt(list2.Count-1);
                    }
                }
                ready = true;

            });
            thread.Start();
            for (int i = 0; i < 1000000; i++)
            {
                list1.Sync(list2);
            }
            while (!ready)
            {
                Thread.Sleep(1);
            }
            Assert.AreEqual(list1.Count,list2.Count);
            for (int i = 0; i < list1.Count; i++)
            {
                Assert.AreEqual(list1[i],list2[i]);
            }
        }
    }
}
