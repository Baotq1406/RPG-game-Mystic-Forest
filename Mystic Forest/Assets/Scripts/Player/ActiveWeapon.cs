using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    //[SerializeField] private MonoBehaviour currrentActiveWeapon;

    public MonoBehaviour CurrentActveWeapon {  get; private set; } 

    private PlayerControls playerControls;
    private float timeBetweenAttacks;

    private bool attackButtomDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        //_=> bieu thuc lambda
        //Su dung chuot trai de tan cong
        playerControls.Combat.Attack.started += _ => StartAttacking();
        //Dung tan cong 
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        AttackCooldown();
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActveWeapon = newWeapon;

        AttackCooldown();
        timeBetweenAttacks = (CurrentActveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    public void WeaponNull()
    {
        CurrentActveWeapon = null;
    }

    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
    } 

    private void StartAttacking()
    {
        attackButtomDown = true;
    }

    private void StopAttacking()
    {
        attackButtomDown = false;
    }

    private void Attack()
    {
        if (attackButtomDown && !isAttacking)
        {
            AttackCooldown();
            (CurrentActveWeapon as IWeapon).Attack();           
        }
    }
}
