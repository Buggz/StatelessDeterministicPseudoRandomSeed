using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using StatelessDeterministicPseudoRandom;
using Xunit;

namespace Tests
{
    public class ListRandomizerTests
    {
        [Fact]
        public void Should_randomize_same_way_with_same_seed_value()
        {
            var list = Enumerable.Range(0, 50).Select(x => x).ToList();

            list.ForEach(i =>
            {
                var seed = new SeedInstance(123, i);
                var listRandomizer1 = new ListRandomizer<int>(seed, list);
                var listRandomizer2 = new ListRandomizer<int>(seed, list);

                listRandomizer1.GetTodaysItem().Should().Be(listRandomizer2.GetTodaysItem());
            });
        }

        [Fact]
        public void Random_should_be_outside_rollover_index_range()
        {
            var list = Enumerable.Range(0, 50).Select(x => x).ToList();

            var firstN = new List<int>();
            // get first N days
            for (var i = 0; i < SeedGenerator.DaysPerSeed; i++)
            {
                var seed = new SeedInstance(345, i);
                var listRand = new ListRandomizer<int>(seed, list);
                firstN.Add(listRand.GetTodaysItem());
            }

            var listRandomizer = new ListRandomizer<int>(new SeedInstance(345, 0), list);
            for (var i = 0; i < list.Count - SeedGenerator.DaysPerSeed; i++)
            {
                firstN.Should().NotContain(listRandomizer.GetRandomItemOutsideRolloverScope());
            }
        }
    }
}