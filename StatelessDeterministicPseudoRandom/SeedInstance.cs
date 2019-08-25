namespace StatelessDeterministicPseudoRandom
{
    public class SeedInstance
    {
        public SeedInstance(int value, int daysSinceRollover)
        {
            Value = value;
            DaysSinceRollover = daysSinceRollover;
        }

        /// <summary>Seed value to use in new System.Random</summary>
        public int Value { get; }
        /// <summary>Days since last seed value rollover, use as index</summary>
        public int DaysSinceRollover { get; }
    }
}