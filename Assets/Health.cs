using LordBreakerX.Stats;
using UnityEngine;

public class Health : MonoBehaviour, IStatHandler
{
    [SerializeField]
    private StatProfile _profile;

    public float _maxHealth;

    [ContextMenu("Update Stats")]
    public void UpdateStats()
    {
        StatManager.UpdateStats(_profile);
    }

    public void OnStatUpdate(StatContext context)
    {
        Debug.Log("Stat Update");

        if (context.IsStat("Health"))
        {
            _maxHealth = context.GetValue();
        }
    }
}
