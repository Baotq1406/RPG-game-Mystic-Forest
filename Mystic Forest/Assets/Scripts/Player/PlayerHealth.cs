using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider healthSlider;
    private int curentHealth;   
    private bool canTakeDamage = true;
    private KnockBack knockBack;
    private Flash flash;
    const string HEALTH_SLIDER_TEXT = "Health Slider";

    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        curentHealth = maxHealth;

        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
        
        if (enemy && canTakeDamage)
        {
            //TakeDamage(1);
            //knockBack.GetKnockedBack(other.gameObject.transform, knockBackThrustAmount);
            //StartCoroutine(flash.FlashRoutine());

            if (enemy)
            {
                TakeDamage(1, other.transform);
            }
        }
    }

    public void HealPlayer()
    {
        if (curentHealth < maxHealth) 
        {
            curentHealth += 1;
            UpdateHealthSlider();
        }
        
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ShakeScreen();
        knockBack.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        curentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath() 
    {
        if (curentHealth <= 0) 
        { 
            curentHealth = 0;
            Debug.Log("Player Death");
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;

    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null) 
        { 
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = curentHealth;
    }
}
