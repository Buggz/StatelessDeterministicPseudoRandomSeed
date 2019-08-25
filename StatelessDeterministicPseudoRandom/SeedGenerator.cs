using System;
using NodaTime;

namespace StatelessDeterministicPseudoRandom
{
    public class SeedGenerator
    {
        private readonly IClock _clock;
        public static int DaysPerSeed = 10;

        /// <summary>Pass SystemClock.Instance in production</summary>
        public SeedGenerator(IClock clock)
        {
            _clock = clock;
        }
        
        public SeedInstance GetCurrent()
        {
            var now = _clock.GetCurrentInstant();
            // make sure to strip away anything but day.
            var today = now.InUtc().LocalDateTime.Date;
            var totalDays = (today - LocalDate.MinIsoValue).Days;

            var dayOfSeed = totalDays % 10;
            var seed = totalDays - dayOfSeed;
            
            return new SeedInstance(seed, dayOfSeed);
        }
    }
}