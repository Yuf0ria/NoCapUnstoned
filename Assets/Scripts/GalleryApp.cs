using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class GalleryApp : MonoBehaviour
{
    Vector2 scrollDir;
    float scrollSpeed = 0.25f;

    float scrollUpperLimit = 0;
    float scrollLowerLimit = 6.25f;
    [SerializeField] GameObject scrollObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scrollDir = Mouse.current.scroll.ReadValue();
        if (scrollDir.y > 0 && scrollObject.transform.position.y <= scrollLowerLimit)
            scrollObject.transform.position += new Vector3(0,scrollSpeed,0);
            
        if (scrollDir.y < 0  && scrollObject.transform.position.y >= scrollUpperLimit)
            scrollObject.transform.position -= new Vector3(0, scrollSpeed, 0);
    }
}
