
using UnityEngine;
namespace LordBreakerX.Stats
{
    [System.Serializable]
    public sealed class Stat
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private float _baseValue;

        public float BaseValue { get => _baseValue; set => _baseValue = value; }

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
