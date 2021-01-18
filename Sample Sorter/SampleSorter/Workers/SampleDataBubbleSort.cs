using SampleSorter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleSorter.Workers
{
    class SampleDataBubbleSort
    {
        public List<SampleDataModel> BubbleSort(List<SampleDataModel> list)
        {
            int i, j;
            int n = list.Count;

            for (j = n - 1; j > 0; j--) //two for loops so it only scans from the begining of already sorted portion 
            {
                for (i = 0; i < j; i++)
                {
                    if (String.Compare(list[i].SampleText, list[i + 1].SampleText) > 0)
                    {
                        list.Reverse(i, 2);
                    }                        
                }
            }

            return list;
        }
    }
}
