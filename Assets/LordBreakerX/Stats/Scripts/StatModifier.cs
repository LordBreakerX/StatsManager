namespace LordBreakerX.Stats
{
    [System.Serializable]
    public class StatModifier
    {
        [UnityEngine.SerializeField]
        private float _myValue;

        public StatModifier()
        {

        }

        public virtual float Apply(float currentValue, float baseValue)
        {
            return currentValue;
        }

        public virtual StatModifier Copy()
        {
            return new StatModifier();
        }
    }
}
