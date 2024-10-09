using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float swordAttackCD = .5f;

    // PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;
    //private bool attackButtonDown, isAttacking = false;

    private GameObject SlashAnim;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        //playerControls = new PlayerControls();
    }

    //private void OnEnable()
    //{
    //    playerControls.Enable();
    //}

    //private void Start()
    //{
    //    playerControls.Combat.Attack.started += _ => StartAttacking();
    //    playerControls.Combat.Attack.canceled += _ => StopAttacking();
    //}

    private void Update()
    {
        MouseFollowWithOffset();   
        //Attack();
    }

    //private void StartAttacking()
    //{
    //    attackButtonDown = true;
    //}

    //private void StopAttacking()
    //{
    //    attackButtonDown = false;
    //}

    public void Attack()
    {
        //myAnimator.SetTrigger("Attack");
        //weaponCollider.gameObject.SetActive(true);

        //SlashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        //SlashAnim.transform.parent = this.transform.parent;
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);
            SlashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            SlashAnim.transform.parent = this.transform.parent;
            StartCoroutine(AttackCDRountine());
    }
    private IEnumerator AttackCDRountine()
    {
        yield return new WaitForSeconds(swordAttackCD);
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    private void SwingUpFlipAnimEvent()
    {
        SlashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (playerController.FacingLeft)
        {
            SlashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void SwingDownFlipAnimEvent()
    {
        SlashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController.FacingLeft)
        {
            SlashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if(mousePos.x < playerScreenPoint.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
