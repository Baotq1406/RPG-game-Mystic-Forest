using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamSource : MonoBehaviour
{
    [SerializeField] private int damAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDam(damAmount);
    }
}
