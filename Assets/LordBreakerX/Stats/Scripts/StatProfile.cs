using System;
using System.Collections.Generic;
using UnityEngine;

namespace LordBreakerX.Stats
{
    public class StatProfile : ScriptableObject
    {
        [SerializeField]
        private List<Stat> _stats = new List<Stat>();

        private Dictionary<string, Stat> _statRegsitry = new Dictionary<string, Stat>();

        public string ID { get { return name; } }

        public List<Stat> Stats { get { return _stats; } }

        private void OnEnable()
        {
            //_statRegsitry.Clear();

            //foreach (Stat stat in _stats) 
            //{
            //    _statRegsitry[stat.Id] = stat;
            //}
        }

        public Stat GetStat(string id)
        {
            if (_statRegsitry.ContainsKey(id))
            {
                return _statRegsitry[id];
            }

            return null;
        }

        public T GetStatValue<T>(string id) where T : class
        {
            if (_statRegsitry[id] is Stat<T> stat)
            {
                return stat.GetTypedValue();
            }

            throw new InvalidCastException($"Stat {id} is not of type {typeof(T)}");
        }
    }
}
