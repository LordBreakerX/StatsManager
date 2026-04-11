namespace LordBreakerX.Stats
{
    public readonly struct StatContext
    {
        public readonly StatProfile statProfile;
        public readonly Stat stat;

        public StatContext(StatProfile statProfile, Stat stat)
        {
            this.stat = stat;
            this.statProfile = statProfile;
        }

        public readonly float GetValue()
        {
            return this.stat.GetValue();
        }

        public readonly bool IsStat(string statID)
        {
            return this.stat.GetId() == statID;
        }

        public readonly bool IsStat(string profileId, string statID)
        {
            return this.statProfile.ID == profileId && this.stat.GetId() == statID;
        }
    }
}
