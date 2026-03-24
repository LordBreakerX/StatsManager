namespace LordBreakerX.Stats
{
    [System.Serializable]
    public class StatModifier
    {
        public StatModifier()
        {

        }

        public virtual float Apply(float currentValue, float baseValue)
        {
            return currentValue;
        }
    }
}
