using System;
using UnityEngine;

namespace LordBreakerX.Stats
{
    [Serializable]
    public abstract class Stat<TValue> : Stat
    {
        [SerializeField]
        private TValue _baseValue;

        //private List<IStatModifier<TValue>> _modifiers = new List<IStatModifier<TValue>>();

        protected sealed override Type ValueType => typeof(TValue);

        public sealed override object GetValue()
        {
            return GetTypedValue();
        }

        public TValue GetTypedValue()
        {
            TValue value = _baseValue;

            //foreach (var modifier in _modifiers)
            //{
            //    modifier.Apply(value);
            //}

            return value;
        }

        public sealed override void SetValue(object value)
        {
            if (value == null)
            {
                _baseValue = default;
            }
            else if (value is TValue typedValue)
            {
                _baseValue = typedValue;
            }
            else
            {
                Type valueType = _baseValue.GetType();
                throw new InvalidCastException($"Provided value is not of type {valueType}");
            }
        }

        //public void AddModifier(IStatModifier<TValue> modifier)
        //{
        //    _modifiers.Add(modifier);
        //}
    }

    [Serializable]
    public abstract class Stat
    {
        [SerializeField]
        private string _id;

        protected abstract Type ValueType { get; }

        public abstract object GetValue();

        public abstract void SetValue(object value);

        public bool SetId(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                _id = id;
                return true;
            }

            return false;
        }

        public string GetId()
        {
            return _id;
        }
    }
}
