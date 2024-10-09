using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    [SerializeField] private MonoBehaviour currrentActiveWeapon;

    private PlayerControls playerControls;

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
    }

    private void Update()
    {
        Attack();
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
            isAttacking = true;
            (currrentActiveWeapon as IWeapon).Attack();           
        }
    }
}
