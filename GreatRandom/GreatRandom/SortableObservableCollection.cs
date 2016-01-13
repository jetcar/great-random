using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GreatRandom
{
    public class SortableObservableCollection<T> : ObservableCollection<T>
    {


        
       


       

        public void Sort<TKey>(Func<T, TKey> keySelector, System.ComponentModel.ListSortDirection direction)
        {
            {
                switch (direction)
                {
                    case System.ComponentModel.ListSortDirection.Ascending:
                        {
                            ApplySort(Items.OrderBy(keySelector));
                            break;
                        }
                    case System.ComponentModel.ListSortDirection.Descending:
                        {
                            ApplySort(Items.OrderByDescending(keySelector));
                            break;
                        }
                }
            }
        }

        public void Sort<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
        {
            {
                ApplySort(Items.OrderBy(keySelector, comparer));
            }
        }

        private void ApplySort(IEnumerable<T> sortedItems)
        {
            var sortedItemsList = sortedItems.ToList();

            foreach (var item in sortedItemsList)
            {
                Move(IndexOf(item), sortedItemsList.IndexOf(item));
            }
        }



        public void Sync(SortableObservableCollection<T> list)
        {
            {
                {
                    var firstCount = list.Count;
                    for (int j = 0; j < list.Count; j++)
                    {
                        Debug.Assert(firstCount == list.Count);
                        if (this.Count - 1 < j)
                        {
                            this.Add(list[j]);
                        }
                        var item1 = this[j];
                        var item2 = list[j];
                        if (item1.Equals(item2))
                            continue;
                        this.SetItem(j, item2);
                    }
                    while (this.Count > firstCount)
                    {
                        this.RemoveAt(this.Count-1);
                    }
                }

            }
        }

    }
}
