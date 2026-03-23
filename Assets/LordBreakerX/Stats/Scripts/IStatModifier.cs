using UnityEngine;

namespace LordBreakerX.Stats
{
    public interface IStatModifier<TValue>
    {
        public TValue Apply(TValue current);
    }
}
