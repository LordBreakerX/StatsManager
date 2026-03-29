namespace LordBreakerX.Stats
{
    [System.Serializable]
    public abstract class StatModifier
    {
        public StatModifier()
        {

        }

        public abstract float Apply(float currentValue, float baseValue);

        public abstract StatModifier Copy();
    }
}
