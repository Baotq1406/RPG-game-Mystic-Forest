using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    -T?o hi?u ?ng ?o giác v? chi?u sâu trong 2D
    -B?ng cách di chuy?n các layer ? t?c ?? khác nhau
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
    //Nó tính toán v? trí m?i cho ??i t??ng
    private void FixedUpdate()
    {
        transform.position = startPos + travel * parallaxOffset;
    }
}
