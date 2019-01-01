using System;
using System.Collections.Generic;
using System.Text;

namespace EssentialTraining
{
    public class ProgrammingLoops
    {
        public int ForEachLoop()
        {
            var numbers = new List<int> { 1, 3, 5, 7, 9 };
            var sum = 0;
            foreach(var number in numbers)
            {
                sum += number;
            }
        }
    }
}
