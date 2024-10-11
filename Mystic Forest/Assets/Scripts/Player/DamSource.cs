using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamSource : MonoBehaviour
{
    //[SerializeField] private int damAmount = 1;
    private int damageAmount;

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDam;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDam(damageAmount);
    }
}
