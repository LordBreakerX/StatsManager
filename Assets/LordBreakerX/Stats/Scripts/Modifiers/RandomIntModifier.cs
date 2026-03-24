using UnityEngine;

namespace LordBreakerX.Stats
{
    [System.Serializable]
    [CustomStatModifier("Random Number", StatType.Int)]
    public sealed class RandomIntModifier : StatModifier
    {
        [SerializeField]
        [Min(0)]
        private int _min = 0;

        [SerializeField]
        [Min(0)]
        private int _max = 0;

        public sealed override float Apply(float currentValue, float baseValue)
        {
            int currentMin = (int)currentValue + _min;
            int currentMax = (int)currentValue + _max;
            return Random.Range(currentMin, currentMax + 1);
        }
    }
}
