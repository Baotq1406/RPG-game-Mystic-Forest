using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlimeAttacking : MonoBehaviour, IEnemy 
{
    private Animator animator; // Reference to the Animator
    private EnemyAI enemyAI;


    // Start is called before the first frame update
    private void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
        enemyAI = new EnemyAI();
    }

    public void Attack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        if (distanceToPlayer < enemyAI.AttackRange)
        {
            // Trigger the "Attack" animation
            animator.SetTrigger("Attack");
        }
    }

}
