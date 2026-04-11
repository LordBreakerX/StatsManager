using UnityEngine;
using UnityEngine.Events;

namespace LordBreakerX.Stats
{
    public class StatManager : MonoBehaviour
    {
        #region Constants
        private const string INSTANCE_NAME = "Stat Manager";

        #endregion

        #region Variables
        private static StatManager _instance;

        private UnityEvent<StatContext> _onStatUpdate;

        #endregion

        #region Singleton Creation
        private static StatManager Singleton
        {
            get
            {
                if (_instance == null)
                {
                    GameObject instanceObject = new GameObject();
                    instanceObject.name = INSTANCE_NAME;
                    _instance = instanceObject.AddComponent<StatManager>();
                    GameObject.DontDestroyOnLoad(instanceObject);
                }

                return _instance;
            }
        }

        private void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
                GameObject.DontDestroyOnLoad(_instance);
            }
            else if (_instance != this)
            {
#if UNITY_EDITOR
                Debug.LogWarning("");
#endif
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
        #endregion

        #region Register Events
        public static void RegisterListener(UnityAction<StatContext> statUpdateCallback)
        {
            Singleton._onStatUpdate.AddListener(statUpdateCallback);
        }

        public static void UnregisterListener(UnityAction<StatContext> statUpdateCallback)
        {
            Singleton._onStatUpdate.RemoveListener(statUpdateCallback);
        }
        #endregion

        #region Update Stats
        public static void UpdateStats(StatProfile profile)
        {
            foreach (Stat stat in profile.Stats)
            {
                StatContext context = new StatContext(profile, stat);
                Singleton._onStatUpdate?.Invoke(context);
            }
        }

        public static void UpdateStats(StatProfilesAsset profilesAsset, string profileID)
        {
            StatProfile profile = profilesAsset.GetProfile(profileID);

            if (profile != null)
            {
                UpdateStats(profile);
            }
        }

        #endregion


    }
}
