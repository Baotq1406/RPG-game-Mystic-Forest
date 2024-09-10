using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    -T?o hi?u ?ng ?o gi�c v? chi?u s�u trong 2D
    -B?ng c�ch di chuy?n c�c layer ? t?c ?? kh�c nhau
 */

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxOffset = -0.15f;

    private Camera cam;
    private Vector2 startPos;
    private Vector2 travel => (Vector2)cam.transform.position - startPos;

    private void Awake()
    {
        cam = Camera.main;
    }
  
    private void Start()
    {
        startPos = transform.position;
    }
    //N� t�nh to�n v? tr� m?i cho ??i t??ng
    private void FixedUpdate()
    {
        transform.position = startPos + travel * parallaxOffset;
    }
}
