using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace StatelessDeterministicPseudoRandom
{
    public class ListRandomizer<T>
    {
        private readonly SeedInstance _seed;
        private readonly List<T> _list;
        private readonly Random _rng;
        
        public ListRandomizer(SeedInstance seed, IEnumerable<T> list)
        {
            _seed = seed;
            _rng = new Random(_seed.Value);

            _list = list.ToList();
            Randomize();
        }
        
        public T GetTodaysItem()
        {
            return _list[_seed.DaysSinceRollover];
        }

        public T GetRandomItemOutsideRolloverScope()
        {
            // can't use fixed-seed random here, else every subsequent call would return the very same item 
            var random = new Random();
            var index = random.Next(SeedGenerator.DaysPerSeed, _list.Count);
            return _list[index];
        }

        private void Randomize()
        {
            var n = _list.Count;

            // Fisher-Yates shuffle
            while (n > 1)
            {
                n--;
                var k = _rng.Next(n + 1);
                T item = _list[k];
                _list[k] = _list[n];
                _list[n] = item;
            }
        }
    }
}