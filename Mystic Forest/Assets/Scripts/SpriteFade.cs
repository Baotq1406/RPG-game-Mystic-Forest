using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    [SerializeField] private float fadeTime = .4f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //This method is a coroutine that gradually changes the alpha (transparency) value of the sprite's color over the specified
    public IEnumerator SlowFadeRoutine()
    {
        //This variable keeps track of how much time has passed since the fading started
        float elapsedTime = 0;
        //This holds the initial alpha value of the sprite (the starting transparency level)
        float startValue = spriteRenderer.color.a;

        while (elapsedTime < fadeTime) { 
            
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime/fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
