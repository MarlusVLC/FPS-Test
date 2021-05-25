using System.Collections.Generic;
using UnityEngine;

namespace Auxiliary
{
    public static class MathTools
    {
        // xxxx0,3,0xxxx
        //0,3,1
        //0,3,2
        //0,3,3
        public static HashSet<int> GenerateDistinctNumbersWithinRange(int min, int max, int count)  //count 1 ou 2 ou 3
        {
            HashSet<int> result = new HashSet<int>();
            
            
            var possibilities = new List<int>(max-min);

            for (int i = min; i < possibilities.Capacity; i++)
            {
                possibilities.Add(i);
            }
            //[0,1,2]

            for (int i = 0; i < count; i++)
            {
                var value = possibilities[Random.Range(0, possibilities.Count)]; //0 ou 1 ou 2
                possibilities.Remove(value);
                result.Add(value);
            }
            return result;
        }
    }
}