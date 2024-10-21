using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2f;

    private bool isGrowing = true; 
    private float laserRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        //This method is triggered when the laser enters a 2D collider marked as a trigger
        if (other.gameObject.GetComponent<Indestructible>() && !other.isTrigger)
        {
            isGrowing = false;
        }
    }

    //This method allows the laser's range (length) to be updated externally
    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;

        //Once the new range is set,
        //it starts a coroutine called IncreaseLaserLengthRoutine() to gradually increase the laser’s length.
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    //coroutine gradually increases the laser's length over time
    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            //sprite
            //The visual size of the laser's sprite
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), 1f);

            //collider
            //These adjust the size and position of the collider so it matches the visual size of the laser
            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), capsuleCollider2D.size.y);
            capsuleCollider2D.offset = new Vector2((Mathf.Lerp(1f, laserRange, linearT)) / 2, capsuleCollider2D.offset.y);

            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    private void LaserFaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); 
        Vector2 direction = transform.position - mousePosition;
        //It then calculates the direction from the laser's position to the mouse pointer and rotates the laser to point toward the mouse
        transform.right = -direction;
    }
}
