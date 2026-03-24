using System.Collections.Generic;
using UnityEngine;

namespace LordBreakerX.Stats
{
    public class StatProfileTemplate : ScriptableObject
    {
        [SerializeField]
        private List<Stat> _stats = new List<Stat>();

        public IReadOnlyList<Stat> Stats { get { return _stats; } }

        public void SetStats(List<Stat> stats)
        {
            _stats = new List<Stat>();

            foreach (Stat stat in stats) 
            {
                _stats.Add(new Stat(stat));
            }
        }

        public List<Stat> CopyStats()
        {
            var copiedStats = new List<Stat>();

            foreach (Stat stat in _stats)
            {
                copiedStats.Add(new Stat(stat));
            }

            return copiedStats;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
