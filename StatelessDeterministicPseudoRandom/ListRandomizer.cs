using System;
using System.Collections.Generic;
using NodaTime;

namespace StatelessDeterministicPseudoRandom
{
    public class ListRandomizer
    {
        private readonly SeedInstance _seed;
        private readonly Random _rng;
        
        public ListRandomizer()
        {
            _seed = new SeedGenerator(SystemClock.Instance).GetCurrent();
            _rng = new Random(_seed.Value);
        }
        
        public T GetTodaysItem<T>(List<T> list)
        {
            var randomizedList = Randomize(list, _seed.Value);

            return randomizedList[_seed.DaysSinceRollover];
        }

        public T GetRandomItemOutsideRolloverScope<T>(List<T> list)
        {
            var index = _rng.Next(SeedGenerator.DaysPerSeed, list.Count);
            return list[index];
        }

        private List<T> Randomize<T>(List<T> list, int seed)
        {
            var n = list.Count;

            // Fisher-Yates shuffle
            while (n > 1)
            {
                n--;
                var k = _rng.Next(n + 1);
                T item = list[k];
                list[k] = list[n];
                list[n] = item;
            }

            return list;
        }
    }
}