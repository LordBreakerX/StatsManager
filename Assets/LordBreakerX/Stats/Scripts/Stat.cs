
using System.Collections.Generic;
using UnityEngine;
namespace LordBreakerX.Stats
{
    [System.Serializable]
    public sealed class Stat
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private StatType _type = StatType.Float;

        [SerializeField]
        private float _baseValue;

        [SerializeField]
        private List<StatModifier> _modifiers = new List<StatModifier>();

        public StatType ValueType { get { return _type; } set { _type = value; } }

        public float BaseValue { get => _baseValue; set => _baseValue = value; }

        public IReadOnlyList<StatModifier> Modifiers { get { return _modifiers; } }

        public float GetValue()
        {
            float value = _baseValue;

            foreach (StatModifier modifier in _modifiers)
            {
                value = modifier.Apply(value, _baseValue);
            }

            return value;
        }

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

        public void AddModifier(StatModifier modifier)
        {
            if (modifier != null)
            {
                _modifiers.Add(modifier);
            } 
        }

    }
}
