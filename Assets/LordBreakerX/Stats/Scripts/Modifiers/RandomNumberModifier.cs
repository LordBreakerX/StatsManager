using UnityEngine;

namespace LordBreakerX.Stats
{
    [System.Serializable]
    [CustomStatModifier("Random Int Modifier")]
    public class RandomIntModifier : RandomNumberModifier<int>
    {
        public RandomIntModifier(int min, int max) : base(min, max)
        {
        }

        public override int Apply(int currentValue)
        {
            return currentValue + Random.Range(MinOffset, MaxOffset);
        }
    }

    [System.Serializable]
    [CustomStatModifier("Random Float Modifier")]
    public class RandomFloatModifier : RandomNumberModifier<float>
    {
        public RandomFloatModifier(float min, float max) : base(min, max)
        {
        }

        public override float Apply(float currentValue)
        {
            return currentValue + Random.Range(MinOffset, MaxOffset);
        }
    }

    [System.Serializable]
    public abstract class RandomNumberModifier<T> : IStatModifier<T>
    {
        [SerializeField]
        private T _minOffset;

        [SerializeField]
        private T _maxOffset;

        public T MinOffset { get => _minOffset; }
        public T MaxOffset { get => _maxOffset; }

        public RandomNumberModifier(T min, T max)
        {
            _minOffset = min;
            _maxOffset = max;
        }

        public abstract T Apply(T currentValue);
    }
}
