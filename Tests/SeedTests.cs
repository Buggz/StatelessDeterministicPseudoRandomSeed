using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NodaTime;
using NodaTime.Testing;
using StatelessDeterministicPseudoRandom;
using Xunit;

namespace Tests
{
    public class SeedTests
    {
        [Fact]
        public void Days_per_seed_should_be_significant()
        {
            // avoid stupid mistakes?
            Seed.DaysPerSeed.Should().BeGreaterThan(1);
        }
        
        [Fact]
        public void Value_should_be_same_for_n_days()
        {
            var startInstant = Instant.MinValue;

            var seeds = new List<int>();
            
            for (var i = 0; i < Seed.DaysPerSeed; i++)
            {
                var fakeClock = new FakeClock(startInstant.Plus(Duration.FromDays(i)));
                var seed = new Seed(fakeClock);
                seeds.Add(seed.GetCurrent().Value);
            }

            var first = seeds.First();
            seeds.All(x => x == first).Should().BeTrue();
        }

        [Fact]
        public void Day_should_equal_number_of_days_since_last_rollover()
        {
            var startInstant = Instant.MinValue;

            for (var i = 0; i < Seed.DaysPerSeed; i++)
            {
                var fakeClock = new FakeClock(startInstant.Plus(Duration.FromDays(i)));
                var seed = new Seed(fakeClock);

                seed.GetCurrent().Day.Should().Be(i);
            }
        }

        [Fact]
        public void Value_should_be_different_after_n_plus_one_days()
        {
            var firstInstant = Instant.MinValue.Plus(Duration.FromDays(Seed.DaysPerSeed - 1));
            var secondInstant = Instant.MinValue.Plus(Duration.FromDays(Seed.DaysPerSeed));
            
            var firstSeed = new Seed(new FakeClock(firstInstant)).GetCurrent();
            var secondSeed = new Seed(new FakeClock(secondInstant)).GetCurrent();

            firstSeed.Should().NotBe(secondSeed);
        }
    }
}