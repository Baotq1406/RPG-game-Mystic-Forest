using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;
    [SerializeField] private bool MovingWhileAttacking = false;

    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private Vector2 attackPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathfinding enemyPathfinding;

    public float AttackRange { get { return attackRange; } }
    //public float GetAttackRange() { return attackRange; }

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        //StartCoroutine(RoamingRoutine());
        //Debug.Log("Roaming continue");

        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state) { 
            default:
            case State.Roaming:
                Roaming();
            break;

            case State.Attacking:
                Attacking();
            break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTo(roamPosition);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
            attackPosition = GetFollowingPlayer();

        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            else if (MovingWhileAttacking)
            {
                enemyPathfinding.MoveTo(attackPosition);
            }
            else 
            {
                enemyPathfinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    //private IEnumerator RoamingRoutine()
    //{
        
    //    while (state == State.Roaming)
    //    {
    //        //Debug.Log("Roaming");
    //        Vector2 roamPosition = GetRoamingPosition();
    //        //method is called to set the enemy's direction toward the new roaming position.
    //        enemyPathfinding.MoveTo(roamPosition);
    //        //pauses the coroutine for roamChangeDirFloat seconds before the loop repeats,
    //        //allowing the enemy to move toward the new position for a set duration before changing direction again.
    //        yield return new WaitForSeconds(roamChangeDirFloat);
    //        //Debug.Log("Roaming yield");
    //    }
    //}

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private Vector3 GetFollowingPlayer()
    {
        timeRoaming = 0f;
        Vector3 moveDir = (PlayerController.Instance.transform.position - transform.position).normalized; 
        return moveDir;
    }
}
