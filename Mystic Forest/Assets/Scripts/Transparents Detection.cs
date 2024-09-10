using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tran : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = .4f;

    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    //Ki?m tra xem ??i t??ng(ng??i ch?i) ?ã va ch?m(?i vào) v?i tilemap ho?c sprite(Khu v?c ??c bi?t) 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>()) {
            if (spriteRenderer)
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));
            }
            else if (tilemap) {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount));
            }
        }
    }
    //Ki?m tra xem ??i t??ng ?ã ?i ra kh?i khu v?c ??c bi?t 
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if(spriteRenderer)
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1f));
            } else if (tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));
            }
        }
    }

    //Hi?u ?ng làm m? cho sprire  
    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetTransparency) {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime) { 
            elapsedTime += Time.deltaTime;
            float newAlpha  = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime ); //Làm m? theo ki?u suy d?n t? giá tr? startValue ??n giá tr? cu?i cùng targetTransparency
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }
    }

    //Hi?u ?ng làm m? cho tilemap
    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime) { 
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
    }
}
