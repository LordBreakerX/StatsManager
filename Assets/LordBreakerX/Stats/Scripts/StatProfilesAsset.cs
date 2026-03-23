using System.Collections.Generic;
using UnityEngine;

namespace LordBreakerX.Stats
{
    [CreateAssetMenu(menuName = "Stats/Profiles Asset")]
    public class StatProfilesAsset : ScriptableObject
    {
        [SerializeField]
        private List<StatProfile> _profiles = new List<StatProfile>();

        public List<StatProfile> Profiles { get => _profiles; }

        private Dictionary<string, StatProfile> _profilesRegistry = new Dictionary<string, StatProfile>();

        private void OnEnable()
        {
            _profilesRegistry.Clear();

            foreach (StatProfile profile in _profiles)
            {
                _profilesRegistry[profile.ID] = profile;
            }
        }

        public StatProfile GetProfile(string profileID) 
        {
            if (_profilesRegistry.ContainsKey(profileID))
                return _profilesRegistry[profileID];

            return null;
        }
    }
}
