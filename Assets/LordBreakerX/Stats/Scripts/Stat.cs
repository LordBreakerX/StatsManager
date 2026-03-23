using System;
using System.Collections.Generic;

namespace LordBreakerX.Stats
{
    public abstract class Stat<TValue> : Stat
    {
        private TValue _baseValue;

        private List<IStatModifier<TValue>> _modifiers = new List<IStatModifier<TValue>>();

        public Stat(TValue baseValue)
        {
            _baseValue = baseValue;
        }

        public override Type ValueType => typeof(TValue);

        public override object GetValue()
        {
            return GetTypedValue();
        }

        public TValue GetTypedValue()
        {
            TValue value = _baseValue;

            foreach (var modifier in _modifiers)
            {
                modifier.Apply(value);
            }

            return value;
        }

        public void AddModifier(IStatModifier<TValue> modifier)
        {
            _modifiers.Add(modifier);
        }
    }

    public abstract class Stat
    {
        public string Id { get; set; }

        public abstract object GetValue();

        public abstract System.Type ValueType { get; }
    }
}
