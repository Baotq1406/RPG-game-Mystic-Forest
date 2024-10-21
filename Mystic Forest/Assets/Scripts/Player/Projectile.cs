using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;

    //private WeaponInfo weaponInfo;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
    }

    //public void UpdateWeaponInfo(WeaponInfo weaponInfo)
    //{
    //    this.weaponInfo = weaponInfo;
    //}

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed) { 
        this.moveSpeed = moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Checks if the object it collided with has an enemy.
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

        //Checks if the object has an Indestructible component, it's something like a wall that can't be destroyed.
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();

        //Checks if the object it collided with has an player.
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        if (!other.isTrigger && (enemyHealth || indestructible || player))
        {
            if ((player && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            {
                player?.TakeDamage(1, transform);
                //The visual effects are instantiated at the projectile’s current position.
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if (!other.isTrigger && indestructible)
            {
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            //function reduces the enemy's health by the weapon's damage
            //enemyHealth?.TakeDam(weaponInfo.weaponDam);
        }
    }

    private void DetectFireDistance()
    {
        //If the projectile’s current position
        //is farther from its starting position than the weapon's maximum range,
        //the projectile is destroyed.
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}
