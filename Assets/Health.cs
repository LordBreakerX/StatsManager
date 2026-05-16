using LordBreakerX.Stats;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private StatHolder _holder;

    public float _maxHealth;

    private void Start()
    {
        _maxHealth = _holder.GetFloat("Health");
    }
}
