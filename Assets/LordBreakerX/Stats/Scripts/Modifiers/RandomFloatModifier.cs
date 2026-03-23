using UnityEngine;

namespace LordBreakerX.Stats
{
    [System.Serializable]
    [CustomStatModifier("Random Number", StatType.Float)]
    public class RandomFloatModifier : StatModifier
    {
        [SerializeField]
        [Min(0)]
        private float _min = 0;

        [SerializeField]
        [Min(0)]
        private float _max = 0;

        public override float Apply(float currentValue, float baseValue)
        {
            float currentMin = currentValue + _min;
            float currentMax = currentValue + _max;
            return Random.Range(currentMin, currentMax);
        }
    }
}
