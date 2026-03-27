
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
        [SerializeReference]
        private List<StatModifier> _modifiers = new List<StatModifier>();

        public int ModifierCount {  get { return _modifiers.Count; } }

        public Stat()
        {

        }

        public Stat(Stat toCopy)
        {
            _id = toCopy._id;
            _type = toCopy._type;
            _baseValue = toCopy._baseValue;

            _modifiers = new List<StatModifier>();

            foreach (StatModifier modifier in toCopy._modifiers)
            {
                _modifiers.Add(modifier.Copy());
            }
        }

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

        public void InsertModifier(StatModifier modifier, int index)
        {
            if (modifier != null && index <= _modifiers.Count)
            {
                _modifiers.Insert(index, modifier);
            }
        }

        public void RemoveModifier(int index)
        {
            _modifiers.RemoveAt(index);
        }
    }
}
